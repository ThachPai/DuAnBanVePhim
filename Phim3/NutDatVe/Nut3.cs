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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Phim3
{
    public partial class Nut3 : Form
    {
        // Biến để lưu thông tin phim được truyền sang
        private string _tenPhim;
        private decimal _giaVe;
        private string _username; // Người đang đăng nhập
        public Nut3(string tenPhim, decimal giaVe, string username)
        {
            InitializeComponent();
            // Lưu lại vào biến
            _tenPhim = tenPhim;
            _giaVe = giaVe;
            _username = username;
            // 👇 THÊM DÒNG NÀY: Tự động điền ngày hôm nay
            txtNgayChieu.Text = DateTime.Now.ToString("dd/MM/yyyy");

            // Khóa ô này lại không cho sửa (chỉ để xem)
            txtNgayChieu.ReadOnly = true;

            // 👇 GỌI HÀM TẢI GHẾ NGAY KHI MỞ FORM
            LoadGheConLai();
        }
        private async void LoadGheConLai()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Gọi API (Lưu ý: movieTitle phải được mã hóa nếu có dấu cách)
                    string apiUrl = "https://localhost:7071/api/booking/check-seats?movieTitle=" + Uri.EscapeDataString(_tenPhim);

                    var response = await client.GetStringAsync(apiUrl);

                    // Đọc kết quả JSON (ví dụ: { "remaining": 48 })
                    dynamic result = JsonConvert.DeserializeObject(response);
                    int conLai = result.remaining;

                    // Hiển thị lên Label
                    lblGheConLai.Text = $"Còn {conLai} ghế";

                    // Logic phụ: Nếu hết ghế (<= 0) thì khóa nút Xác nhận lại
                    if (conLai <= 0)
                    {
                        lblGheConLai.Text = "HẾT VÉ";
                        lblGheConLai.ForeColor = System.Drawing.Color.Red;
                        btnXacNhan.Enabled = false; // Khóa nút
                    }
                }
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Nut3_Load(object sender, EventArgs e)
        {

        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtSoLuong.Text, out int soLuong)) // txtSoLuong là tên ô nhập
            {
                decimal tongTien = soLuong * _giaVe;
                lblTongTien.Text = tongTien.ToString("N0") + " VNĐ"; // lblTongTien là nhãn hiển thị tiền
            }
        }

        private async void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!");
                return;
            }

            // 2. Lấy số ghế còn lại từ cái Label mình vừa hiển thị
            // (Mẹo: Cắt chuỗi "Còn 48 ghế" để lấy số 48, hoặc lưu biến toàn cục)
            // Cách đơn giản: Gọi lại API hoặc lưu biến. Ở đây mình làm cách lưu biến cho dễ nhé.
            // ... Thôi làm cách đơn giản nhất là check text đi:

            string textGhe = lblGheConLai.Text.Replace("Còn ", "").Replace(" ghế", "");
            int gheHienCo = 0;
            int.TryParse(textGhe, out gheHienCo);

            // 3. So sánh
            if (soLuong > gheHienCo)
            {
                MessageBox.Show($"Xin lỗi, chỉ còn lại {gheHienCo} ghế thôi!");
                return; // Dừng lại, không cho mua
            }

            decimal tongTien = soLuong * _giaVe;

            // Đóng gói dữ liệu
            var bookingData = new
            {
                Username = _username,
                MovieTitle = _tenPhim,
                Quantity = soLuong,
                TotalPrice = tongTien
            };

            // Gọi API (Copy y chang mấy bài trước)
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string json = JsonConvert.SerializeObject(bookingData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // SỬA PORT API
                    var response = await client.PostAsync("https://localhost:7071/api/booking", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Đặt vé thành công!");
                        this.Close(); // Đóng form đặt vé
                    }
                    else
                    {
                        MessageBox.Show("Lỗi đặt vé!");
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }
    }
}
