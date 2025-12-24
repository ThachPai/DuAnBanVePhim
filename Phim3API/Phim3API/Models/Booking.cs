using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Thêm cái này nếu muốn dùng ForeignKey

namespace Phim3API.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }


        public string Username { get; set; } = string.Empty;

        public string MovieTitle { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public int UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public int Quantity { get; set; }

        public DateTime BookingDate { get; set; }

        public int? ShowtimeId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }  // Chú ý: Dùng class 'User', thêm dấu ? cho phép null

        // Nếu có Showtime thì thêm luôn
        [ForeignKey("ShowtimeId")]
        public virtual Showtime? Showtime { get; set; }

        // public virtual AppUser User { get; set; } 
    }
}