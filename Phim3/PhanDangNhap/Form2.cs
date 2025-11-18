using System;
using System.Windows.Forms;
// (Không cần using HttpClient hay Json, vì ApiClient đã lo)

// (Đảm bảo namespace của bạn là 'Phim3' hoặc 'quanlyve2')
namespace Phim3
{
    public partial class Form2 : Form // (Form2 là form Đăng ký)
    {
        public Form2()
        {
            InitializeComponent();
        }

        // === HÀM ĐĂNG KÝ (ĐÃ NÂNG CẤP) ===
        private async void button1_Click(object sender, EventArgs e) // (Giả sử button1 là nút Đăng ký)
        {
            // 1. Kiểm tra mật khẩu (Nếu bạn có ô "Confirm Password")
            // if (txtPassword.Text != txtConfirmPassword.Text)
            // {
            //     MessageBox.Show("Mật khẩu xác nhận không khớp!");
            //     return; 
            // }

            // 2. Kiểm tra rỗng (Giữ nguyên code cũ của bạn)
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
                return;
            }

            // 3. Chuẩn bị "gói hàng" (dùng class từ AuthModels.cs)
            var requestData = new RegisterRequest
            {
                FullName = "Default User", // (Code cũ của bạn không có ô FullName)
                Email = txtEmail.Text,
                Username = txtUsername.Text,
                Password = txtPassword.Text
            };

            try
            {
                // 4. Gọi API (dùng "dây điện" từ ApiClient.cs)
                var response = await ApiClient.PostAsync("register", requestData);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đăng ký tài khoản thành công! Vui lòng quay lại đăng nhập.");
                    this.Close(); // Đóng form để quay lại Đăng nhập
                }
                else
                {
                    // Hiển thị lỗi trả về từ API (ví dụ: "Email đã tồn tại")
                    MessageBox.Show(responseString, "Lỗi Đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Lỗi này xảy ra khi API backend của bạn bị tắt
                MessageBox.Show($"Lỗi kết nối đến server: {ex.Message}", "Lỗi Mạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}