🎬 Hệ Thống Đặt Vé Xem Phim Real-time (Bản Hoàn Thiện)
Dự án đã được nâng cấp toàn diện từ cấu trúc Database đến các tính năng cao cấp như đồng bộ hóa thời gian thực và tự động hóa quy trình nghiệp vụ.

1. Kiến Trúc Database & Backend (SQL Server & ASP.NET Core)
Chuẩn hóa dữ liệu: Tái cấu trúc toàn bộ các bảng chính (Users, Movies, Showtimes) với cơ chế IDENTITY(1,1), đảm bảo tính toàn vẹn dữ liệu và tự động hóa việc quản lý khóa chính.

Bảo mật hệ thống: Tích hợp mã hóa mật khẩu bằng thuật toán BCrypt và xác thực quyền truy cập qua JWT Token, đảm bảo an toàn cho dữ liệu người dùng.

Logic nghiệp vụ thông minh: * Xây dựng lớp API trung gian để giải quyết triệt để sự nhầm lẫn giữa MovieId và ShowtimeId.

Phát triển tính năng Auto-Showtime: Tự động khởi tạo lịch chiếu và sơ đồ ghế ngay khi thêm phim mới vào hệ thống.

2. Công Nghệ Đồng Bộ Thời Gian Thực (SignalR)
Cơ chế "Trọng tài Server": Ngăn chặn hoàn toàn tình trạng đặt trùng ghế (Race Condition) bằng cách xử lý yêu cầu giữ ghế trực tiếp tại Server trước khi phản hồi cho Client.

Hệ thống Seat-Map đa trạng thái: * Sử dụng SignalR Groups để quản lý phiên chọn ghế theo từng suất chiếu riêng biệt.

Đồng bộ trạng thái ghế linh hoạt qua hệ thống mã màu trực quan: Vàng (ghế đang chọn), Cam (người khác đang giữ), Đỏ (đã bán).

3. Hệ Thống Thông Báo & Chăm Sóc Khách Hàng (SMTP)
Email Confirmation: Tích hợp tự động gửi thư xác nhận đặt vé ngay sau khi giao dịch thành công qua thư viện MailKit.

Trải nghiệm người dùng cao cấp: Nội dung Email được thiết kế bằng HTML Template chuyên nghiệp, tự động nhúng link ảnh Poster phim động, giúp người dùng dễ dàng kiểm tra thông tin vé trực quan.

4. Giao Diện Người Dùng (WinForms)
Tối ưu hóa UI/UX: Cải thiện luồng chuyển đổi giữa các Form quản lý phim và sơ đồ ghế, đảm bảo tốc độ phản hồi nhanh.

Lắng nghe sự kiện Hub: Khắc phục triệt để lỗi trễ trạng thái (bấm 2 lần) bằng cách đồng bộ hóa sự kiện Click với tín hiệu phản hồi từ SignalR Hub.

💡 Lưu ý cho người dùng sau (Teamwork):
Để hệ thống vận hành chính xác, vui lòng chạy file Script SQL được cung cấp để khởi tạo cấu trúc bảng và cập nhật ConnectionString trong file appsettings.json cho phù hợp với môi trường local.
