# Đồ án Hệ thống Bán vé xem phim (WinForms 3 Tầng)

Đây là project môn học sử dụng WinForms (.NET 8), ASP.NET Core Web API và SQL Server, mô phỏng hệ thống quản lý rạp chiếu phim PhimMoi.

## Hướng dẫn cài đặt và chạy (Quan trọng)

1.  **Tải Code:**
    * Clone hoặc tải ZIP project này về máy.

2.  **Cài đặt Database:**
    * Mở SQL Server Management Studio (SSMS).
    * Mở file `script.sql` (đã có sẵn trong thư mục này).
    * Bấm **Execute** để tạo Database và dữ liệu mẫu (phim, tài khoản admin).

3.  **Cấu hình API (Project Phim3API):**
    * Mở file `Phim3API/appsettings.json`.
    * Sửa lại dòng `ConnectionStrings` cho đúng với tên Server/Database SQL của máy bạn.

4.  **Chạy ứng dụng (Bắt buộc):**
    * Mở file `.sln` bằng Visual Studio.
    * Chuột phải vào **Solution** (dòng trên cùng) -> **Configure Startup Projects...**
    * Chọn **Multiple startup projects**.
    * Đặt `Phim3API` (Project API) thành **Start**.
    * Đặt `Phim3` (Project WinForms) thành **Start**.
    * Bấm nút Play (F5). Cả API và App sẽ cùng chạy!

5.  **Tài khoản Test:**
    * **Admin:** `admin` / `123456`
    * **User:** Tự đăng ký
