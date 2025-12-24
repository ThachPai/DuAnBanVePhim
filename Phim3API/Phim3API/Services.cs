using System.Net;
using System.Net.Mail;

namespace Phim3API.Services
{
    public class EmailService
    {
        private static string _fromEmail = "phucnguyenhoang7749@gmail.com";
        private static string _appPassword = "fddf qvde tqvr cqix"; 

        public static void Send(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_fromEmail, _appPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, "Rạp Phim Admin"), // Tên hiển thị
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true, 
                };

                mailMessage.To.Add(toEmail);

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi gửi mail: " + ex.Message);
            }
        }

        public static void SendBookingConfirmation(string toEmail, string username, string movieTitle, string seats, decimal totalAmount, string posterUrl, DateTime startTime)
        {
            // Tạo nội dung HTML đẹp mắt với Poster
            string body = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; border: 1px solid #eee; padding: 20px;'>
            <h2 style='color: #e74c3c; text-align: center;'>XÁC NHẬN ĐẶT VÉ THÀNH CÔNG</h2>
            <p>Chào <b>{username}</b>,</p>
            <p>Bạn đã đặt vé thành công cho bộ phim:</p>
            <div style='display: flex; gap: 20px; margin-top: 20px;'>
                <img src='{posterUrl}' alt='Poster' style='width: 150px; border-radius: 10px; margin-right: 20px;' />
                <div>
                    <p style='font-size: 18px; margin: 0;'><b>{movieTitle}</b></p>
                    <p>💺 Ghế: <b style='color: #2980b9;'>{seats}</b></p>
                    <p>⏰ Giờ chiếu: <b>{startTime:HH:mm dd/MM/yyyy}</b></p>
                    <p>💰 Tổng tiền: <b style='color: #27ae60;'>{totalAmount:N0} VNĐ</b></p>
                </div>
            </div>
            <hr style='border: 0; border-top: 1px solid #eee; margin: 20px 0;' />
            <p style='font-size: 12px; color: #7f8c8d; text-align: center;'>Vui lòng xuất trình email này tại quầy để nhận vé. Chúc bạn xem phim vui vẻ!</p>
        </div>";

            // Gọi lại hàm Send static có sẵn của bạn
            Send(toEmail, $"[PhimMoi] Xác nhận đặt vé: {movieTitle}", body);
        }
    }
}