using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Phim3API.Data;
using Phim3API.Models;
using System;
using System.Linq;

namespace Phim3API.Controllers // Nhớ đổi tên namespace theo tên project của bạn
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context; // Biến để kết nối DB

        // Constructor: Nhận kết nối DB vào đây
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // --- 1. ĐĂNG KÝ (THẬT) ---
        [HttpPost("register")]
        public IActionResult Register([FromBody] Models.RegisterRequest request)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return BadRequest(new { message = "Thiếu thông tin!" });

            // Kiểm tra trùng tên trong DB
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == request.Username);
            if (existingUser != null)
                return BadRequest(new { message = "Tên đăng nhập đã tồn tại!" });

            // Tạo user mới
            var newUser = new User
            {
                Username = request.Username,
                Password = request.Password, // Lưu ý: Thực tế phải mã hóa pass (MD5/BCrypt), ở đây làm demo thì lưu trần
                Email = request.Email,
                Role = "User"
            };

            // Lưu vào DB
            _context.Users.Add(newUser);
            _context.SaveChanges(); // Lệnh này mới thực sự ghi vào SQL

            return Ok(new { message = "Đăng ký thành công!" });
        }

        // --- 2. ĐĂNG NHẬP (THẬT) ---
        [HttpPost("login")]
        public IActionResult Login([FromBody] Models.LoginRequest request)
        {
            // Tìm user trong DB khớp cả tên và pass
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu!" });
            }

            // Tìm thấy thì trả về thông tin
            return Ok(new
            {
                message = "Đăng nhập thành công!",
                user = new { user.Username, user.Email, user.Role } // Chỉ trả về những cái cần thiết
            });
        }

        // --- 3. QUÊN MẬT KHẨU (THẬT) ---
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] Microsoft.AspNetCore.Identity.Data.ForgotPasswordRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null) return BadRequest(new { message = "Email không tồn tại!" });

            string otp = new Random().Next(1000, 9999).ToString();

            // Cập nhật OTP vào bảng Users
            user.OTPCode = otp;
            _context.SaveChanges(); // Lưu lại

            return Ok(new { message = "Mã OTP là: " + otp });
        }
        // --- 4. XÁC NHẬN ĐỔI MẬT KHẨU (Mới thêm) ---
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] Models.ResetPasswordRequest request)
        {
            // 1. Tìm user theo email
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
                return BadRequest(new { message = "Email không tồn tại!" });

            // 2. Kiểm tra mã OTP có khớp với trong DB không
            if (user.OTPCode != request.OTPCode)
            {
                return BadRequest(new { message = "Mã OTP không đúng!" });
            }

            // 3. Nếu đúng OTP -> Cập nhật mật khẩu mới
            user.Password = request.NewPassword;

            // Quan trọng: Xóa mã OTP đi để không dùng lại được nữa
            user.OTPCode = null;

            _context.SaveChanges(); // Lưu vào SQL

            return Ok(new { message = "Đổi mật khẩu thành công! Hãy đăng nhập lại." });
        }
        // --- 5. LẤY DANH SÁCH TẤT CẢ USER (Cho Admin) ---
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            // Chỉ lấy những thông tin cần thiết, KHÔNG trả về Mật khẩu (Bảo mật)
            var users = _context.Users
                .Select(u => new {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.Role
                })
                .ToList();

            return Ok(users);
        }

        // --- 6. XÓA USER ---
        [HttpDelete("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound(new { message = "Không tìm thấy user!" });

            // Không cho phép xóa chính mình (Admin không thể tự xóa Admin đang đăng nhập)
            // (Logic này làm nâng cao sau, giờ cứ cho xóa hết để test)

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok(new { message = "Đã xóa tài khoản thành công!" });
        }
        
    }
}