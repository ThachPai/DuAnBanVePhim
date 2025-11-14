using System.ComponentModel.DataAnnotations;
namespace Phim3API.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        // Tên người đặt (lưu username)
        public string Username { get; set; }

        // Tên phim đặt
        public string MovieTitle { get; set; }

        // Số lượng vé
        public int Quantity { get; set; }

        // Tổng tiền
        public decimal TotalPrice { get; set; }

        // Ngày giờ đặt vé
        public DateTime BookingDate { get; set; }
    }
}
