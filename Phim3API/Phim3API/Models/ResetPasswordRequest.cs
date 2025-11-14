namespace Phim3API.Models
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string OTPCode { get; set; }
        public string NewPassword { get; set; }
    }
}
