using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.Utility.Services
{
    public class ChatHub:Hub
    {

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                Console.WriteLine("User connected without userId");
            }
            else
            {
                Console.WriteLine($"User connected: {userId}");
            }

            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string receiverUserId, string message)
        {
            await Clients.User(receiverUserId).SendAsync("ReceiveNotification", message);
        }

        public async Task AcceptNotification(string senderUserId, string message)
        {
            await Clients.User(senderUserId).SendAsync("ReceiveNotification", message);
        }
    }
}
    