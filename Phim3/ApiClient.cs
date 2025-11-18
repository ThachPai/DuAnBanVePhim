using System.Net.Http;
using System.Text;
using System.Text.Json; // Dùng thư viện JSON của Microsoft
using System.Threading.Tasks;

// Đảm bảo namespace này khớp với project Frontend của bạn (ví dụ: Phim3)
namespace Phim3
{
    public static class ApiClient
    {
        // CẢNH BÁO: BẠN BẮT BUỘC PHẢI THAY SỐ CỔNG (PORT) 7071
        // BẰNG SỐ CỔNG TRONG launchSettings.json CỦA BACKEND CỦA BẠN
        private static readonly string BASE_URL = "https://localhost:7071/api/auth";

        private static HttpClient _client;

        static ApiClient()
        {
            // Code này để bỏ qua lỗi chứng chỉ SSL (lỗi tự ký của localhost)
            // Rất quan trọng khi demo
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            _client = new HttpClient(handler);
        }

        // Hàm chung để gọi POST, giúp bạn không phải lặp lại code
        public static async Task<HttpResponseMessage> PostAsync(string endpoint, object data)
        {
            // Đóng gói dữ liệu (data) thành JSON
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json"
            );

            // Gửi request (ví dụ: https://localhost:7071/api/auth/register)
            return await _client.PostAsync($"{BASE_URL}/{endpoint}", jsonContent);
        }
    }
}