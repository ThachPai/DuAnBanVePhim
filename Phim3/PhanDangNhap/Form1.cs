using System;
using System.Net.Http; // (Cần cho HttpClient)
using System.Text;
using System.Windows.Forms;
using Phim3.MainChinh;
using System.Text.Json; // <-- Dùng thư viện JSON mới
// (Không cần Newtonsoft.Json nữa)

namespace Phim3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // === HÀM ĐĂNG NHẬP (ĐÃ NÂNG CẤP HOÀN CHỈNH) ===
        private async void btnDangNhap_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu!");
                return;
            }

            // 1. Đóng gói dữ liệu (Dùng class từ AuthModels.cs)
            var requestData = new LoginRequest
            {
                EmailOrUsername = username,
                Password = password
            };

            try
            {
                // 2. Gọi API (Dùng file helper ApiClient.cs)
                HttpResponseMessage response = await ApiClient.PostAsync("login", requestData);
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đăng nhập thành công!");

                    // 3. "Mở" gói hàng (JSON) nhận về
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    // 4. Cất "vé thông hành" (Token) vào "ví" toàn cục
                    GlobalToken.Token = loginResponse?.token;

                    // 5. Mở Form Trang Chủ (KHÔNG TRUYỀN GÌ CẢ)
                    GiaoDienNguoiDung trangChu = new GiaoDienNguoiDung();
                    trangChu.Show();

                    this.Hide();
                }
                else
                {
                    // Hiển thị lỗi từ API (ví dụ: "Sai tài khoản hoặc mật khẩu!")
                    MessageBox.Show(responseString, "Lỗi Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Lỗi này xảy ra nếu API Backend bị tắt
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi Mạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // === CÁC HÀM CŨ GIỮ NGUYÊN ===
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
    }
}