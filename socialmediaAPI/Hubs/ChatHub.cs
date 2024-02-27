using Microsoft.AspNetCore.SignalR;
using socialmediaAPI.Models.Entities;
using System.Security.Claims;

namespace socialmediaAPI.Hubs
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            if (user?.Identity?.IsAuthenticated != true)
                return;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return;
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = Context.User;
            if (user?.Identity?.IsAuthenticated!=true)
                return;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return;
            await Clients.All.SendAsync("UserDisconnected", userId);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(List<string> receiverIds, string conversationId, Message message)
        {
            var senderId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (senderId == null)
                return;
            await Clients.Groups(receiverIds).SendAsync("ReceiveMessage", conversationId, message);
        }
        public async Task DeleteMessage(List<string> receiverIds, string conversationId, string messageId)
        {
            var senderId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (senderId == null)
                return;
            await Clients.Groups(receiverIds).SendAsync("DeleteMessage", conversationId, messageId);
        }
    }
}
