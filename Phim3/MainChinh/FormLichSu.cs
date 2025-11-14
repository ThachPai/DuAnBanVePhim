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
                // 👇 THÊM ĐOẠN KIỂM TRA NÀY
                if (string.IsNullOrEmpty(_username))
                {
                    MessageBox.Show("Lỗi to: Tên người dùng bị rỗng rồi!");
                    return;
                }
                // 👇 THÊM DÒNG NÀY ĐỂ KIỂM TRA XEM NÓ ĐANG TÌM CỦA AI
                MessageBox.Show("Đang tìm vé của user: [" + _username + "]");
                using (HttpClient client = new HttpClient())
                {
                    // Gọi API lấy lịch sử
                    string apiUrl = "https://localhost:7071/api/booking/history?username=" + _username;

                    var response = await client.GetStringAsync(apiUrl);

                    // Bạn cần tạo class BookingDTO ở client tương ứng để hứng dữ liệu nhé
                    // Hoặc dùng tạm dynamic nếu lười tạo class
                    var listVe = JsonConvert.DeserializeObject<List<BookingDTO>>(response);

                    dgvLichSu.DataSource = listVe;
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải lịch sử: " + ex.Message); }
        }
    }
    public class BookingDTO
    {
        // Tên các cột này phải GIỐNG Y HỆT tên trong API (BookingController) trả về
        public string MovieTitle { get; set; }  // Tên phim
        public int Quantity { get; set; }       // Số lượng
        public decimal TotalPrice { get; set; } // Tổng tiền
        public DateTime BookingDate { get; set; } // Ngày đặt
    }
}
