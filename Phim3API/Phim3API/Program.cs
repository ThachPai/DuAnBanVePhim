using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Phim3API.Hubs ; // Thêm dòng này để tìm thấy SeatHub
using System.Text.Json.Serialization;

// 1. KHỞI TẠO BUILDER (Phải làm đầu tiên)
var builder = WebApplication.CreateBuilder(args);

// 2. ĐĂNG KÝ DỊCH VỤ (Add Services)
// ----------------------------------

// Thêm SignalR (Để làm ghế Real-time)
builder.Services.AddSignalR();

builder.Services.AddControllers().AddJsonOptions(x =>
{
    // Dòng này cực kỳ quan trọng để sửa lỗi Swagger 500 khi có quan hệ bảng
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký kết nối SQL Server
builder.Services.AddDbContext<Phim3API.Data.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký xác thực JWT (Bảo mật)
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("AppSettings:Token").Value!)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// 3. XÂY DỰNG APP
// ----------------------------------
var app = builder.Build();

// 4. CẤU HÌNH PIPELINE (Use & Map)
// ----------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Quan trọng: Authentication phải ĐỨNG TRƯỚC Authorization
app.UseAuthentication();
app.UseAuthorization();

// Định tuyến (Map)
app.MapControllers();
app.MapHub<SeatHub>("/seatHub"); // Đường dẫn cho SignalR

// 5. CHẠY APP (Dòng cuối cùng)
app.Run();