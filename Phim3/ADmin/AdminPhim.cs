using Newtonsoft.Json;
using Phim3.ADmin;
using Phim3.MainChinh;
using Phim3.PhanDangNhap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // Thư viện biểu đồ
namespace Phim3
{
    public partial class AdminPhim : Form
    {

        public AdminPhim()
        {
            InitializeComponent();
            cbRole.SelectedIndex = 0; // Mặc định chọn cái đầu tiên (Admin/User) 12/1//2025 : Cập nhật thêm tài khoản
        }
        private async System.Threading.Tasks.Task LoadUsers()
        {
            try
            {
                HttpClient client = new HttpClient();
                
                    // Gọi API lấy danh sách
                    string apiUrl = "https://localhost:7071/api/auth/users"; // SỬA PORT
                    var response = await client.GetStringAsync(apiUrl);

                    // Chuyển đổi JSON sang danh sách
                    var userList = JsonConvert.DeserializeObject<List<UserDto>>(response);

                    // Đổ dữ liệu vào bảng (DataGridView)
                    dgvTaiKhoan.DataSource = userList;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách user: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            GiaoDienNguoiDung fgiaoDienNguoiDung = new GiaoDienNguoiDung("Admin", "Admin");
            fgiaoDienNguoiDung.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Giả sử phim 1 có ID là 1 (hoặc bạn phải lưu ID này vào biến toàn cục lúc load phim)
            int idPhim = 1;

            // Lấy dữ liệu hiện tại đang hiển thị để ném sang form sửa
            string tenPhim = "Avengers: EndGame"; // Tốt nhất là lấy từ biến movie[0].Title
            int thoiLuong = 181;
            decimal giaTien = 95000;

            // Mở form sửa
            CapNhatcs editForm = new CapNhatcs(idPhim, tenPhim, thoiLuong, giaTien);

            editForm.ShowDialog(); // Chờ admin sửa xong

            // Sau khi sửa xong, tải lại trang dashboard để thấy thay đổi
            LoadDashboard(); // Gọi lại hàm Load nãy mình viết
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Giả sử phim 1 có ID là 1 (hoặc bạn phải lưu ID này vào biến toàn cục lúc load phim)
            int idPhim = 2;

            // Lấy dữ liệu hiện tại đang hiển thị để ném sang form sửa
            string tenPhim = "The Conjuring"; // Tốt nhất là lấy từ biến movie[0].Title
            int thoiLuong = 112;
            decimal giaTien = 85000;

            // Mở form sửa
            CapNhatcs editForm = new CapNhatcs(idPhim, tenPhim, thoiLuong, giaTien);

            editForm.ShowDialog(); // Chờ admin sửa xong

            // Sau khi sửa xong, tải lại trang dashboard để thấy thay đổi
            LoadDashboard(); // Gọi lại hàm Load nãy mình viết
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Giả sử phim 1 có ID là 1 (hoặc bạn phải lưu ID này vào biến toàn cục lúc load phim)
            int idPhim = 3;

            // Lấy dữ liệu hiện tại đang hiển thị để ném sang form sửa
            string tenPhim = "The Shawshank Redemption"; // Tốt nhất là lấy từ biến movie[0].Title
            int thoiLuong = 142;
            decimal giaTien = 70000;

            // Mở form sửa
            CapNhatcs editForm = new CapNhatcs(idPhim, tenPhim, thoiLuong, giaTien);

            editForm.ShowDialog(); // Chờ admin sửa xong

            // Sau khi sửa xong, tải lại trang dashboard để thấy thay đổi
            LoadDashboard(); // Gọi lại hàm Load nãy mình viết
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int idPhim = 4;

            // Lấy dữ liệu hiện tại đang hiển thị để ném sang form sửa
            string tenPhim = "Inception"; // Tốt nhất là lấy từ biến movie[0].Title
            int thoiLuong = 148;
            decimal giaTien = 90000;

            // Mở form sửa
            CapNhatcs editForm = new CapNhatcs(idPhim, tenPhim, thoiLuong, giaTien);

            editForm.ShowDialog(); // Chờ admin sửa xong

            // Sau khi sửa xong, tải lại trang dashboard để thấy thay đổi
            LoadDashboard(); // Gọi lại hàm Load nãy mình viết
        }

        private async void AdminPhim_Load(object sender, EventArgs e)
        {
            await LoadDashboard();
            await LoadUsers();
        }
        private async System.Threading.Tasks.Task LoadDashboard()
        {
            try
            {
                HttpClient client = new HttpClient();
                
                    // Gọi API lấy báo cáo
                    // SỬA PORT API CHO ĐÚNG MÁY BẠN
                    string apiUrl = "https://localhost:7071/api/stats/dashboard";

                    var response = await client.GetStringAsync(apiUrl);

                    // Giải nén dữ liệu
                    var stats = JsonConvert.DeserializeObject<DashboardStats>(response);

                    // Hiển thị lên 4 cái Label (Nhớ đặt tên Label chưa?)
                    lblDoanhThu.Text = stats.Revenue.ToString("N0") + " VNĐ"; // Định dạng tiền tệ
                    lblVeDaban.Text = stats.Tickets.ToString() + " Vé";
                    lblSoLuongPhim.Text = stats.Movies.ToString() + " Phim";
                    lblSokhachHang.Text = stats.Customers.ToString() + " Người";
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không lấy được thống kê: " + ex.Message);
            }
        }

        private void lblVeDaban_Click(object sender, EventArgs e)
        {

        }

        private void lblSokhachHang_Click(object sender, EventArgs e)
        {

        }

        private async void btnXoaUser_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có dòng nào đang được chọn không
            if (dgvTaiKhoan.SelectedRows.Count > 0)
            {
                // Lấy ID của dòng đang chọn
                // Cells[0] thường là cột Id (nếu UserDTO bạn để Id ở đầu)
                int userId = Convert.ToInt32(dgvTaiKhoan.SelectedRows[0].Cells[0].Value);
                string username = dgvTaiKhoan.SelectedRows[0].Cells[1].Value.ToString();

                // Hỏi cho chắc ăn
                DialogResult dialogResult = MessageBox.Show($"Bạn chắc chắn muốn xóa user '{username}' chứ?", "Cảnh báo", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    // Gọi API xóa
                    HttpClient client = new HttpClient();
                    
                        string apiUrl = "https://localhost:7071/api/auth/users/" + userId;
                        var response = await client.DeleteAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Đã xóa thành công!");
                            await LoadUsers(); // Tải lại bảng để cập nhật
                            await LoadDashboard(); // Tải lại thống kê (vì số khách hàng đã giảm)
                        }
                        else
                        {
                            MessageBox.Show("Lỗi khi xóa!");
                        }
                    
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!");
            }
        }
        // 12/1/2025 : Cập nhật thêm tài khoản

        private async void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra nhập thiếu
            if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPass.Text))
            {
                MessageBox.Show("Vui lòng nhập tên và mật khẩu!");
                return;
            }

            // 2. Lấy dữ liệu từ giao diện
            var newUser = new
            {
                Username = txtUser.Text,
                Password = txtPass.Text,
                Email = txtEmail.Text,
                Role = cbRole.Text // Lấy quyền Admin/User
            };

            // Kiểm tra nếu chưa chọn quyền thì mặc định là User
            if (string.IsNullOrEmpty(newUser.Role)) newUser = new { newUser.Username, newUser.Password, newUser.Email, Role = "User" };

            // 3. Gọi API thêm mới
            try
            {
                HttpClient client = new HttpClient(); // Hoặc dùng ApiClient.GetClient() nếu đã làm bài trước
                
                    string json = JsonConvert.SerializeObject(newUser);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // SỬA PORT CỦA BẠN VÀO ĐÂY
                    string apiUrl = "https://localhost:7071/api/auth/create-user";

                    var response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Thêm tài khoản thành công!");

                        // --- ĐOẠN NÀY KHÁC FORM CŨ ---

                        // A. Xóa trắng các ô nhập để nhập người tiếp theo
                        txtUser.Text = "";
                        txtPass.Text = "";
                        txtEmail.Text = "";
                        cbRole.SelectedIndex = -1; // Bỏ chọn

                        // B. Tải lại bảng danh sách bên trên để thấy ngay người vừa thêm
                        await LoadUsers();

                        // C. Cập nhật luôn thống kê khách hàng
                        await LoadDashboard();
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Lỗi: " + error);
                    }
                
            }
            catch (Exception ex) { MessageBox.Show("Lỗi kết nối: " + ex.Message); }
        }

        private void btnDangXuatAdmin_Click(object sender, EventArgs e)
        {
            // 1. Hỏi xác nhận cho chắc ăn
            DialogResult result = MessageBox.Show("Admin có chắc muốn đăng xuất không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 2. Mở lại Form Đăng nhập
                // (Lưu ý: Nếu Form đăng nhập của bạn tên khác thì sửa lại chỗ này nhé)
                Form1 loginForm = new Form1();
                loginForm.Show();

                // 3. Đóng Form Admin hiện tại
                this.Close();
            }
        }
    }
    public class DashboardStats
    {
        public decimal Revenue { get; set; }
        public int Tickets { get; set; }
        public int Movies { get; set; }
        public int Customers { get; set; }
    }
}
