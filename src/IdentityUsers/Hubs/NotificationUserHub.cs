using System;
using System.Threading.Tasks;
using IdentityUsers.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace IdentityUsers.Hubs
{
    [Authorize]
    public class NotificationUserHub : Hub
    {
        private readonly IUserConnectionManager _userConnectionManager;

        public NotificationUserHub(IUserConnectionManager userConnectionManager)
        {
            _userConnectionManager = userConnectionManager;
        }

        public async Task<string> GetConnectionId()
        {
            var httpContext = this.Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"];
            await _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);

            return Context.ConnectionId;
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            //get the connectionId
            var connectionId = Context.ConnectionId;
            await _userConnectionManager.RemoveUserConnection(connectionId);
        }

        public async Task SendToUser(string connectionId, string message)
        {
            await Clients.Client(connectionId).SendAsync("sendToUser", message);
        }
    }
}
