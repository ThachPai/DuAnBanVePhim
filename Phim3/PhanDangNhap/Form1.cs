using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Phim3.MainChinh;
namespace Phim3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form = new Form2();
            form.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        private async void btnDangNhap_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu!");
                return;
            }

            // 1. Đóng gói dữ liệu
            var loginData = new { Username = username, Password = password };
            string jsonContent = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // ⚠️ Đổi số PORT 
                    string apiUrl = "https://localhost:7071/api/auth/login";

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Đọc kết quả trả về (Để xem là Admin hay User thường)
                        string responseString = await response.Content.ReadAsStringAsync();
                        dynamic result = JsonConvert.DeserializeObject(responseString);

                        string role = result.user.role; // Lấy quyền (Admin/User)
                        string userTen = result.user.username;
                        
                        MessageBox.Show($"Xin chào {userTen}! Đăng nhập thành công.");

                        // --- CHUYỂN MÀN HÌNH ---
                        // Mở Form Trang Chủ
                        GiaoDienNguoiDung trangChu = new GiaoDienNguoiDung(userTen,role);
                        trangChu.Show();

                        // Ẩn Form Đăng Nhập đi
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Đăng nhập thất bại! Kiểm tra lại tài khoản/mật khẩu.");
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
