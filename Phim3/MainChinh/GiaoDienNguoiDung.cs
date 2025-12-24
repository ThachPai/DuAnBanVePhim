using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Net.Http;
using System.Text.Json;

namespace Phim3.MainChinh
{
    public partial class GiaoDienNguoiDung : Form
    {
        private string _username;
        private string _role;
        private string _userId;
        private List<Movie> _danhSachPhimHienTai = new List<Movie>();

        public GiaoDienNguoiDung()
        {
            InitializeComponent();
            CheckTokenAndLogin(); // Tách logic check login ra cho gọn
        }

        // 1. Logic kiểm tra đăng nhập
        private void CheckTokenAndLogin()
        {
            if (string.IsNullOrEmpty(GlobalToken.Token))
            {
                MessageBox.Show("Phiên đăng nhập hết hạn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Load += (s, e) => {
                    new Form1().Show();
                    this.Close();
                };
                return;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(GlobalToken.Token) as JwtSecurityToken;

                var claimId = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                _userId = claimId != null ? claimId.Value : "0";

                var claimName = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "unique_name");
                _username = claimName != null ? claimName.Value : "Unknown";

                var claimRole = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role");
                _role = claimRole != null ? claimRole.Value : "User";

                // Ẩn nút Admin nếu không phải Admin
                if (_role != "Admin") btnAdminMode.Visible = false;
            }
            catch
            {
                MessageBox.Show("Token lỗi. Vui lòng đăng nhập lại.");
                return;
            }
        }

        // 2. Sự kiện LOAD form (Kích hoạt khi form hiện lên)
        private async void GiaoDienNguoiDung_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_username)) return;
            
            // 👇 GỌI ĐÚNG HÀM TẢI PHIM 👇
            await LoadMoviesToUI(); 
        }

        // 3. Hàm tải phim và VẼ lên màn hình
        private async System.Threading.Tasks.Task LoadMoviesToUI()
        {
            try
            {
                // Thay số cổng 7071 bằng cổng Swagger của bạn (ví dụ 7123)
                string apiUrl = "https://localhost:7500/api/movie/get-all";

                var handler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true };
                using (HttpClient client = new HttpClient(handler))
                {
                    if (!string.IsNullOrEmpty(GlobalToken.Token))
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalToken.Token);
                    }

                    var response = await client.GetStringAsync(apiUrl);
                    
                    var listMovies = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Movie>>(response);
                    _danhSachPhimHienTai = listMovies;

                    // --- VẼ GIAO DIỆN ---
                    // Đảm bảo tên panel của bạn là flowLayoutPanel1 (như trong code cũ của bạn)
                    if (flowLayoutPanel1 != null)
                    {
                        flowLayoutPanel1.Controls.Clear(); // Xóa cũ

                        foreach (var m in listMovies)
                        {
                            // 👇 Sửa dòng này: Truyền thêm m.Duration và m.ReleaseDate
                            var item = new Phim3.UC_MovieItem(
                                m.Id,
                                m.Title,
                                m.Price,
                                m.PosterUrl,
                                m.Duration,    // <-- Thêm
                                m.ReleaseDate  ??DateTime.Now // <-- Thêm
                            );

                            flowLayoutPanel1.Controls.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải phim: " + ex.Message);
            }
        }

        // --- CÁC NÚT KHÁC ---
        private void button3_Click(object sender, EventArgs e) // Lịch sử
        {
            FormLichSu historyForm = new FormLichSu(_username);
            historyForm.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e) // Đăng xuất
        {
            if (MessageBox.Show("Đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                GlobalToken.Token = null;
                new Form1().Show();
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e) // Admin
        {
            AdminPhim adminForm = new AdminPhim();
            adminForm.Show();
            this.Hide();
        }

        // CÁC HÀM THỪA (Button cũ, click ảnh cũ...) -> CỨ ĐỂ TRỐNG HOẶC XÓA ĐI
        // Vì bây giờ ta bấm trực tiếp vào UserControl (UC_MovieItem) rồi
        private void button2_Click(object sender, EventArgs e) { }
        private void button4_Click(object sender, EventArgs e) { }
        private void button5_Click(object sender, EventArgs e) { }
        private void button6_Click(object sender, EventArgs e) { }
        private void picPhim1_Click(object sender, EventArgs e) { }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
    }

    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string PosterUrl { get; set; }
        public int Duration { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}