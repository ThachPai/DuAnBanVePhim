namespace Phim3API.Models
{
    public class BookedSeat
    {
        public int Id { get; set; }
        public string Status { get; set; } // "Sold", "Holding"

        // Khóa ngoại
        public int BookingId { get; set; }
        public int? ShowtimeId { get; set; }
        public int? SeatId { get; set; }

        // 👇👇👇 THÊM DÒNG NÀY ĐỂ SỬA LỖI ĐỎ "does not contain definition for Booking" 👇👇👇
        [System.Text.Json.Serialization.JsonIgnore] // Thêm dòng này để tránh lỗi vòng lặp khi API trả về JSON
        public virtual Booking Booking { get; set; }



    }
}