using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
// Thêm 3 dòng "using" này để giải mã Token
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
// Thêm 2 dòng "using" này để gọi API
using System.Net.Http;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Phim3.MainChinh
{
    public partial class GiaoDienNguoiDung : Form
    {
        // Biến để lưu thông tin người đang dùng (Lấy từ Token)
        private string _username;
        private string _role;
        private string _userId;

        // Biến để lưu danh sách phim
        private List<Movie> _danhSachPhimHienTai = new List<Movie>();

        // === HÀM KHỞI TẠO (ĐÃ NÂNG CẤP) ===
        public GiaoDienNguoiDung() // <-- Không còn tham số (username, role)
        {
            InitializeComponent();
            
            // 1. Kiểm tra "vé" (Token) trong "ví"
            if (string.IsNullOrEmpty(GlobalToken.Token))
            {
                MessageBox.Show("Bạn chưa đăng nhập hoặc phiên đã hết hạn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Đóng Form này và quay lại Đăng nhập
                this.Load += (s, e) => { 
                    Form1 loginForm = new Form1(); 
                    loginForm.Show(); 
                    this.Close(); 
                };
                return;
            }

            // 2. Nếu có Token -> "Giải mã" Token để lấy thông tin
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(GlobalToken.Token) as JwtSecurityToken;

                // Lấy thông tin từ Token (phải khớp với lúc bạn tạo Token ở Backend)
                // --- CODE MỚI AN TOÀN HƠN ---
                // Dùng FirstOrDefault: Nếu không có thì trả về null, không gây lỗi sập app

                // 1. Lấy ID (Nếu không có thì mặc định là "0")
                var claimId = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                _userId = claimId != null ? claimId.Value : "0";

                // 2. Lấy Tên (Nếu không có thì mặc định là "Unknown")
                // Lưu ý: Đôi khi JWT lưu tên dưới dạng "unique_name" thay vì ClaimTypes.Name
                var claimName = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "unique_name");
                _username = claimName != null ? claimName.Value : "Unknown";

                // 3. Lấy Role (Nếu không có thì mặc định là "User")
                var claimRole = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role");
                _role = claimRole != null ? claimRole.Value : "User";
                // -----------------------------
            }
            catch (Exception ex)
            {
                MessageBox.Show("Token không hợp lệ, vui lòng đăng nhập lại: " + ex.Message);
                // Xử lý lỗi (quay về đăng nhập)
                return;
            }

            // 3. LOGIC QUAN TRỌNG: Ẩn nút Admin nếu không phải Admin
            if (_role != "Admin")
            {
                btnAdminMode.Visible = false;
            }

            // Chào mừng user
            // (Giả sử bạn có một Label tên là lblWelcome)
            // lblWelcome.Text = "Xin chào, " + _username;
        }

        // === HÀM TẢI PHIM (ĐÃ NÂNG CẤP: Dùng ApiClient) ===
        private async void GiaoDienNguoiDung_Load(object sender, EventArgs e)
        {
            // Nếu chưa đăng nhập (do lỗi ở trên), thì không tải phim
            if (string.IsNullOrEmpty(_username)) return;

            await LoadMovies();
        }

        private async System.Threading.Tasks.Task LoadMovies()
        {
            try
            {
                // 1. Tạo "dây điện" (HttpClient) riêng cho API Phim
                // (Vì ApiClient của chúng ta chỉ trỏ đến "/api/auth")
                
                // ⚠️ THAY SỐ CỔNG 7071
                string apiUrl = "https://localhost:7071/api/movie"; 
                
                var handler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true };
                using (HttpClient client = new HttpClient(handler))
                {
                    // 2. Thêm Token (vé thông hành) vào Header
                    // (Một số API lấy phim có thể không cần, nhưng đây là cách làm đúng)
                    client.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalToken.Token);

                    var response = await client.GetStringAsync(apiUrl);

                    // 3. Convert JSON (Dùng System.Text.Json)
                    List<Movie> movies = JsonSerializer.Deserialize<List<Movie>>(response, new JsonSerializerOptions 
                    { 
                        PropertyNameCaseInsensitive = true 
                    });
                    _danhSachPhimHienTai = movies;

                    // 4. Hiển thị lên giao diện (Giữ nguyên logic cũ)
                    if (movies.Count >= 1) { /* ... Hiển thị Phim 1 ... */ }
                    if (movies.Count >= 2) { /* ... Hiển thị Phim 2 ... */ }
                    // ... (Tương tự cho Phim 3, 4)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách phim: " + ex.Message);
            }
        }

        // === CÁC HÀM SỰ KIỆN KHÁC (ĐÃ NÂNG CẤP) ===

        // Nút Lịch sử
        private void button3_Click(object sender, EventArgs e)
        {
            // Truyền _username (đã giải mã từ Token) sang
            FormLichSu historyForm = new FormLichSu(_username);
            historyForm.ShowDialog();
        }

        // Nút Đăng xuất
        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                // 1. "XÉ VÉ": Đây là bước quan trọng nhất
                GlobalToken.Token = null;
                
                // 2. Mở lại Form Đăng nhập
                Form1 loginForm = new Form1();
                loginForm.Show();

                // 3. Đóng Form hiện tại
                this.Close();
            }
        }

        // Nút Admin
        private void button1_Click(object sender, EventArgs e)
        {
            // Logic kiểm tra Role đã được làm trong hàm Khởi tạo
            // (Nếu nút này hiển thị, nghĩa là bạn là Admin)
            AdminPhim adminForm = new AdminPhim();
            adminForm.Show();
            this.Hide();
        }

        // === CÁC NÚT ĐẶT VÉ (ĐÃ NÂNG CẤP) ===
        // (Chúng ta truyền _username từ Token, thay vì "admin" fix cứng)

        private void button2_Click(object sender, EventArgs e) // Phim 1
        {
            if (_danhSachPhimHienTai.Count >= 1)
            {
                var phim = _danhSachPhimHienTai[0];
                Nut1 formDatVe = new Nut1(phim.Title, phim.Price, _username);
                formDatVe.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e) // Phim 2
        {
            if (_danhSachPhimHienTai.Count >= 2)
            {
                var phim = _danhSachPhimHienTai[1];
                Nut2 formDatVe = new Nut2(phim.Title, phim.Price, _username);
                formDatVe.ShowDialog();
            }
        }

        private void button5_Click(object sender, EventArgs e) // Phim 3
        {
            if (_danhSachPhimHienTai.Count >= 3)
            {
                var phim = _danhSachPhimHienTai[2];
                Nut3 formDatVe = new Nut3(phim.Title, phim.Price, _username);
                formDatVe.ShowDialog();
            }
        }

        private void button6_Click(object sender, EventArgs e) // Phim 4
        {
            if (_danhSachPhimHienTai.Count >= 4)
            {
                var phim = _danhSachPhimHienTai[3];
                Nut4 formDatVe = new Nut4(phim.Title, phim.Price, _username);
                formDatVe.ShowDialog();
            }
        }
        
        // (Các hàm rỗng khác giữ nguyên)
    }

    // Bạn cần class Movie này (có thể tạo file riêng)
    public class Movie
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string PosterUrl { get; set; }
        // (Thêm các thuộc tính khác nếu API trả về)
    }
}