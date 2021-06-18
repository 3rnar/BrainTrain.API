using BrainTrain.Core.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Hubs
{
    public class EventHub : Hub
    {
        private static IHubContext<EventHub> _hubContext;
        private static List<string> connectedUsers = new List<string>();

        public EventHub(IHubContext<EventHub> hubContext)
        {
            _hubContext = hubContext;
        }


        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userName = httpContext.Request.Query["userName"];
            await Groups.AddToGroupAsync(Context.ConnectionId, userName);

            if (!connectedUsers.Exists(x => x == userName))
            {
                connectedUsers.Add(userName);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var httpContext = Context.GetHttpContext();
            var userName = httpContext.Request.Query["userName"];
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userName);
            connectedUsers.Remove(userName);
            await base.OnDisconnectedAsync(ex);
        }

        //public static void Notify(Event e, string userName)
        //{
        //    hubContext.Clients.Group(userName).ShowNotification(JsonConvert.SerializeObject(e));
        //}

        [HubMethodName("SendNotification")]
        public async Task SendNotification(Event e, string userName)
        {
            if (connectedUsers.Any(a => a == userName))
            {
                await _hubContext.Clients.Group(userName).SendAsync("ShowNotification", JsonConvert.SerializeObject(e));
            }
        }
    }
}
