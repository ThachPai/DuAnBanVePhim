using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace Phim3
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // 1. Lấy dữ liệu từ các ô nhập
            string username = txtUsername.Text; // Đổi tên txtUsername theo giao diện của bạn
            string password = txtPassword.Text; // Đổi tên txtPassword theo giao diện của bạn
            string email = txtEmail.Text;       // Đổi tên txtEmail theo giao diện của bạn

            // Kiểm tra rỗng
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
                return;
            }

            // 2. Đóng gói dữ liệu thành kiện hàng (Object nặc danh)
            // Lưu ý: Tên Username, Password, Email phải giống hệt bên API của Phúc
            var registerData = new
            {
                Username = username,
                Password = password,
                Email = email
            };

            // Biến thành chuỗi JSON
            string jsonContent = JsonConvert.SerializeObject(registerData);

            // Đóng gói để gửi đi (Encoding UTF8 để không lỗi tiếng Việt)
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // 3. Gửi đi (Gọi điện cho Phúc)
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // ⚠️ QUAN TRỌNG: Thay số 7123 bằng cổng API trên máy bạn
                    string apiUrl = "https://localhost:7071/api/auth/register";

                    // Gửi lệnh POST
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // 4. Nhận phản hồi
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Đăng ký thành công! Giờ bạn có thể đăng nhập.");
                        this.Close(); // Đóng form đăng ký
                    }
                    else
                    {
                        // Đọc lỗi từ API gửi về (ví dụ: "Tài khoản đã tồn tại")
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Đăng ký thất bại: " + errorResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }
    }
    
}
