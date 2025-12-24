namespace Phim3API.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string? RowName { get; set; } // Lưu ý: SQL là char(1), C# dùng string vẫn ổn
        public int Number { get; set; }
        public string? Type { get; set; }
    }
}