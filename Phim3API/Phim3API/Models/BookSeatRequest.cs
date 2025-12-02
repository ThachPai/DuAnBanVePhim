using System.Collections.Generic;
namespace Phim3API.Models
{
    public class BookSeatRequest
    {
        public int ShowtimeId { get; set; } // Đặt cho suất nào
        public string Username { get; set; } // Ai đặt
        public List<string> SeatNumbers { get; set; } // Danh sách ghế: ["A1", "A2"]
        public decimal PricePerTicket { get; set; } // Giá 1 vé
    }
}
