
using System.Collections.Generic;

namespace Phim3API.Models
{
    public class BookingRequest
    {
        public int ShowtimeId { get; set; } // Phải khớp với tên trong WinForms
        public List<string> SelectedSeats { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}