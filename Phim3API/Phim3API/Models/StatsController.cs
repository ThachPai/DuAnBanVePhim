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
            // 1. Tính tổng doanh thu
            // (Trong bảng Tickets, cột tiền tên là PriceAtBooking)
            var doanhThu = _context.Tickets.Sum(t => (decimal?)t.PriceAtBooking) ?? 0;

            // 2. Tính tổng vé bán ra
            // (Mỗi dòng trong Tickets là 1 vé, nên chỉ cần đếm số dòng)
            var soVe = _context.Tickets.Count();

            // 3. Đếm số phim (Giữ nguyên)
            var soPhim = _context.Movies.Count();

            // 4. Đếm số khách hàng (Giữ nguyên)
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
        // API lấy doanh thu theo từng phim (Dùng cho Biểu đồ)
        // GET: api/stats/revenue-by-movie
        [HttpGet("revenue-by-movie")]
        public IActionResult GetRevenueByMovie()
        {
            // Logic: Join 3 bảng (Tickets -> Showtimes -> Movies) để tính tiền theo tên phim
            var data = from t in _context.Tickets
                       join s in _context.Showtimes on t.ShowtimeId equals s.Id
                       join m in _context.Movies on s.MovieId equals m.Id
                       group t by m.Title into g // Gom nhóm theo Tên phim
                       select new
                       {
                           MovieName = g.Key,
                           TotalRevenue = g.Sum(x => x.PriceAtBooking) // Cộng tổng tiền
                       };

            return Ok(data.ToList());
        }
    }
}
