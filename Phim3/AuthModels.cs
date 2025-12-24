namespace Phim3
{
    // Dùng cho API Đăng ký
    public class RegisterRequest
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    // Dùng cho API Đăng nhập (Gửi đi)
    public class LoginRequest
    {
        public string? EmailOrUsername { get; set; }
        public string? Password { get; set; }
    }

    public class LoginResponse
    {
        public string? token { get; set; }     // Token xác thực
        public int userId { get; set; }        // ID người dùng (để lưu vào SessionData)
        public string? username { get; set; }  // Tên người dùng
    }


    // Dùng cho API Quên Mật khẩu (Bước 1: Gửi email)
    public class ForgotPasswordRequest
    {
        public string? Email { get; set; }
    }

    // Dùng cho API Quên Mật khẩu (Bước 2: Đặt lại)
    public class ResetPasswordRequest
    {
        public string? Token { get; set; }
        public string? NewPassword { get; set; }
        public string? Email { get; set; }
        public string? OTPCode { get; set; }
    }
}