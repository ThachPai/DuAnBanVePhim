using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phim3API.Data;
using Phim3API.Models;

namespace Phim3API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovieController(AppDbContext context)
        {
            _context = context;
        }

        // 1. API Lấy danh sách tất cả phim (GET: api/movie/get-all)
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _context.Movies.ToListAsync();
            return Ok(movies);
        }

        // 2. API Thêm phim mới (POST: api/movie/add)
        [HttpPost("add")]
        public async Task<IActionResult> AddMovie([FromBody] Movie movie)
        {
            try
            {
                // 1. Lưu phim mới
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync(); // Lúc này movie.Id đã có giá trị tự tăng (VD: 36)

                // 2. Tự động tạo Suất chiếu mặc định cho phim vừa thêm
                var autoShowtime = new Showtime
                {
                    MovieId = movie.Id, // Dùng ID vừa sinh ra ở trên
                    RoomId = 1,         // Mặc định phòng 1
                    StartTime = DateTime.Now.AddDays(1).Date.AddHours(19), // Mặc định 19:00 tối mai
                    Price = movie.Price > 0 ? movie.Price : 75000 // Lấy giá phim hoặc mặc định 75k
                };

                _context.Showtimes.Add(autoShowtime);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Thêm phim và tạo suất chiếu tự động thành công!",
                    movieId = movie.Id,
                    showtimeId = autoShowtime.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi thêm phim: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

    }
}