using Microsoft.AspNetCore.Mvc;
using Phim3API.Data;
using System.Linq;
namespace Phim3API.Models
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

        // Lấy danh sách suất chiếu của một phim
        // GET: api/showtime?movieId=1
        [HttpGet]
        public IActionResult GetShowtimes(int movieId)
        {
            var list = _context.Showtimes
                        .Where(s => s.MovieId == movieId)
                        .OrderBy(s => s.StartTime) // Sắp xếp theo giờ chiếu
                        .ToList();
            return Ok(list);
        }
    }
}
