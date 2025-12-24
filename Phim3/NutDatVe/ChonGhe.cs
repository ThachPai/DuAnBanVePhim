using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;

namespace Phim3.NutDatVe
{
    public partial class ChonGhe : Form
    {
        private HubConnection? _hubConnection;
        private int _showtimeId;
        private int _soLuongVeCanMua;
        private int _userId;
        private decimal _totalAmount;

        public ChonGhe(int showtimeId, int soLuongVe, int userId, decimal totalAmount)
        {
            InitializeComponent();

            _showtimeId = showtimeId;
            _soLuongVeCanMua = soLuongVe;
            this._userId = userId;
            _totalAmount = totalAmount;

            this.Text = $"Suất chiếu: {_showtimeId} - Chọn {_soLuongVeCanMua} ghế";

            InitializeSignalR();
            GanSuKienChoGhe();
            LoadGheDaBan(); // Gọi hàm tải dữ liệu ghế từ DB
        }

        public ChonGhe() : this(1, 1, 0, 0) { }

        private async void InitializeSignalR()
        {
            string hubUrl = "https://localhost:7500/seatHub";

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl, options =>
                {
                    options.HttpMessageHandlerFactory = (message) =>
                    {
                        if (message is HttpClientHandler clientHandler)
                            clientHandler.ServerCertificateCustomValidationCallback +=
                                (sender, certificate, chain, sslPolicyErrors) => { return true; };
                        return message;
                    };
                })
                .WithAutomaticReconnect()
                .Build();

            // Nhận tín hiệu từ Hub (Server đã gửi kèm showtimeId)
            _hubConnection.On<int, string, string, int>("ReceiveSeatStatus", (showId, seatName, status, senderId) =>
            {
                if (showId != _showtimeId) return;

                this.Invoke(new Action(() =>
                {
                    Control[] found = this.Controls.Find(seatName, true);
                    if (found.Length > 0 && found[0] is Button btn)
                    {
                        if (status == "Holding")
                        {
                            // Nếu người gửi tín hiệu trùng với ID máy mình -> Màu Vàng
                            // Nếu là người khác -> Màu Cam (Orange)
                            btn.BackColor = (senderId == _userId) ? Color.Yellow : Color.Orange;
                        }
                        else if (status == "Free")
                        {
                            btn.BackColor = Color.White;
                        }
                        else if (status == "Sold")
                        {
                            btn.BackColor = Color.Red;
                            btn.Enabled = false;
                        }
                    }
                }));
            });

            try
            {
                await _hubConnection.StartAsync();
                // Vào đúng nhóm của suất chiếu này để nhận thông tin riêng
                await _hubConnection.InvokeAsync("JoinShowtimeGroup", _showtimeId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối Realtime: {ex.Message}");
            }
        }

        private void GanSuKienChoGhe()
        {
            GanSuKienDeQuy(this);
        }

        private void GanSuKienDeQuy(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button btn && btn.Name.StartsWith("btnSeat_"))
                {
                    btn.Click -= Ghe_Click;
                    btn.Click += Ghe_Click;
                }
                else if (c.HasChildren)
                {
                    GanSuKienDeQuy(c);
                }
            }
        }

        private async void Ghe_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn) return;

            // Nếu ghế đỏ (đã bán) hoặc ghế cam (người khác đang giữ) thì không cho bấm
            if (btn.BackColor == Color.Red || btn.BackColor == Color.Orange) return;

            // CHỈ ĐẾM GHẾ MÀU VÀNG (Của mình)
            List<Button> allButtons = GetAllButtons(this);
            int minhDangChon = allButtons.Count(b => b.Name.StartsWith("btnSeat_") && b.BackColor == Color.Yellow);

            string statusGuiDi = "";

            if (btn.BackColor == Color.Yellow)
            {
                // Mình hủy chọn ghế của mình
                btn.BackColor = Color.White;
                statusGuiDi = "Free";
            }
            else
            {
                // Kiểm tra giới hạn vé của RIÊNG MÌNH
                if (minhDangChon >= _soLuongVeCanMua)
                {
                    MessageBox.Show($"Bạn chỉ được mua {_soLuongVeCanMua} vé!", "Đủ số lượng");
                    return;
                }

                // Mình chọn ghế
                btn.BackColor = Color.Yellow;
                statusGuiDi = "Holding";
            }

            // Gửi tín hiệu SignalR
            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("SendSeatStatus",
                                  _showtimeId,
                                  btn.Name,
                                  statusGuiDi,
                                  _userId);
            }
        }

        private List<Button> GetAllButtons(Control parent)
        {
            List<Button> result = new List<Button>();
            foreach (Control c in parent.Controls)
            {
                if (c is Button btn) result.Add(btn);
                if (c.HasChildren) result.AddRange(GetAllButtons(c));
            }
            return result;
        }

        private void DoiMauGhe(string btnName, string status)
        {
            if (InvokeRequired) { this.Invoke(new Action(() => DoiMauGhe(btnName, status))); return; }

            Control[] found = this.Controls.Find(btnName, true);
            if (found.Length > 0 && found[0] is Button btn)
            {
                if (status == "Holding")
                {
                    // Nếu là người khác đang chọn (Realtime) -> Tô màu Cam (khác với màu Vàng của mình)
                    // Lưu ý: Chỉ đổi sang Cam nếu nó đang là màu Trắng (chưa ai chọn)
                    if (btn.BackColor == Color.White)
                        btn.BackColor = Color.Orange;
                }
                else if (status == "Sold")
                {
                    btn.BackColor = Color.Red;
                    btn.Enabled = false;
                }
                else if (status == "Free")
                {
                    // Nếu người kia bỏ giữ ghế -> Trả về trắng (trừ khi mình đang chọn ghế đó)
                    if (btn.BackColor != Color.Yellow)
                        btn.BackColor = Color.White;
                }

                btn.Refresh();
            }
        }

        // --- HÀM TẢI DỮ LIỆU TỪ DATABASE (KHÔNG XÓA) ---
        private async void LoadGheDaBan()
        {
            try
            {
                string getUrl = $"https://localhost:7500/api/booking/showtime/{_showtimeId}/seats";
                var handler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true };

                using (HttpClient client = new HttpClient(handler))
                {
                    var result = await client.GetStringAsync(getUrl);
                    var danhSachGhe = System.Text.Json.JsonSerializer.Deserialize<List<GheDaBanDto>>(result,
                        new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (danhSachGhe != null)
                    {
                        foreach (var ghe in danhSachGhe)
                        {
                            string btnName = "btnSeat_" + ghe.SeatName;
                            Control[] found = this.Controls.Find(btnName, true);
                            if (found.Length > 0 && found[0] is Button btn)
                            {
                                if (ghe.Status == "Sold")
                                {
                                    btn.BackColor = Color.Red;
                                    btn.Enabled = false;
                                }
                                else if (ghe.Status == "Holding")
                                {
                                    btn.BackColor = Color.Yellow;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi tải sơ đồ ghế: " + ex.Message);
            }
        }

        private async void btnXacNhan_Click(object sender, EventArgs e)
        {
            List<string> gheDaChon = new List<string>();
            List<Button> allButtons = GetAllButtons(this);

            foreach (var btn in allButtons)
            {
                if (btn.Name.StartsWith("btnSeat_") && btn.BackColor == Color.Yellow)
                {
                    gheDaChon.Add(btn.Name.Replace("btnSeat_", ""));
                }
            }

            if (gheDaChon.Count != _soLuongVeCanMua)
            {
                MessageBox.Show($"Vui lòng chọn đủ {_soLuongVeCanMua} ghế!");
                return;
            }

            try
            {
                var bookingRequest = new
                {
                    ShowtimeId = _showtimeId,
                    SelectedSeats = gheDaChon,
                    UserId = this._userId,
                    TotalAmount = _totalAmount
                };

                // Giả sử ApiClient của bạn đã được cấu hình đúng URL mới
                var response = await PostToApi("api/booking/book", bookingRequest);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đặt vé thành công!");
                    this.Close();
                }
                else
                {
                    string errorMsg = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi: {errorMsg}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        // Hàm hỗ trợ gọi API
        private async Task<HttpResponseMessage> PostToApi(string endpoint, object data)
        {
            var handler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true };
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://localhost:7500/");
                var json = System.Text.Json.JsonSerializer.Serialize(data);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                return await client.PostAsync(endpoint, content);
            }
        }
    }

    public class GheDaBanDto
    {
        public string SeatName { get; set; }
        public string Status { get; set; }
    }
}