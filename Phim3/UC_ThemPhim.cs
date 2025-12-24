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
    public partial class UC_ThemPhim : UserControl
    {
        public UC_ThemPhim()
        {
            InitializeComponent();
        }

        private void UC_ThemPhim_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra nhập liệu cơ bản (Validate Client)
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên phim và Giá vé!");
                return;
            }

            try
            {
                // 2. Gom dữ liệu (Chuyển đổi số cẩn thận)
                // LƯU Ý: Tên các thuộc tính (Title, Genre...) phải KHỚP với file Movie.cs bên Server
                var newMovie = new
                {
                    Title = txtTitle.Text,
                    Description = txtDescription.Text, // Nếu không có textbox này thì bỏ dòng này
                    Genre = txtGenre.Text,             // Nếu không có textbox này thì bỏ dòng này
                    PosterUrl = txtPosterUrl.Text,     // Link ảnh

                    // Xử lý số: Nếu lỗi định dạng số thì nó sẽ nhảy xuống catch
                    Duration = int.Parse(txtDuration.Text),
                    Price = decimal.Parse(txtPrice.Text),
                    ReleaseDate = dtpNgayChieu.Value
                };

                // 3. Gọi API (Sử dụng HttpClient trực tiếp để dễ debug lỗi)
                using (var client = new HttpClient())
                {
                    // --- KIỂM TRA LẠI PORT Ở ĐÂY (7071 hay 7123?) ---
                    string url = "https://localhost:7500/api/movie/add";

                    // Ép kiểu JSON
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(newMovie);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);

                    // 4. ĐỌC LỖI TỪ SERVER (QUAN TRỌNG NHẤT)
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("✅ Thêm phim thành công!");
                        // Xóa trắng ô nhập
                        txtTitle.Text = ""; txtPrice.Text = ""; txtDuration.Text = "";
                    }
                    else
                    {
                        // Hiện nguyên văn lỗi Server trả về để biết đường sửa
                        MessageBox.Show($"❌ Lỗi Server ({response.StatusCode}): \n{responseBody}");
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi định dạng: Thời lượng và Giá vé phải là số nguyên/số thực!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

        // Hàm xóa trắng ô nhập
        private void ClearInputs()
        {
            txtTitle.Clear();
            txtGenre.Clear();
            txtDuration.Clear();
            txtPrice.Clear();
            txtPosterUrl.Clear();
            txtDescription.Clear();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
  