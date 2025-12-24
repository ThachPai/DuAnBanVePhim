using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Phim3API.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Genre { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string PosterUrl { get; set; } 
        public DateTime? ReleaseDate { get; set; }
    }
}
