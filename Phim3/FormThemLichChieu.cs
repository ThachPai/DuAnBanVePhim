using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phim3
{
    public partial class FormThemLichChieu : Form
    {
        public class MovieDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }
        private async void FormThemLichChieu_Load(object sender, EventArgs e)
        {
            await LoadDanhSachPhim();
        }

        private async Task LoadDanhSachPhim()
        {
            try
            {
                // Thay cổng 7071 bằng cổng API của bạn
                string url = "https://localhost:7500/api/movie/get-all";

                // Bỏ qua SSL (dành cho localhost)
                var handler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (s, c, ch, ssl) => true };

                using (HttpClient client = new HttpClient(handler))
                {
                    var response = await client.GetStringAsync(url);
                    var listMovies = JsonSerializer.Deserialize<List<MovieDto>>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // Đổ dữ liệu vào ComboBox
                    cbPhim.DataSource = listMovies;
                    cbPhim.DisplayMember = "Title"; // Cái hiện lên cho người dùng thấy (Tên phim)
                    cbPhim.ValueMember = "Id";      // Cái giá trị ẩn bên dưới (ID Phim)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách phim: " + ex.Message);
            }
        }
        public FormThemLichChieu()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (cbPhim.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn phim!");
                    return;
                }

                // Tạo gói dữ liệu để gửi đi
                var requestData = new
                {
                    MovieId = (int)cbPhim.SelectedValue, // Lấy ID phim từ ValueMember
                    RoomId = int.Parse(textBox2.Text),   // Lấy ID phòng
                    StartTime = dtpThoiGian.Value,       // Lấy ngày giờ từ DateTimePicker
                    Price = decimal.Parse(txtGiaVe.Text) // Lấy giá vé
                };

                // Gọi API Tạo Lịch Chiếu (API mà bạn vừa viết ở bước trước)
                string url = "https://localhost:7500/api/showtime/create";

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(requestData),
                    Encoding.UTF8,
                    "application/json");

                var handler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (s, c, ch, ssl) => true };
                using (HttpClient client = new HttpClient(handler))
                {
                    var response = await client.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Thêm lịch chiếu thành công!");
                        this.Close(); // Đóng form sau khi xong
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Lỗi từ Server: " + error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}