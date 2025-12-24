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
    public partial class UC_MovieItem : UserControl
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public decimal MoviePrice { get; set; }
        public string PosterUrl { get; set; }
        public int _duration;
        public DateTime _releaseDate;
        public UC_MovieItem(int id, string title, decimal price, string imgUrl, int duration, DateTime date)
        {
            InitializeComponent();
            this.MovieId = id;
            this.MovieTitle = title;
            this.MoviePrice = price;
            this.PosterUrl = imgUrl;
            this._duration = duration;
            this._releaseDate = date;


            // Gán dữ liệu lên giao diện
            lblTenPhim.Text = title;
            lblGia.Text = price.ToString("N0") + " đ";

            try
            {
                if (!string.IsNullOrEmpty(imgUrl))
                {
                    picPoster.LoadAsync(imgUrl); // Dùng LoadAsync cho mượt
                }
            }
            catch
            {
               
            }

            // Gắn sự kiện Click cho toàn bộ thẻ (bấm vào hình hay chữ đều ăn)
            this.Click += Movie_Click;
            picPoster.Click += Movie_Click;
            lblTenPhim.Click += Movie_Click;
        }

        private void Movie_Click(object sender, EventArgs e)
        {
            Phim3.Nut1 formDatVe = new Phim3.Nut1(
             this.MovieId,
             this.MovieTitle,
             this.MoviePrice,
             this.PosterUrl,
             this._duration,    // <-- Mới
             this._releaseDate  // <-- Mới
         );

            formDatVe.ShowDialog();
        }

        private void UC_MovieItem_Load(object sender, EventArgs e)
        {

        }
    }
}

