using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phim3
{
    public static class SessionData
    {
        // Biến này sẽ lưu ID của người đang đăng nhập
        // Mặc định là null hoặc 0
        public static int? CurrentUserId { get; set; }

        public static string? CurrentUsername { get; set; }
    }
}
