using Microsoft.AspNetCore.Mvc;
using Phim3API.Data;
using Phim3API.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using BCrypt.Net;

namespace Phim3API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // --- 1. ĐĂNG KÝ ---
        // (Đã xóa chữ 'Models.' trước RegisterRequest)
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return BadRequest(new { message = "Thiếu thông tin!" });

            var existingUser = _context.Users.FirstOrDefault(u => u.Username == request.Username);
            if (existingUser != null)
                return BadRequest(new { message = "Tên đăng nhập đã tồn tại!" });

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Username = request.Username,
                Password = passwordHash,
                Email = request.Email,
                Role = "User"
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(new { message = "Đăng ký thành công!" });
        }

        // --- 2. ĐĂNG NHẬP ---
        // (Đã xóa chữ 'Models.' trước LoginRequest)
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Tìm user theo Username (hoặc EmailOrUsername nếu bạn muốn)
            // Ở đây logic Frontend gửi 'EmailOrUsername' vào biến request.EmailOrUsername
            // Nhưng Backend tìm trong cột Username của DB
            var user = _context.Users.FirstOrDefault(u => u.Username == request.EmailOrUsername);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu!" });
            }

            string token = CreateToken(user);

            return Ok(new
            {
                message = "Đăng nhập thành công!",
                token = token
            });
        }

        // --- 3. QUÊN MẬT KHẨU ---
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null) return BadRequest(new { message = "Email không tồn tại!" });

            string otp = new Random().Next(1000, 9999).ToString();
            user.OTPCode = otp;
            _context.SaveChanges();

            return Ok(new { message = "Mã OTP là: " + otp });
        }

        // --- 4. XÁC NHẬN ĐỔI MẬT KHẨU ---
        // (Đã xóa chữ 'Models.' trước ResetPasswordRequest)
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null) return BadRequest(new { message = "Email không tồn tại!" });

            if (user.OTPCode != request.OTPCode)
            {
                return BadRequest(new { message = "Mã OTP không đúng!" });
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.OTPCode = null;
            _context.SaveChanges();

            return Ok(new { message = "Đổi mật khẩu thành công! Hãy đăng nhập lại." });
        }

        // --- 5. CÁC API KHÁC (Giữ nguyên) ---
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.Select(u => new { u.Id, u.Username, u.Email, u.Role }).ToList();
            return Ok(users);
        }

        [HttpDelete("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound(new { message = "Không tìm thấy user!" });
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok(new { message = "Đã xóa tài khoản thành công!" });
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var appSettingsToken = _config.GetSection("AppSettings:Token").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingsToken));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    // ========================================================
    // CÁC CLASS MODEL MỚI (ĐƯỢC DÙNG TRỰC TIẾP TRONG CONTROLLER)
    // ========================================================
    public class LoginRequest
    {
        public string? EmailOrUsername { get; set; } // <-- Đã khớp với Frontend
        public string? Password { get; set; }
    }

    public class RegisterRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
    }

    public class ForgotPasswordRequest
    {
        public string? Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string? Email { get; set; }
        public string? OTPCode { get; set; }
        public string? NewPassword { get; set; }
    }
}