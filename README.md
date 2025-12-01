# Đồ án Hệ thống Bán vé xem phim (WinForms 3 Tầng)

Đây là project môn học sử dụng WinForms (.NET 8), ASP.NET Core Web API và SQL Server, mô phỏng hệ thống quản lý rạp chiếu phim PhimMoi.

## Hướng dẫn cài đặt và chạy (Quan trọng)

## 1. Cách Tải Code (Rất Quan Trọng!)

Bạn có 2 cách để tải, nhưng **Cách 1 (Git Clone) là cách tốt nhất** để không bị lỗi bảo mật "Mark of the Web".

### Cách 1: Dùng Git Clone (Khuyên Dùng)

1.  Trên trang GitHub, bấm vào nút màu xanh lá **`<> Code`**.
2.  Copy đường link **HTTPS** (ví dụ: `https://github.com/TenBan/DuAnBanVePhim.git`).
3.  Mở **Visual Studio** lên (mở app thôi, không cần mở project).
4.  Ở cửa sổ khởi động, chọn **"Clone a repository"** (Nhân bản một kho chứa).
5.  Dán cái link HTTPS đó vào -> Bấm **Clone**.

👉 Visual Studio sẽ tự tải code về, và bạn có thể chạy được luôn mà **không cần làm "Unblock"** gì cả!

---

### Cách 2: Tải file ZIP (Nếu dùng cách này, BẮT BUỘC phải làm thêm)

1.  Bấm `Code` -> **Download ZIP**.
2.  **TRƯỚC KHI GIẢI NÉN:** Chuột phải vào file `.zip` vừa tải về.
3.  Chọn **Properties**.
4.  Ở tab General, tích vào ô **"Unblock"** -> Bấm **OK**.
5.  Bây giờ mới giải nén file `.zip`.

2.  **Cài đặt Database:**
    * Mở SQL Server Management Studio (SSMS).
    * Mở file `script.sql` (đã có sẵn trong thư mục này).
    * Bấm **Execute** để tạo Database và dữ liệu mẫu (phim, tài khoản admin).

3.  **Cấu hình API (Project Phim3API):**
    * Mở file `Phim3API/appsettings.json`.
    * Sửa lại dòng `ConnectionStrings` cho đúng với tên Server/Database SQL của máy bạn.

**Phần Tiếp theo sau khi đã cài đặt thành công**

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
