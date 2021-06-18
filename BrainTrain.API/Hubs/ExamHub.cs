using BrainTrain.Core.Models;
using BrainTrain.Core.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Hubs
{
    public class ExamHub : Hub
    {
        private static IHubContext<ExamHub> _hubContext;
        private readonly BrainTrainContext db;

        public ExamHub(BrainTrainContext _db, IHubContext<ExamHub> hubContext)
        {
            db = _db;
            _hubContext = hubContext;
        }


        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"];

            await Groups.AddToGroupAsync(Context.ConnectionId, userId);

            StatsHandler.ExamConnectedIds.Add(new UserTimeStore { UserName = userId, StartTime = DateTime.Now });

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"];

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            var us = StatsHandler.ExamConnectedIds.FirstOrDefault(u => u.UserName == userId);

            if (us != null)
            {
                StatsHandler.ExamConnectedIds.Remove(us);

                var user = db.Midterm_Users.Include(u => u.Midterm_UserEvents).FirstOrDefault(u => u.Id.ToString() == userId);
                if (user != null)
                {
                    var seconds = (DateTime.Now - us.StartTime).TotalSeconds;
                    var ev = user.Midterm_UserEvents.FirstOrDefault();

                    if (ev.SecondsSpent == null)
                        ev.SecondsSpent = seconds;
                    else
                        ev.SecondsSpent += seconds;

                    db.SaveChanges();

                }
            }

            await base.OnDisconnectedAsync(ex);
        }
    }
}
