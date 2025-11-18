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

namespace Phim3
{
    public partial class AdminPhim : Form
    {
        public AdminPhim()
        {
            InitializeComponent();
        }
        private async System.Threading.Tasks.Task LoadUsers()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Gọi API lấy danh sách
                    string apiUrl = "https://localhost:7071/api/auth/users"; // SỬA PORT
                    var response = await client.GetStringAsync(apiUrl);

                    // Chuyển đổi JSON sang danh sách
                    var userList = JsonConvert.DeserializeObject<List<UserDto>>(response);

                    // Đổ dữ liệu vào bảng (DataGridView)
                    dgvTaiKhoan.DataSource = userList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách user: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            GiaoDienNguoiDung fgiaoDienNguoiDung = new GiaoDienNguoiDung();
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
                using (HttpClient client = new HttpClient())
                {
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
                    using (HttpClient client = new HttpClient())
                    {
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
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!");
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
