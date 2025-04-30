using Microsoft.AspNetCore.SignalR;

namespace SystemProject_Hotel_Management_.Hubs
{
    public class OrderHub : Hub
    {
        public async Task SendOrderStatusUpdate(int orderId, string status)
        {
            
            await Clients.All.SendAsync("ReceiveOrderStatusUpdate", orderId, status);
        }
    }
}
