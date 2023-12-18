using Microsoft.AspNetCore.SignalR;

namespace socialmediaAPI.Hubs
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext?.Request.Cookies["userID"];
            await Groups.AddToGroupAsync(Context.ConnectionId, userId ?? "null");
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext?.Request.Cookies["userID"];
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId ?? "null");

            // Notify clients that a user disconnected
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string receiverId,string senderId, string message)
        {
            if (!string.IsNullOrEmpty(receiverId))
            {
                // Send the message to a specific user
                await Clients.Group(receiverId).SendAsync("ReceiveMessage", senderId, message);
            }
            else
            {
                // Send the message to all clients
                await Clients.All.SendAsync("ReceiveMessage", senderId, message);
            }
        }
    }
}
