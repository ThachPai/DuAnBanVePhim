using System;
using System.ComponentModel.DataAnnotations;
namespace Phim3API.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public int ShowtimeId { get; set; }
        public string Username { get; set; }
        public string SeatNumber { get; set; } // Ví dụ: "A1", "B5"
        public decimal PriceAtBooking { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
