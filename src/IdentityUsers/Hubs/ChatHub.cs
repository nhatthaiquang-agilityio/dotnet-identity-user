using System;
using System.Threading.Tasks;
using IdentityUsers.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace IdentityUsers.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly IUserConnectionManager _userConnectionManager;

        public ChatHub(IUserConnectionManager userConnectionManager)
        {
            _userConnectionManager = userConnectionManager;
        }

        public async Task<string> GetConnectionId()
        {
            var userId = this.Context.GetHttpContext().Request.Query["userId"];
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

        public async Task SendToUserId(string userId, string message)
        {
            if (!string.IsNullOrEmpty(userId))
            {
               //get the connection from the
                var connections = _userConnectionManager.GetUserConnections(userId);

                if (connections != null && connections.Count > 0)
                {
                    foreach (var connectionId in connections)
                    {
                        var senderId = this.Context.GetHttpContext().Request.Query["userId"];

                        var msgObj = new ObjectMessage(senderId, message);

                        //send to user
                        await Clients.Client(connectionId).SendAsync("SendToUserId", msgObj);
                    }
               }
            }
        }

        public async Task ReceiveMessageToRoom(string username, string message)
        {
            await Clients.All.SendAsync("ReceiveMessageToRoom", username, message);
        }
    }

    public struct ObjectMessage
    {
        public string sender;
        public string message;

        public ObjectMessage(string strsender, string strmessage)
        {
            sender = strsender;
            message = strmessage;
        }
    }

}
