using Microsoft.AspNetCore.Mvc;
using Phim3API.Data;
using Phim3API.Models;
namespace Phim3API.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult BookTicket([FromBody] BookingRequest request)
        {
            // Tạo bản ghi mới
            var newBooking = new Booking // Class này ánh xạ với bảng SQL của Thịnh
            {
                Username = request.Username,
                MovieTitle = request.MovieTitle,
                Quantity = request.Quantity,
                TotalPrice = request.TotalPrice,
                BookingDate = DateTime.Now
            };

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            return Ok(new { message = "Đặt vé thành công!" });
        }
        [HttpGet("check-seats")]
        public IActionResult GetRemainingSeats(string movieTitle)
        {
            // 1. Giả sử mỗi rạp mặc định có 50 ghế
            int tongGhe = 50;

            // 2. Tính tổng số vé đã bán của phim này trong Database
            // (Cộng dồn cột Quantity trong bảng Bookings theo tên phim)
            var veDaBan = _context.Bookings
                            .Where(b => b.MovieTitle == movieTitle)
                            .Sum(b => (int?)b.Quantity) ?? 0;

            // 3. Tính số ghế còn lại
            int conLai = tongGhe - veDaBan;

            // Trả về kết quả
            return Ok(new { Remaining = conLai });
        }
        // Lấy lịch sử vé của một người dùng cụ thể
        // GET: api/booking/history?username=testuser
        [HttpGet("history")]
        public IActionResult GetUserHistory(string username)
        {
            var history = _context.Bookings
                            .Where(b => b.Username == username) // Lọc theo tên
                            .OrderByDescending(b => b.BookingDate) // Mới nhất lên đầu
                            .ToList();

            return Ok(history);
        }
    }
}
