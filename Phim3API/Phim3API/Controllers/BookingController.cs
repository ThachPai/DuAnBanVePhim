using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Phim3API.Data;
using Phim3API.Hubs;
using Phim3API.Models;
using Phim3API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phim3API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<SeatHub> _hubContext;
        public class SeatSelectionRequest
        {
            public int ShowtimeId { get; set; }
            public string SeatName { get; set; }
            public int UserId { get; set; }
        }
        public BookingController(AppDbContext context, IHubContext<SeatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet("showtime/{showtimeId}/seats")]
        public async Task<IActionResult> GetSeats(int showtimeId) // Đổi hẳn thành showtimeId cho đồng bộ
        {
            // 1. Tìm suất chiếu
            var showtime = await _context.Showtimes.FindAsync(showtimeId);
            if (showtime == null) return NotFound("Suất chiếu không tồn tại");

            // 2. Lấy danh sách ghế gốc theo RoomId
            var allSeats = await _context.Seats
                                         .Where(s => s.RoomId == showtime.RoomId)
                                         .ToListAsync();

            // 3. Lấy danh sách ghế đã đặt
            var bookedSeats = await _context.BookedSeats
                                             .Where(b => b.ShowtimeId == showtimeId)
                                             .ToListAsync();

            // 4. Ghép dữ liệu
            var result = allSeats.Select(seat =>
            {
                var booking = bookedSeats.FirstOrDefault(b => b.SeatId == seat.Id);

                return new
                {
                    ShowtimeId = showtimeId, // <-- THÊM DÒNG NÀY để Client nhận diện đúng phòng SignalR
                    SeatName = seat.RowName + "_" + seat.Number,
                    Status = booking != null ? booking.Status : "Available",
                    Price = seat.Type == "VIP" ? showtime.Price + 20000 : showtime.Price
                };
            });

            return Ok(result);
        }
        // 2. API Lấy Lịch sử đặt vé (Dành cho Admin xem thống kê)
        // Đây chính là hàm bạn cần để sửa lỗi 500
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var bookings = await _context.Bookings
                    .Include(b => b.User)      // Nối bảng User
                    .Include(b => b.Showtime)  // Nối bảng Suất chiếu
                    .OrderByDescending(b => b.BookingDate) // Mới nhất lên đầu
                    .Select(b => new
                    {
                        b.Id,
                        // Kiểm tra null an toàn cho User
                        Username = b.User != null ? b.User.Username : "Khách vãng lai",
                        UserId = b.UserId,

                        // Lấy tên phim
                        MovieTitle = b.MovieTitle, // Lấy từ booking (nếu có lưu)

                        // SỬA LỖI 500: Dùng đúng tên biến TotalAmount
                        TotalAmount = b.TotalAmount,

                        b.Quantity,
                        b.BookingDate,

                        // Kiểm tra null cho Status
                        Status = b.Status ?? "Completed"
                    })
                    .ToListAsync();

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                // In lỗi ra cửa sổ Output của Visual Studio
                Console.WriteLine($"Lỗi API History: {ex.Message}");
                return StatusCode(500, new { message = "Lỗi Server", error = ex.Message });
            }
        }
        [HttpGet("get-by-movie/{movieId}")]
        public async Task<IActionResult> GetShowtimesByMovie(int movieId)
        {
            // Tìm tất cả suất chiếu của phim này
            var showtimes = await _context.Showtimes
                .Where(s => s.MovieId == movieId)
                .Select(s => new { s.Id, s.StartTime, s.Price })
                .ToListAsync();

            // Nếu không thấy, trả về mảng rỗng [] thay vì lỗi 404 để WinForms dễ xử lý
            return Ok(showtimes);
        }
        private static readonly Dictionary<int, Dictionary<string, string>> _tempSeats = new();

        [HttpPost("toggle-seat")]
        public async Task<IActionResult> ToggleSeat([FromBody] SeatSelectionRequest request)
        {
            if (!_tempSeats.ContainsKey(request.ShowtimeId))
                _tempSeats[request.ShowtimeId] = new Dictionary<string, string>();

            var seats = _tempSeats[request.ShowtimeId];

            // Nếu ghế đã bị người khác giữ (Holding)
            if (seats.ContainsKey(request.SeatName) && seats[request.SeatName] != request.UserId.ToString())
            {
                return BadRequest("Ghế này đã có người đang chọn!");
            }

            // Nếu đang giữ thì bỏ, nếu chưa thì giữ
            if (seats.ContainsKey(request.SeatName))
                seats.Remove(request.SeatName);
            else
                seats[request.SeatName] = request.UserId.ToString();

            // Thông báo cho tất cả qua SignalR
            string status = seats.ContainsKey(request.SeatName) ? "Holding" : "Free";
            await _hubContext.Clients.Group(request.ShowtimeId.ToString())
                .SendAsync("ReceiveSeatStatus", request.ShowtimeId, request.SeatName, status, request.UserId);

            return Ok(new { status });
        }
        // 3. API Đặt vé & Realtime
        [HttpPost("book")]
        public async Task<IActionResult> BookTickets([FromBody] BookingRequest request)
        {
            if (request.SelectedSeats == null || request.SelectedSeats.Count == 0)
                return BadRequest("Chưa chọn ghế nào!");

            // 1. Kiểm tra suất chiếu tồn tại
            var showtime = await _context.Showtimes
                .FirstOrDefaultAsync(s => s.Id == request.ShowtimeId);

            if (showtime == null)
                return NotFound($"Suất chiếu ID {request.ShowtimeId} không tồn tại!");

            // 2. Tìm thông tin phim
            var movie = await _context.Movies.FindAsync(showtime.MovieId);
            string realMovieTitle = movie != null ? movie.Title : "Phim chưa cập nhật";

            // 3. Tìm User
            string currentUsername = "Khách vãng lai";
            if (request.UserId > 0)
            {
                var user = await _context.Users.FindAsync(request.UserId);
                if (user != null) currentUsername = user.Username;
            }

            // A. Lưu Booking (Đơn hàng)
            var newBooking = new Booking
            {
                UserId = request.UserId,
                TotalAmount = request.TotalAmount,
                BookingDate = DateTime.Now,
                ShowtimeId = request.ShowtimeId,
                Status = "Paid",
                Quantity = request.SelectedSeats.Count,
                MovieTitle = realMovieTitle,
                Username = currentUsername
            };

            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();

            // B. Lưu Chi tiết ghế (BookedSeats)
            foreach (var seatName in request.SelectedSeats)
            {
                var parts = seatName.Split('_');
                if (parts.Length < 2) continue;

                string row = parts[0];
                if (!int.TryParse(parts[1], out int number)) continue;

                var seat = await _context.Seats.FirstOrDefaultAsync(s => s.RowName == row && s.Number == number);
                if (seat != null)
                {
                    var bookedSeat = new BookedSeat
                    {
                        BookingId = newBooking.Id,
                        ShowtimeId = request.ShowtimeId,
                        SeatId = seat.Id,
                        Status = "Sold"
                    };
                    _context.BookedSeats.Add(bookedSeat);
                }
            }
            await _context.SaveChangesAsync();

            // C. GỬI MAIL XÁC NHẬN (Đã sửa lỗi trùng tên biến showtime)
            try
            {
                var showtimeForEmail = await _context.Showtimes
                    .Include(s => s.Movie)
                    .FirstOrDefaultAsync(s => s.Id == request.ShowtimeId);

                var userForEmail = await _context.Users.FindAsync(request.UserId);

                if (userForEmail != null && !string.IsNullOrEmpty(userForEmail.Email) && showtimeForEmail != null)
                {
                    Task.Run(() => EmailService.SendBookingConfirmation(
                        userForEmail.Email,
                        userForEmail.Username,
                        showtimeForEmail.Movie.Title,
                        string.Join(", ", request.SelectedSeats),
                        request.TotalAmount,
                        showtimeForEmail.Movie.PosterUrl,
                        showtimeForEmail.StartTime
                    ));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi gửi mail: " + ex.Message);
            }

            // D. GỬI TÍN HIỆU REALTIME
            foreach (var seatName in request.SelectedSeats)
            {
                string seatIdForClient = "btnSeat_" + seatName;
                await _hubContext.Clients.Group(request.ShowtimeId.ToString())
                    .SendAsync("ReceiveSeatStatus", request.ShowtimeId, seatIdForClient, "Sold", request.UserId);
            }

            return Ok(new { message = "Đặt vé thành công" });
        }
    }


    }