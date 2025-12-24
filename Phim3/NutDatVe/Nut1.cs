using Newtonsoft.Json;
using Phim3.NutDatVe;
using System;
using System.Net.Http; 
using System.Windows.Forms;

namespace Phim3
{
    public partial class Nut1 : Form
    {
        // 1. Biến toàn cục để lưu dữ liệu phim hiện tại
        private int _movieId;       
        private string _tenPhim;   
        private decimal _giaVe;    

        // 2. Constructor: Phải nhận ĐỦ 3 tham số khớp với UC_MovieItem
        public Nut1(int idPhim, string tenPhim, decimal giaVe, string posterUrl, int duration, DateTime ngayChieu)
        {
            InitializeComponent();
            MessageBox.Show("Link nhận được: " + posterUrl);

            // Hứng dữ liệu từ bên ngoài truyền vào
            this._movieId = idPhim;
            this._tenPhim = tenPhim;
            this._giaVe = giaVe;
            lblThoiLuong.Text = duration + " phút";
            lblTenPhim.Text = tenPhim;
            lblGiaVe.Text = giaVe.ToString("N0") + " VNĐ";

            // Hiển thị ngày giờ hiện tại
            txtNgayChieu.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtNgayChieu.ReadOnly = true;
            try
            {
                if (!string.IsNullOrEmpty(posterUrl))
                {
                    pictureBox4.Load(posterUrl); // picPoster là tên cái khung ảnh góc phải
                }
            }
            catch { } 

                    // Hiển thị tên phim lên Form (nếu bạn có label tiêu đề)
                    this.Text = "Đặt vé: " + _tenPhim;

            // Gọi API kiểm tra ghế ngay lập tức
            LoadGheConLai();
        }

        // Hàm gọi API lấy số ghế còn lại từ Server
        private async void LoadGheConLai()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // URL API: Đảm bảo PORT 7071 hoặc 7123 là đúng máy bạn
                    // Gửi kèm Tên Phim để Server đếm ghế
                    string apiUrl = "https://localhost:7500/api/booking/check-seats?movieTitle=" + Uri.EscapeDataString(_tenPhim);

                    var response = await client.GetStringAsync(apiUrl);

                    // Convert JSON về Object
                    dynamic result = JsonConvert.DeserializeObject(response);

                    // Giả sử API trả về: { "remaining": 50 }
                    int conLai = result.remaining;

                    // Hiển thị
                    lblGheConLai.Text = $"Còn {conLai} ghế trống";

                    // Logic: Hết ghế thì khóa nút
                    if (conLai <= 0)
                    {
                        lblGheConLai.Text = "HẾT VÉ";
                        lblGheConLai.ForeColor = System.Drawing.Color.Red;
                        btnXacNhan.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Nếu lỗi mạng thì tạm thời hiện "Đang cập nhật" chứ đừng crash
                lblGheConLai.Text = "Đang cập nhật...";
                // MessageBox.Show("Lỗi tải ghế: " + ex.Message); // Bật lên nếu muốn debug
            }
        }

        // Khi người dùng nhập số lượng -> Tự động tính tiền theo giá DB
        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtSoLuong.Text, out int soLuong))
            {
                // KHÔNG DÙNG 75000 NỮA -> Dùng _giaVe thật
                decimal tongTien = soLuong * _giaVe;

                lblGiaVe.Text = tongTien.ToString("N0") + " VNĐ";
            }
            else
            {
                lblGiaVe.Text = "0 VNĐ";
            }
        }

        // Nút xác nhận chuyển sang chọn ghế
        private async void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0) return;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Gọi API lấy suất chiếu theo MovieId
                    string url = $"https://localhost:7500/api/booking/get-by-movie/{_movieId}";
                    var response = await client.GetStringAsync(url);
                    var showtimes = JsonConvert.DeserializeObject<List<dynamic>>(response);

                    if (showtimes != null && showtimes.Count > 0)
                    {
                        int realShowtimeId = showtimes[0].id; // Lấy ID suất chiếu thực (ID = 1, 2, 3...)

                        int userId = SessionData.CurrentUserId ?? 1;
                        decimal tongTien = soLuong * _giaVe;

                        this.Hide();
                        ChonGhe formGhe = new ChonGhe(realShowtimeId, soLuong, userId, tongTien);
                        formGhe.ShowDialog();
                        this.Show();
                    }
                    else
                    {
                        MessageBox.Show("Phim này chưa có suất chiếu. Hãy thử thêm lại phim mới để hệ thống tự tạo suất chiếu!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        // Các sự kiện thừa (do lỡ click nhầm), cứ để trống cũng được
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void lblTongTien_Click(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}