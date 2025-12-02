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

            // Giả sử phim 1 là Avengers
            // TẠM THỜI: Fix cứng Suất chiếu số 1 (9h sáng) để test trước
            int showtimeId = 1;

            // Lấy thông tin phim (như bài trước)
            string tenPhim = _danhSachPhimHienTai[0].Title;
            decimal giaVe = _danhSachPhimHienTai[0].Price;

            // Mở Form Chọn Ghế "Xịn"
            FormChonGhe formGhe = new FormChonGhe(showtimeId, tenPhim, giaVe, _username);
            formGhe.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_danhSachPhimHienTai.Count < 2) return;

            var phim = _danhSachPhimHienTai[1]; // Phim thứ 2
            int showtimeId = 2; // Khớp với SQL

            FormChonGhe formGhe = new FormChonGhe(showtimeId, phim.Title, phim.Price, _username);
            formGhe.ShowDialog();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            if (_danhSachPhimHienTai.Count < 3) return;

            var phim = _danhSachPhimHienTai[2]; // Phim thứ 3
            int showtimeId = 3;

            FormChonGhe formGhe = new FormChonGhe(showtimeId, phim.Title, phim.Price, _username);
            formGhe.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (_danhSachPhimHienTai.Count < 4) return;

            var phim = _danhSachPhimHienTai[3]; // Phim thứ 4
            int showtimeId = 4;

            FormChonGhe formGhe = new FormChonGhe(showtimeId, phim.Title, phim.Price, _username);
            formGhe.ShowDialog();
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
                HttpClient client = new HttpClient();

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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách phim: " + ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
