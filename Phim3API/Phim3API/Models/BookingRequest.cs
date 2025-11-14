namespace Phim3API.Models
{
    public class BookingRequest
    {
        public string Username { get; set; }
        public string MovieTitle { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
