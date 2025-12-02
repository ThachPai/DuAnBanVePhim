using Microsoft.AspNetCore.Mvc;
using Phim3API.Data;
using Phim3API.Models;
using System;
using System.Linq;
namespace Phim3API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TicketController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Lấy danh sách các ghế ĐÃ BỊ ĐẶT của một suất chiếu
        // GET: api/ticket/booked-seats/5 (5 là ShowtimeId)
        [HttpGet("booked-seats/{showtimeId}")]
        public IActionResult GetBookedSeats(int showtimeId)
        {
            // Chỉ cần lấy ra cột SeatNumber thôi (ví dụ: ["A3", "A4", "B5"])
            var bookedSeats = _context.Tickets
                                .Where(t => t.ShowtimeId == showtimeId)
                                .Select(t => t.SeatNumber)
                                .ToList();
            return Ok(bookedSeats);
        }

        // 2. Đặt vé (Mua nhiều ghế cùng lúc)
        // POST: api/ticket/book
        [HttpPost("book")]
        public IActionResult BookTickets([FromBody] BookSeatRequest request)
        {
            // Kiểm tra xem có ghế nào trong danh sách khách chọn đã bị người khác nhanh tay đặt trước không
            var existingSeats = _context.Tickets
                                .Where(t => t.ShowtimeId == request.ShowtimeId && request.SeatNumbers.Contains(t.SeatNumber))
                                .Select(t => t.SeatNumber)
                                .ToList();

            if (existingSeats.Count > 0)
            {
                string gheBiTrung = string.Join(", ", existingSeats);
                return BadRequest(new { message = $"Xin lỗi, các ghế sau vừa bị người khác đặt mất rồi: {gheBiTrung}" });
            }

            // Nếu ngon lành cành đào -> Lưu tất cả ghế vào DB
            foreach (var seat in request.SeatNumbers)
            {
                var newTicket = new Ticket
                {
                    ShowtimeId = request.ShowtimeId,
                    Username = request.Username,
                    SeatNumber = seat,
                    PriceAtBooking = request.PricePerTicket,
                    BookingDate = DateTime.Now
                };
                _context.Tickets.Add(newTicket);
            }

            _context.SaveChanges();
            return Ok(new { message = "Đặt vé thành công! Chúc bạn xem phim vui vẻ." });
        }
        // 3. Xem lịch sử đặt vé của User (Kỹ thuật JOIN 3 bảng)
        // GET: api/ticket/history?username=admin
        [HttpGet("history")]
        public IActionResult GetUserHistory(string username)
        {
            // Logic: Từ bảng Ticket -> Join sang Showtimes -> Join sang Movies để lấy tên phim
            var history = from t in _context.Tickets
                          join s in _context.Showtimes on t.ShowtimeId equals s.Id
                          join m in _context.Movies on s.MovieId equals m.Id
                          where t.Username == username
                          orderby t.BookingDate descending // Mới nhất lên đầu
                          select new
                          {
                              Id = t.Id,
                              MovieTitle = m.Title,   // Lấy tên phim từ bảng Movies
                              RoomName = s.RoomName,  // Lấy tên rạp
                              Time = s.StartTime,     // Lấy giờ chiếu
                              SeatNumber = t.SeatNumber,
                              Price = t.PriceAtBooking,
                              Date = t.BookingDate
                          };

            return Ok(history.ToList());
        }
    }
}
