using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityUsers.Hubs;
using IdentityUsers.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace IdentityUsers.Pages.Account
{
    public class MessageModel : PageModel
    {
        private readonly IHubContext<NotificationUserHub> _notificationUserHubContext;
        private readonly IUserConnectionManager _userConnectionManager;
        private static string userId;

        public MessageModel(IHubContext<NotificationUserHub> notificationUserHubContext,
           IUserConnectionManager userConnectionManager)
        {
            _notificationUserHubContext = notificationUserHubContext;
            _userConnectionManager = userConnectionManager;
        }

        public void OnGet(string id)
        {

            if (!string.IsNullOrEmpty(id))
                userId = id;

        }

        public async Task<IActionResult> OnPostAsync([FromForm] string message)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                //get the connection from the
                var connections = _userConnectionManager.GetUserConnections(userId);
                if (connections != null && connections.Count > 0)
                {
                    foreach (var connectionId in connections)
                    {
                        //send to user
                        await _notificationUserHubContext.Clients
                            .Client(connectionId).SendAsync("sendToUser", message);
                    }
                }
            } 

            return Page();
        }

    }
}
