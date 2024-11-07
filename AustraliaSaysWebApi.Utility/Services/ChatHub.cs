using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.Utility.Services
{
    public class ChatHub:Hub
    {
        public async Task SendNotification(string receiverUserId, string message)
        {
            await Clients.User(receiverUserId).SendAsync("ReceiveNotification", message);
        }

        public async Task AcceptNotification(string senderUserId, string message)
        {
            await Clients.User(senderUserId).SendAsync("ReceiveNotification", message);
        }
    }
}
