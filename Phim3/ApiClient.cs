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
        // Biến này sẽ được cập nhật động từ file text
        public static string BaseUrl = "https://localhost:7071";

        // Hàm khởi tạo tĩnh: Tự chạy ngay khi App bật lên
        static ApiClient()
        {
            try
            {
                // Tìm file ip.txt nằm ngay cạnh file .exe
                string path = Path.Combine(Application.StartupPath, "ip.txt");

                if (File.Exists(path))
                {
                    // Đọc nội dung file (Ví dụ: http://192.168.1.10:7071)
                    string ipFromFile = File.ReadAllText(path).Trim();
                    if (!string.IsNullOrEmpty(ipFromFile))
                    {
                        BaseUrl = ipFromFile;
                    }
                }
            }
            catch { /* Nếu lỗi đọc file thì cứ dùng mặc định localhost */ }
        }

        public static HttpClient GetClient()
        {
            return new HttpClient();
        }
    }
}
