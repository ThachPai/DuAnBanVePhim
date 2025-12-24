using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Phim3API.Models;

namespace Phim3API.Models
{

    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string? OTPCode { get; set; }
        public string? ResetToken { get; set; } // Dấu ? cho phép nó null (khi ko cần reset)
        public DateTime? ResetTokenExpires { get; set; } // Thời gian hết hạn
    }
    
}
