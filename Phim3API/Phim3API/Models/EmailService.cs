using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration; // Để đọc appsettings
using MimeKit;
using MimeKit.Text;

namespace Phim3API.Services // Đổi namespace nếu cần
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Tạo nội dung mail
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:Email"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // Kết nối SMTP
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["EmailSettings:SmtpHost"],
                                    int.Parse(_config["EmailSettings:SmtpPort"]),
                                    SecureSocketOptions.StartTls);

            // Đăng nhập bằng Mật khẩu Ứng dụng
            await smtp.AuthenticateAsync(_config["EmailSettings:Email"],
                                         _config["EmailSettings:Password"]);

            // Gửi mail
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}