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
        public Form3()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void btnGuiOTP_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập Email!");
                return;
            }

            var data = new { Email = email };
            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Gọi API gửi OTP
                    string apiUrl = "https://localhost:7071/api/auth/forgot-password"; // Nhớ sửa PORT
                    var response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Mẹo: Vì mình chưa gửi email thật, nên API trả OTP về đây luôn để test
                        string responseString = await response.Content.ReadAsStringAsync();
                        dynamic result = JsonConvert.DeserializeObject(responseString);

                        // Hiển thị OTP lên màn hình cho người dùng copy (Chỉ dùng khi test thôi nhé)
                        MessageBox.Show(result.message.ToString(), "Thông báo OTP (Test)");
                    }
                    else
                    {
                        MessageBox.Show("Email không tồn tại trong hệ thống!");
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private async void btnXacNhan_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string otp = txtOTP.Text; // Ô nhập mã OTP
            string newPass = txtNewPass.Text; // Ô nhập mật khẩu mới

            if (string.IsNullOrEmpty(otp) || string.IsNullOrEmpty(newPass))
            {
                MessageBox.Show("Vui lòng nhập OTP và Mật khẩu mới!");
                return;
            }

            // Đóng gói 3 món này gửi đi
            var data = new { Email = email, OTPCode = otp, NewPassword = newPass };
            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Gọi API xác nhận đổi pass
                    string apiUrl = "https://localhost:7071/api/auth/reset-password"; // Nhớ sửa PORT
                    var response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Đổi mật khẩu thành công! Mời bạn đăng nhập lại.");
                        this.Close(); // Đóng form này lại
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Thất bại: " + error);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }
    }
}
