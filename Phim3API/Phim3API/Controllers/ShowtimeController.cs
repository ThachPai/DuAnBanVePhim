using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phim3API.Data;
using Phim3API.Models;

namespace Phim3API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowtimeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ShowtimeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateShowtime([FromBody] CreateShowtimeDto request)
        {
            try
            {
                // Sử dụng $ trực tiếp trong chuỗi để EF không tự ý thêm các lệnh cài đặt Identity
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO Showtimes (MovieId, RoomId, StartTime, Price) 
            VALUES ({request.MovieId}, {request.RoomId}, {request.StartTime}, {request.Price})");

                return Ok(new { message = "Thành công dứt điểm!" });
            }
            catch (Exception ex)
            {
                // Nếu vẫn lỗi, in ra câu lệnh SQL thực tế mà nó đang cố chạy
                return StatusCode(500, new { message = ex.Message, detail = ex.InnerException?.Message });
            }
        }
        [HttpGet("get-by-movie/{movieId}")]
        public async Task<IActionResult> GetByMovie(int movieId)
        {
            // Lấy tất cả suất chiếu của phim này
            var showtimes = await _context.Showtimes
                .Where(s => s.MovieId == movieId)
                .ToListAsync();

            if (showtimes == null || !showtimes.Any())
                return NotFound("Phim này chưa có lịch chiếu!");

            return Ok(showtimes);
        }

        public class CreateShowtimeDto
        {
            public int MovieId { get; set; }
            public int RoomId { get; set; }
            public DateTime StartTime { get; set; }
            public decimal Price { get; set; }
        }
    }
}