using Microsoft.AspNetCore.Mvc;
using Phim3API.Data;
using Phim3API.Models;
using System.Linq;
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

        // API lấy tất cả danh sách phim
        // GET: api/movie
        [HttpGet]
        public IActionResult GetAllMovies()
        {
            var movies = _context.Movies.ToList();
            return Ok(movies);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateMovie(int id, [FromBody] Movie movieInfo)
        {
            // 1. Tìm phim cần sửa trong kho
            var movie = _context.Movies.Find(id);

            if (movie == null)
            {
                return NotFound(new { message = "Không tìm thấy phim này!" });
            }

            // 2. Cập nhật thông tin mới
            movie.Title = movieInfo.Title;
            movie.Description = movieInfo.Description;
            movie.Price = movieInfo.Price;
            movie.Duration = movieInfo.Duration;
            movie.PosterUrl = movieInfo.PosterUrl;
            // ...cập nhật thêm các trường khác nếu cần

            // 3. Lưu thay đổi
            _context.SaveChanges();

            return Ok(new { message = "Cập nhật phim thành công!" });
        }
    }
}
