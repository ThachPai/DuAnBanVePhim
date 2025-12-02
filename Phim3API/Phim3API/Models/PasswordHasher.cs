using System.Security.Cryptography;
using System.Text;

namespace Phim3API.Helpers // Nhớ đổi namespace cho đúng
{
    public static class PasswordHasher
    {
        // Hàm này nhận vào chuỗi "123" và trả về chuỗi loằng ngoằng đã mã hóa
        public static string ComputeHash(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            using (MD5 md5 = MD5.Create())
            {
                // Chuyển chuỗi thành bytes
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Mã hóa
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển lại thành chuỗi Hex (x2) để lưu vào DB
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}