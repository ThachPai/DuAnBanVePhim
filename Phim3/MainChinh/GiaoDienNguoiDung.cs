using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 // Để dùng Image
using System.Net.Http;
using Newtonsoft.Json;

namespace Phim3.MainChinh
{
    public partial class GiaoDienNguoiDung : Form
    {
        // Biến để lưu thông tin người đang dùng
        private string _username;
        private string _role; // Vai trò (Admin hay User)

        // 👇 DÒNG NÀY: Biến để lưu danh sách phim dùng chung cho cả Form
        private List<Movie> _danhSachPhimHienTai = new List<Movie>();
        public GiaoDienNguoiDung(string username, string role)
        {
            InitializeComponent();
           
            this._username = username;
            this._role = role;
            // Kiểm tra lại lần nữa
           
            // 👇 THÊM DÒNG NÀY ĐỂ KIỂM TRA
            //MessageBox.Show("Role nhận được là: " + _role);
            // LOGIC QUAN TRỌNG: Ẩn nút Admin nếu không phải Admin
            if (_role != "Admin")
            {
                btnAdminMode.Visible = false; // Ẩn nút đi
                                              // Hoặc btnAdminMode.Enabled = false; // Làm mờ nút đi
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           /* MessageBox.Show("Username hiện tại là: '" + _username + "'");
            if (string.IsNullOrEmpty(_username))
            {
                MessageBox.Show("Bạn đang chạy chế độ ẩn danh, không xem lịch sử được!");
                return;
            }*/
            // Mở Form Lịch sử và truyền tên người dùng hiện tại sang
            FormLichSu historyForm = new FormLichSu(_username);
            historyForm.ShowDialog(); // Hiện lên trên cùng
        }


        private void button2_Click(object sender, EventArgs e)
        {

            // Kiểm tra xem đã có dữ liệu chưa
            if (_danhSachPhimHienTai.Count > 0)
            {
                // Lấy thông tin phim số 1 (Index là 0) từ kho chứa
                var phimSo1 = _danhSachPhimHienTai[0];

                string tenPhim = phimSo1.Title;
                decimal giaVe = phimSo1.Price; // 👈 LẤY GIÁ TỪ BIẾN NÀY MỚI CHUẨN (Nó sẽ là giá mới update)
                string currentUser = "admin";

                // Truyền sang Form Đặt vé
                Nut1 formDatVe = new Nut1(tenPhim, giaVe, currentUser);
                formDatVe.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã có dữ liệu chưa
            if (_danhSachPhimHienTai.Count > 0)
            {
                // Lấy thông tin phim số 1 (Index là 0) từ kho chứa
                var phimSo2 = _danhSachPhimHienTai[0];

                string tenPhim = phimSo2.Title;
                decimal giaVe = phimSo2.Price; // 👈 LẤY GIÁ TỪ BIẾN NÀY MỚI CHUẨN (Nó sẽ là giá mới update)
                string currentUser = "admin";

                // Truyền sang Form Đặt vé
                Nut2 formDatVe = new Nut2(tenPhim, giaVe, currentUser);
                formDatVe.ShowDialog();
            }
        }
        

        private void button5_Click(object sender, EventArgs e)
        {
            if (lblTenPhim3.Text != "")
            {
                string tenPhim = lblTenPhim3.Text;
                decimal giaVe = 105000; // Tạm thời fix cứng hoặc lấy từ biến toàn cục nếu bạn lưu
                string currentUser = "admin"; // Lấy từ biến lưu lúc đăng nhập (bài trước)

                // Mở Form Nut1 và truyền dữ liệu sang
                Nut3 formDatVe = new Nut3(tenPhim, giaVe, currentUser);
                formDatVe.ShowDialog(); // ShowDialog để bắt buộc xử lý xong mới quay lại
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem phim 1 có dữ liệu chưa
            if (lblTenPhim4.Text != "")
            {
                string tenPhim = lblTenPhim4.Text;
                decimal giaVe = 30000; // Tạm thời fix cứng hoặc lấy từ biến toàn cục nếu bạn lưu
                string currentUser = "admin"; // Lấy từ biến lưu lúc đăng nhập (bài trước)

                // Mở Form Nut1 và truyền dữ liệu sang
                Nut4 formDatVe = new Nut4(tenPhim, giaVe, currentUser);
                formDatVe.ShowDialog(); // ShowDialog để bắt buộc xử lý xong mới quay lại
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_role == "Admin")
            {
                AdminPhim adminForm = new AdminPhim();
                adminForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập!");
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            // 1. Hỏi cho chắc ăn
            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                
                Form1 loginForm = new Form1();
                loginForm.Show();

                // 3. Đóng Form hiện tại (Trang chủ)
                this.Close();


            }
        }

        private async void GiaoDienNguoiDung_Load(object sender, EventArgs e)
        {
            await LoadMovies();
        }
        private async System.Threading.Tasks.Task LoadMovies()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // 1. Gọi API lấy danh sách phim
                    string apiUrl = "https://localhost:7071/api/movie"; // Sửa PORT
                    var response = await client.GetStringAsync(apiUrl);

                    // 2. Convert JSON sang List
                    List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(response);
                    _danhSachPhimHienTai = movies;

                    // 3. Hiển thị lên giao diện (Cách thủ công cho 4 ô)

                    // --- Ô SỐ 1 ---
                    if (movies.Count >= 1)
                    {
                        lblTenPhim1.Text = movies[0].Title;
                        try 
                        { 
                            picPhim1.Load(movies[0].PosterUrl); 
                        } 
                        catch { /* Nếu ảnh lỗi thì kệ nó, không hiện popup */ }
                    }

                    // --- Ô SỐ 2 ---
                    if (movies.Count >= 2)
                    {
                        lblTenPhim2.Text = movies[1].Title;
                        try
                        {
                            picPhim2.Load(movies[1].PosterUrl);
                        }
                        catch { }
                    }

                    // --- Ô SỐ 3 ---
                    if (movies.Count >= 3)
                    {
                        lblTenPhim3.Text = movies[2].Title;
                        try
                        {
                            picPhim3.Load(movies[2].PosterUrl);
                        }
                        catch { }
                    }

                    // --- Ô SỐ 4 ---
                    if (movies.Count >= 4)
                    {
                        lblTenPhim4.Text = movies[3].Title;
                        try
                        {
                            picPhim4.Load(movies[3].PosterUrl);
                        }
                        catch { }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách phim: " + ex.Message);
            }
        }
    }
}
