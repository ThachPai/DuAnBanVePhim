Hoàn thiện việc nâng cấp bảo mật & Tính năng đồng bộ ghế Real-time
1. Database SQL Server
Thực hiện chuẩn hóa và mở rộng database để hỗ trợ bảo mật và tính năng đặt vé chi tiết.
🔹 Nâng cấp bảng Users (Bảo mật)
Sửa đổi: Mở rộng cột Password lên NVARCHAR(255) để lưu mã hóa BCrypt.
Dữ liệu: Xóa các tài khoản cũ lưu mật khẩu dạng văn bản thuần (plaintext).
Thêm cột: ResetToken và ResetTokenExpires để hỗ trợ quy trình "Quên mật khẩu" an toàn.
🔹 Thêm mới các bảng (Quản lý Rạp & Ghế)
Rooms: Quản lý thông tin phòng chiếu.
Showtimes: Quản lý suất chiếu (kết nối Phim - Phòng - Giờ chiếu).
Seats: Định nghĩa sơ đồ ghế (Hàng, Số, Loại ghế VIP/Thường) cho từng phòng.
BookedSeats: Lưu trạng thái ghế đã bán của từng suất chiếu (phục vụ tính năng Real-time).
🔹 Dữ liệu mẫu (Sample Data)
Đã tạo script SQL để khởi tạo một Phòng chiếu Demo 20 ghế (4 hàng x 5 cột) và 1 suất chiếu mẫu.

2.Backend
Bảo mật & Xác thực (Authentication)
Cài đặt thư viện: BCrypt.Net-Next (Mã hóa) và Microsoft.AspNetCore.Authentication.JwtBearer (Token).
Cấu hình: Thêm Secret Key vào appsettings.json và bật dịch vụ Authentication trong Program.cs.
Controller AuthController:
Đăng ký: Tự động mã hóa mật khẩu (Hash) trước khi lưu.
Đăng nhập: Kiểm tra mật khẩu bằng BCrypt.Verify và trả về JWT Token (thay vì trả về User object).
Quên mật khẩu: Logic tạo Token ngẫu nhiên và xác thực Token khi đặt lại mật khẩu mới.
🔹 Tính năng Real-time (SignalR)
Cấu hình: Đăng ký dịch vụ SignalR và map đường dẫn /seatHub.
Hub SeatHub: Tạo trạm phát sóng để nhận tín hiệu chọn ghế từ Client và phát lại cho tất cả người dùng khác.
Controller BookingController:
API GET /seats: Lấy trạng thái ghế (đã bán/còn trống) từ Database.
API POST /book: Xử lý đặt vé, lưu vào bảng BookedSeats, và gửi tín hiệu Real-time cập nhật trạng thái ghế "Đã bán".
3.Frontend
Cấu trúc & Helper (Nền tảng)
Thêm thư viện System.Text.Json.
Tạo bộ file Helper để tái sử dụng code:
ApiClient.cs: Quản lý HttpClient, cấu hình BASE_URL và xử lý lỗi SSL (localhost).
GlobalToken.cs: Lưu trữ JWT Token phiên làm việc.
AuthModels.cs: Các class DTO (LoginRequest, RegisterRequest...) để đồng bộ dữ liệu với API.
🔹 Chức năng Xác thực
Đăng nhập (Form1.cs): Sửa logic để nhận JWT Token và lưu vào GlobalToken.
Đăng ký (Form2.cs): Sửa logic để gọi API đăng ký bảo mật.
Đăng xuất: Thực hiện xóa Token khỏi bộ nhớ client.
🔹 Chức năng Đặt vé Real-time
Form ChonGhe.cs (Mới):
Giao diện sơ đồ ghế trực quan (20 ghế).
Logic Real-time: Kết nối SignalR Client, tự động đổi màu ghế khi có người khác chọn.
Logic Nghiệp vụ: Tải trạng thái ghế đã bán từ API khi mở form. Giới hạn số lượng ghế được chọn theo số vé đã mua.
Kết nối: Đã gắn logic mở form chọn ghế vào quy trình đặt vé.