using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Phim3
{
    public static class ApiClient
    {
        // 👇 TẤT CẢ ĐỊA CHỈ API SẼ ĐỌC TỪ ĐÂY 👇
        // (Sửa cổng của bạn vào đây nếu nó khác 7071)
        public static string BaseUrl = "https://localhost:7071";

        // Tạo một cái HttpClient DÙNG CHUNG cho toàn bộ app
        // (Cách này giúp app chạy nhanh và ổn định hơn)
        private static readonly HttpClient _httpClient = new HttpClient();

        public static HttpClient GetClient()
        {
            return _httpClient;
        }
    }
}
