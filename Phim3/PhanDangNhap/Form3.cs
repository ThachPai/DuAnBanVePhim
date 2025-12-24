using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phim3
{
    public partial class Form3 : Form
    {
        public string emailCanKhoiPhuc;

        // Constructor nhận email truyền vào
        public Form3(string email)
        {
            InitializeComponent();
            this.emailCanKhoiPhuc = email;

            // Tự động điền email vào ô textbox (nếu có) để user đỡ phải nhập lại
            if (txtEmail != null) txtEmail.Text = email;
        }
        public Form3()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void btnGuiOTP_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim(); // Trim để xóa khoảng trắng thừa

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập Email!");
                return;
            }

            try
            {
                // Đóng gói dữ liệu gửi đi
                var data = new { Email = email };

                // Dùng ApiClient cho đồng bộ (hoặc giữ HttpClient nếu bạn muốn, nhưng nhớ sửa Port)
                // Lưu ý: Nếu dùng HttpClient thủ công, coi chừng lỗi SSL
                var response = await ApiClient.PostAsync("../auth/forgot-password", data);

                if (response.IsSuccessStatusCode)
                {
                    // Đọc kết quả thành công
                    string responseString = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseString);

                    // Hiện OTP lên để test (Do chưa gửi mail thật)
                    MessageBox.Show(result.message.ToString(), "Thông báo");

                    // Lưu email lại để dùng cho bước sau (nếu cần)
                    this.emailCanKhoiPhuc = email;
                }
                else
                {
                    // --- SỬA QUAN TRỌNG TẠI ĐÂY ---
                    // Đọc lỗi thật sự từ Server thay vì tự đoán
                    string realError = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Thất bại: " + realError);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        private async void btnXacNhan_Click(object sender, EventArgs e)
        {
            // 1. Lấy dữ liệu
            string otp = txtOTP.Text.Trim();
            string newPass = txtNewPass.Text.Trim(); // Hoặc txtMatKhauMoi

            // [QUAN TRỌNG] Kiểm tra Email: Nếu biến toàn cục rỗng thì lấy từ ô nhập
            string emailGuiDi = !string.IsNullOrEmpty(this.emailCanKhoiPhuc)
                                ? this.emailCanKhoiPhuc
                                : txtEmail.Text.Trim();

            // 2. Kiểm tra rỗng
            if (string.IsNullOrEmpty(emailGuiDi))
            {
                MessageBox.Show("Lỗi: Không tìm thấy Email cần khôi phục!");
                return;
            }

            if (string.IsNullOrEmpty(otp) || string.IsNullOrEmpty(newPass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã OTP và Mật khẩu mới!");
                return;
            }

            btnXacNhan.Enabled = false;
            btnXacNhan.Text = "Đang xử lý...";

            try
            {
                // 3. Đóng gói dữ liệu (SỬA LẠI TÊN BIẾN Ở ĐÂY)
                // Dựa vào lỗi của bạn, Backend đang đòi "OTPCode" chứ không phải "Otp"
                var data = new
                {
                    Email = emailGuiDi,
                    OTPCode = otp,         // SỬA: Đổi lại thành OTPCode cho khớp Backend
                    NewPassword = newPass
                };

                // 4. Gọi API
                var response = await ApiClient.PostAsync("../auth/reset-password", data);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Thành công! Mật khẩu đã được đổi. Hãy đăng nhập lại.");
                    this.Close(); // Đóng form
                }
                else
                {
                    // Đọc lỗi chi tiết
                    string errorMsg = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Thất bại: " + errorMsg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
            finally
            {
                btnXacNhan.Enabled = true;
                btnXacNhan.Text = "Xác nhận";
            }
        }

    }
}