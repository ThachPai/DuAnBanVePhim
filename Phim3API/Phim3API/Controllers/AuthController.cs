using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Phim3API.Data;
using Phim3API.Models; // <--- QUAN TRỌNG: Dòng này giúp tìm thấy LoginRequest trong thư mục Models
using Phim3API.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

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
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Tìm user (Sửa lại để dùng đúng thuộc tính trong Model của bạn)
            // Nếu Model của bạn chỉ có Username thì dùng request.Username
            // Nếu Model của bạn có EmailOrUsername thì dùng request.EmailOrUsername
            var user = _context.Users.FirstOrDefault(u => u.Username == request.EmailOrUsername);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu!" });
            }

            string token = CreateToken(user);

            return Ok(new
            {
                message = "Đăng nhập thành công!",
                userId = user.Id,         // Gửi ID về cho Client lưu
                username = user.Username,
                token = token
            });
        }

        // --- 3. QUÊN MẬT KHẨU ---
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            // 1. Kiểm tra email có tồn tại không
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("Email không tồn tại trong hệ thống.");
            }

            // 2. Tạo mã OTP ngẫu nhiên (4 số)
            Random random = new Random();
            string otp = random.Next(1000, 9999).ToString();

            // 3. Lưu OTP vào DB
            user.ResetToken = otp;
            user.ResetTokenExpires = DateTime.Now.AddMinutes(5); // Hết hạn sau 5 phút
            _context.SaveChanges();

            // 4. Gửi Email thật
            try
            {
                string subject = "Mã xác nhận Quên mật khẩu - Rạp Phim";
                string body = $@"
            <h3>Xin chào {user.Username},</h3>
            <p>Bạn đã yêu cầu đặt lại mật khẩu.</p>
            <p>Mã OTP của bạn là: <b style='color:red; font-size:20px;'>{otp}</b></p>
            <p>Mã này có hiệu lực trong 5 phút.</p>";

                EmailService.Send(user.Email, subject, body);

                return Ok(new { message = "Mã OTP đã được gửi đến Email của bạn." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi gửi email: " + ex.Message);
            }
        }

        // --- 4. XÁC NHẬN ĐỔI MẬT KHẨU ---
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            // Tìm user
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Email không tồn tại!" });
            }



            if (user.ResetToken != request.OTPCode.Trim())
            {
                return BadRequest(new { message = "Mã OTP không đúng!" });
            }

            // THÊM MỚI: Kiểm tra hạn sử dụng (ResetTokenExpires)
            // Nếu thời gian hiện tại (Now) đã vượt quá thời gian hết hạn -> Báo lỗi
            if (user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest(new { message = "Mã OTP đã hết hạn. Vui lòng lấy mã mới." });
            }

            // Đổi mật khẩu
  
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);


            user.ResetToken = null;
            user.ResetTokenExpires = null;

            _context.SaveChanges();

            return Ok(new { message = "Đổi mật khẩu thành công! Hãy đăng nhập lại." });
        }

        // --- 5. CÁC API KHÁC ---
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
                new Claim(ClaimTypes.Name, user.Username ?? "Unknown"), // Xử lý null
                new Claim(ClaimTypes.Role, user.Role ?? "User") // Xử lý null
            };

            // Thêm dấu ! để bỏ qua cảnh báo null
            var appSettingsToken = _config.GetSection("AppSettings:Token").Value!;
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
}