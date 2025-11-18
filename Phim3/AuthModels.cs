// Đảm bảo namespace này khớp với project Frontend của bạn (ví dụ: Phim3)
namespace Phim3
{
    // Dùng cho API Đăng ký
    public class RegisterRequest
    {
        public string? FullName
        {get;  set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    // Dùng cho API Đăng nhập
    public class LoginRequest
    {           
        public string? EmailOrUsername { get; set; }
        public string? Password { get; set; }
    }

    // Dùng cho API Quên Mật khẩu (Bước 1: Gửi email)
    public class ForgotPasswordRequest
    {
        public string? Email { get; set; }
    }

    // Dùng cho API Quên Mật khẩu (Bước 2: Đặt lại)
    public class ResetPasswordRequest
    {
        public string? Token { get; set; } // Dùng Token (do Backend in ra Console)
        public string? NewPassword
        { get; set; }
        public string? Email
        { get; set;} // Thêm Email (vì API Reset của bạn cần Email)
        public string? OTPCode
        {
            get; set; } // Thêm OTP (vì API Reset của bạn cần OTP)
        }
    }