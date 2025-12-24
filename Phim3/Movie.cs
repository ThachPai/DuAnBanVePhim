using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phim3
{
    internal class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PosterUrl { get; set; }
        public decimal Price { get; set; }



        public string Description { get; set; } // Thêm mô tả
        public string Genre { get; set; }       // Thêm thể loại
        public int Duration { get; set; }       // Thêm thời lượng


        public DateTime? ReleaseDate { get; set; }
    }
}