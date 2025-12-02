using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http;

namespace Phim3.MainChinh
{
    public partial class FormChonGhe : Form
    {
        private int _showtimeId; // ID suất chiếu (Ví dụ: 1)
        private string _username;
        private decimal _giaVe;

        // Danh sách ghế đang chọn (Ví dụ: ["A1", "A2"])
        private List<string> _gheDangChon = new List<string>();

        // Constructor nhận thông tin từ bên ngoài
        public FormChonGhe(int showtimeId, string tenPhim, decimal giaVe, string username)
        {
            InitializeComponent();
            _showtimeId = showtimeId;
            _username = username;
            _giaVe = giaVe;
            this.Text = $"Đặt vé: {tenPhim} - Giá: {giaVe:N0}đ/vé";
        }

        private async void FormChonGhe_Load(object sender, EventArgs e)
        {
            await LoadSoDoGhe();
        }
        // --- HÀM 1: VẼ SƠ ĐỒ GHẾ ---
        private async System.Threading.Tasks.Task LoadSoDoGhe()
        {
            try
            {
                // 1. Lấy danh sách ghế ĐÃ BÁN từ API
                List<string> gheDaBan = new List<string>();

                HttpClient client = ApiClient.GetClient(); // Dùng ApiClient của bài trước
                
                    // Gọi API: /api/ticket/booked-seats/1
                    string url = ApiClient.BaseUrl + "/api/ticket/booked-seats/" + _showtimeId;
                    var response = await client.GetStringAsync(url);
                    gheDaBan = JsonConvert.DeserializeObject<List<string>>(response);
                

                // 2. Vẽ ghế (Ví dụ: 5 hàng A,B,C,D,E - mỗi hàng 8 ghế)
                flpGhe.Controls.Clear(); // Xóa sạch cũ
                string[] hangGhe = { "A", "B", "C", "D", "E" };

                foreach (string hang in hangGhe)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        string tenGhe = hang + i; // Ví dụ: A1, A2...

                        // Tạo nút bấm đại diện cho ghế
                        Button btnGhe = new Button();
                        btnGhe.Text = tenGhe;
                        btnGhe.Width = 60;
                        btnGhe.Height = 60;
                        btnGhe.Margin = new Padding(5); // Khoảng cách giữa các ghế

                        // Kiểm tra: Nếu ghế này có trong danh sách ĐÃ BÁN -> Tô đỏ, khóa lại
                        if (gheDaBan.Contains(tenGhe))
                        {
                            btnGhe.BackColor = Color.Red;
                            btnGhe.Enabled = false; // Không cho bấm
                        }
                        else
                        {
                            btnGhe.BackColor = Color.White; // Ghế trống
                            // Gắn sự kiện click chọn ghế
                            btnGhe.Click += BtnGhe_Click;
                        }

                        // Thêm nút vào bảng
                        flpGhe.Controls.Add(btnGhe);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải ghế: " + ex.Message); }
        }
        // --- HÀM 2: XỬ LÝ KHI BẤM CHỌN GHẾ ---
        private void BtnGhe_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string tenGhe = btn.Text;

            if (btn.BackColor == Color.White) // Đang trống -> Chọn
            {
                btn.BackColor = Color.Blue; // Đổi màu xanh
                btn.ForeColor = Color.White;
                _gheDangChon.Add(tenGhe); // Thêm vào danh sách chọn
            }
            else if (btn.BackColor == Color.Blue) // Đang chọn -> Bỏ chọn
            {
                btn.BackColor = Color.White; // Trả về màu trắng
                btn.ForeColor = Color.Black;
                _gheDangChon.Remove(tenGhe); // Xóa khỏi danh sách
            }

            // Cập nhật tổng tiền
            CapNhatTongTien();
        }
        private void CapNhatTongTien()
        {
            decimal tongTien = _gheDangChon.Count * _giaVe;
            lblTongTien.Text = $"{tongTien:N0} VNĐ ({_gheDangChon.Count} ghế)";
        }

        private async void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (_gheDangChon.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 ghế!");
                return;
            }

            // Đóng gói dữ liệu gửi đi (Đúng theo BookSeatRequest bên API)
            var requestData = new
            {
                ShowtimeId = _showtimeId,
                Username = _username,
                SeatNumbers = _gheDangChon, // Danh sách ["A1", "B2"]
                PricePerTicket = _giaVe
            };

            try
            {
                HttpClient client = ApiClient.GetClient();
                
                    string json = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    string url = ApiClient.BaseUrl + "/api/ticket/book";
                    var response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Đặt vé thành công!");
                        this.Close();
                    }
                    else
                    {
                        string err = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Lỗi: " + err);
                        // Mẹo: Nếu lỗi do ghế bị người khác đặt trước, nên tải lại sơ đồ ghế
                        await LoadSoDoGhe();
                    }
                
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }

        }
    }
}
