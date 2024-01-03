using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Server.Models.Request;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("OnConnect", $"{Context.ConnectionId} connected");
            var userId = Context.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // Group for current user. All notifications will come here
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);

            // connect to existing chats
            var chats = _chatService.GetAll(userId);

            foreach (var chat in chats)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("OnDisconnect", $"{Context.ConnectionId} disconnected");
        }

        public async Task AddToGroupAsync(Guid chatId, string userId)
        {
            var chat = await _chatService.AddUserAsync(chatId, userId);
            await Clients.Group(userId).SendAsync("OnAddedToGroup", chat);
        }

        public async Task SubscribeToGroupAsync(Guid chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task RemoveFromGroupAsync(Guid chatId, string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
            await Clients.Group(userId).SendAsync("OnRemovedFromGroup");
        }

        public async Task Send(string authorId, Guid chatId, MessageCreateDto dto)
        {
            var message = await _chatService.SendMessage(authorId, chatId, dto);
            await Clients.Group(chatId.ToString()).SendAsync("OnRecieveMessage", message);
        }
    }
}