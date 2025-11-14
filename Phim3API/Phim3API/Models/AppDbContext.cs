using Microsoft.EntityFrameworkCore;
using Phim3API.Models;
namespace Phim3API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Đây chính là bảng Users trong SQL
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
