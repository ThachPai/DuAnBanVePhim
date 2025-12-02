using System;
using System.ComponentModel.DataAnnotations;
namespace Phim3API.Models
{
    public class Showtime
    {
        [Key]
        public int Id { get; set; }
        public int MovieId { get; set; } // Phim nào
        public string RoomName { get; set; } // Rạp mấy
        public DateTime StartTime { get; set; } // Chiếu lúc mấy giờ
    }
}
