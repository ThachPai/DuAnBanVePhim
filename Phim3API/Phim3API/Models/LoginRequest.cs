namespace Phim3API.Models
{
    public class LoginRequest
    {
        // Sửa 'Username' thành 'EmailOrUsername' để khớp với AuthController
        public string? EmailOrUsername { get; set; }
        public string? Password { get; set; }
    }
}