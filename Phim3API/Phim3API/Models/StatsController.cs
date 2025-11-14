using Microsoft.AspNetCore.Mvc;
using Phim3API.Data;
using System.Linq; // Thư viện để tính toán Sum/Count
namespace Phim3API.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StatsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("dashboard")]
        public IActionResult GetDashboardStats()
        {
            // 1. Tính tổng doanh thu (Cộng cột TotalPrice trong bảng Bookings)
            // Dùng hàm Sum(). Nếu chưa có đơn nào thì trả về 0
            var doanhThu = _context.Bookings.Sum(b => (decimal?)b.TotalPrice) ?? 0;

            // 2. Tính tổng vé bán ra (Cộng cột Quantity)
            var soVe = _context.Bookings.Sum(b => (int?)b.Quantity) ?? 0;

            // 3. Đếm số phim đang có (Đếm dòng trong bảng Movies)
            var soPhim = _context.Movies.Count();

            // 4. Đếm số khách hàng (Trừ admin ra cho chuẩn)
            var soKhach = _context.Users.Count(u => u.Role == "User");

            // Đóng gói trả về
            return Ok(new
            {
                Revenue = doanhThu,
                Tickets = soVe,
                Movies = soPhim,
                Customers = soKhach
            });
        }
    }
}
