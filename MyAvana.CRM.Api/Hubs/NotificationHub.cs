using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MyAvana.CRM.Api.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task RoomsUpdated(bool flag)
           => await Clients.Others.SendAsync("RoomsUpdated", flag);
    }
}
