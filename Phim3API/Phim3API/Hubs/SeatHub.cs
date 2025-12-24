using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
namespace Phim3API.Hubs;
public class SeatHub : Hub
{
    // Phải có ĐỦ 4 tham số này
    public async Task SendSeatStatus(int showtimeId, string seatName, string status, int userId)
    {
        // Gửi lại đủ 4 tham số cho tất cả mọi người trong nhóm
        await Clients.Group(showtimeId.ToString())
                     .SendAsync("ReceiveSeatStatus", showtimeId, seatName, status, userId);
    }

    public async Task JoinShowtimeGroup(int showtimeId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, showtimeId.ToString());
    }
}