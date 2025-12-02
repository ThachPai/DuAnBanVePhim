using Phim3.ADmin; // Nhớ using cái chỗ chứa class Booking/UserDTO
using System;
using System.Collections.Generic;
using System.Collections.Generic; // Để dùng List
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Phim3.MainChinh
{
    public partial class FormLichSu : Form
    {
        private string _username;
        public FormLichSu(string username)
        {
            InitializeComponent();
            _username = username;
        }

        private async void FormLichSu_Load(object sender, EventArgs e)
        {
            await LoadHistory();
        }
        private async System.Threading.Tasks.Task LoadHistory()
        {
            try
            {
                // 1. Kiểm tra xem có username chưa
                if (string.IsNullOrEmpty(_username)) return;

                // 2. Gọi API Ticket mới
                // Lưu ý: Đường dẫn là /api/ticket/history
                using (HttpClient client = ApiClient.GetClient()) // Dùng ApiClient cho chuẩn
                {
                    string url = ApiClient.BaseUrl + "/api/ticket/history?username=" + Uri.EscapeDataString(_username);

                    var response = await client.GetStringAsync(url);

                    // 3. Đổ dữ liệu vào List BookingDTO mới
                    var listVe = JsonConvert.DeserializeObject<List<BookingDTO>>(response);

                    // 4. Hiển thị lên bảng
                    dgvLichSu.DataSource = listVe;

                    // (Optional) Đặt tên cột tiếng Việt cho đẹp
                    if (dgvLichSu.Columns["MovieTitle"] != null) dgvLichSu.Columns["MovieTitle"].HeaderText = "Tên Phim";
                    if (dgvLichSu.Columns["SeatNumber"] != null) dgvLichSu.Columns["SeatNumber"].HeaderText = "Ghế";
                    if (dgvLichSu.Columns["Price"] != null) dgvLichSu.Columns["Price"].HeaderText = "Giá Tiền";
                    // ... Bạn tự đổi tên tiếp nhé
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử: " + ex.Message);
            }
        }
    }
    public class BookingDTO
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; } // Tên phim
        public string RoomName { get; set; }   // Tên rạp (Ví dụ: Rạp 1)
        public DateTime Time { get; set; }     // Giờ chiếu
        public string SeatNumber { get; set; } // Ghế (Ví dụ: A1)
        public decimal Price { get; set; }     // Giá vé
        public DateTime Date { get; set; }     // Ngày mua vé
    }
}
