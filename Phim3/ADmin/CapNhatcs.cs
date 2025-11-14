using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phim3.PhanDangNhap
{
    public partial class CapNhatcs : Form
    {
        private int _movieId;
        public CapNhatcs(int id, string title, int duration, decimal price)
        {
            InitializeComponent();
            _movieId = id;
            // Điền dữ liệu cũ vào ô cho Admin thấy
            txtTenPhim.Text = title;
            txtThoiLuong.Text = duration.ToString();
            txtGiaTien.Text = price.ToString("N0").Replace(",", "").Replace(".", ""); // Bỏ dấu phẩy định dạng
        }

        private void CapNhatcs_Load(object sender, EventArgs e)
        {

        }

        private async void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Lấy dữ liệu mới từ ô nhập
            string newTitle = txtTenPhim.Text;
            int newDuration = int.Parse(txtThoiLuong.Text);
            decimal newPrice = decimal.Parse(txtGiaTien.Text);
            string newGenre = cbbTheLoai.Text;

            // 2. Đóng gói
            var updateData = new
            {
                Id = _movieId,
                Title = newTitle,
                Duration = newDuration,
                Price = newPrice,
                Genre = newGenre,
                // Nếu Form bạn không có ô nhập Poster/Desciption thì gửi kèm cái cũ hoặc chuỗi rỗng
                PosterUrl = "",
                Description = ""
            };

            // 3. Gọi API (Phương thức PUT)
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string json = JsonConvert.SerializeObject(updateData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // SỬA PORT + ID PHIM
                    string apiUrl = "https://localhost:7071/api/movie/" + _movieId;

                    var response = await client.PutAsync(apiUrl, content); // Dùng PutAsync

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Cập nhật thành công!");
                        this.Close(); // Đóng form
                    }
                    else
                    {
                        MessageBox.Show("Lỗi cập nhật: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void cbbTheLoai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
