using BrainTrain.Core.Models;
using BrainTrain.Core.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Hubs
{
    public class BattleHub : Hub
    {
        private static IHubContext<BattleHub> _hubContext;
        private readonly BrainTrainContext db;

        public BattleHub(BrainTrainContext _db, IHubContext<BattleHub> hubContext)
        {
            db = _db;
            _hubContext = hubContext;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userName = httpContext.Request.Query["userName"];
            await Groups.AddToGroupAsync(Context.ConnectionId, userName);

            foreach (var connection in StatsHandler.ConnectedIds)
            {
                await _hubContext.Clients.Group(connection.UserName).SendAsync("NotifyAboutConnect", userName);
            }

            StatsHandler.ConnectedIds.Add(new UserTimeStore { UserName = userName, StartTime = DateTime.Now });

            await base.OnConnectedAsync();
        }

        public async Task OnDisconnected(Exception ex)
        {
            var httpContext = Context.GetHttpContext();
            var userName = httpContext.Request.Query["userName"];
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userName);
            var us = StatsHandler.ConnectedIds.FirstOrDefault(u => u.UserName == userName);

            if (us != null)
            {
                StatsHandler.ConnectedIds.Remove(us);

                var user = db.ApplicationUsers.FirstOrDefault(u => u.UserName.Equals(userName));
                if (user != null)
                {
                    var seconds = (DateTime.Now - us.StartTime).TotalSeconds;
                    if (user.SecondsOnTheSite == null)
                        user.SecondsOnTheSite = seconds;
                    else
                        user.SecondsOnTheSite += seconds;

                    var battleQueues = db.BattleQueues.Include(b => b.FirstUser).Include(b => b.SecondUser).
                        Where(b => b.BattleResultStatusId == null &&
                            (b.FirstUserId == user.Id || b.SecondUserId == user.Id) &&
                            EF.Functions.DateDiffMinute(b.DateCreated, DateTime.Now) <= 20
                        ).ToList();

                    foreach (var bq in battleQueues)
                    {
                        bq.BattleResultStatusId = 5;
                    }

                    db.SaveChanges();

                    foreach (var connection in StatsHandler.ConnectedIds)
                    {
                        await _hubContext.Clients.Group(connection.UserName).SendAsync("NotifyAboutDisconnect", userName);
                    }

                    foreach (var bq in battleQueues)
                    {
                        if (bq.FirstUserId == user.Id)
                        {
                            await _hubContext.Clients.Group(bq.SecondUser.UserName).SendAsync("NotifyAboutOpponentDeclining", userName);
                        }
                        else
                        {
                            await _hubContext.Clients.Group(bq.FirstUser.UserName).SendAsync("NotifyAboutOpponentDeclining", userName);
                        }
                    }
                }
            }

            await base.OnDisconnectedAsync(ex);
        }


        public static async Task NotifyFirstUser(string firstUserName, string secondUserName)
        {
            await _hubContext.Clients.Group(firstUserName).SendAsync("NotifyFirstUser", secondUserName);
        }

        public static async Task NotifyFriendBattleOpponent(string firstUserName, string secondUserName)
        {
            await _hubContext.Clients.Group(firstUserName).SendAsync("NotifyFriendBattleOpponent", secondUserName);
        }

        [HubMethodName("SayAboutReadiness")]
        public async Task SayAboutReadiness()
        {
            var httpContext = Context.GetHttpContext();
            var userName = httpContext.Request.Query["userName"];
            var battleQueue = db.BattleQueues.Include(bq => bq.FirstUser).Include(bq => bq.SecondUser).OrderByDescending(bq => bq.Id).FirstOrDefault(bq => (bq.FirstUser.UserName == userName || bq.SecondUser.UserName == userName) &&
            EF.Functions.DateDiffMinute(bq.DateCreated, DateTime.Now) <= 20 && (bq.BattleResultStatusId == null || bq.BattleResultStatusId == 1));
            if (battleQueue != null)
            {
                if (battleQueue.FirstUser.UserName == userName)
                {
                    await _hubContext.Clients.Group(battleQueue.SecondUser.UserName).SendAsync("NotifyAboutReadiness", battleQueue.Id);
                }
                else
                {
                    await _hubContext.Clients.Group(battleQueue.FirstUser.UserName).SendAsync("NotifyAboutReadiness", battleQueue.Id);
                }
            }
        }

        [HubMethodName("SayAboutFinishing")]
        public async Task SayAboutFinishing(string opponentUserName)
        {
            var httpContext = Context.GetHttpContext();
            var userName = httpContext.Request.Query["userName"];

            var battleQueue = db.BattleQueues.
                Include(bq => bq.FirstUser).
                Include(bq => bq.SecondUser).
                Include(bq => bq.BattleQueuesToQuestions).
                Include(bq => bq.BattleQueuesToQuestions.Select(bqtq => bqtq.Question)).
                Include(bq => bq.BattleQueuesToQuestions.Select(bqtq => bqtq.Question.QuestionVariants)).
                OrderByDescending(bq => bq.Id).
                FirstOrDefault(bq => (bq.FirstUser.UserName == userName || bq.SecondUser.UserName == userName) && (bq.BattleResultStatusId == 1 || bq.BattleResultStatusId == 2));

            if (battleQueue.BattleResultStatusId == 1) // пользователь закончил первым
            {
                battleQueue.BattleResultStatusId = 2; //поменять статус на "Один из пользователей закончил"
            }
            else if (battleQueue.BattleResultStatusId == 2) // оппонент еще не закончил
            {
                var bqAnswers = db.QuestionAnswers.
                    Include(qa => qa.QuestionAnswerVariants).
                    Include(qa => qa.QuestionAnswerVariants.Select(qav => qav.QuestionVariant)).
                    Where(qa => qa.AnswerSourceId == 5 && (qa.UserId == battleQueue.FirstUserId || qa.UserId == battleQueue.SecondUserId)).ToList();

                var firstUserMark = 0.0; var secondUserMark = 0.0;

                foreach (var q in battleQueue.BattleQueuesToQuestions)
                {
                    var firstUserAnsw = bqAnswers.OrderByDescending(bqa => bqa.Id).FirstOrDefault(bqa => bqa.UserId == battleQueue.FirstUserId && bqa.QuestionId == q.QuestionId);
                    firstUserMark += firstUserAnsw.QuestionAnswerVariants.Count() == 0 ?
                        0 :
                        (double)firstUserAnsw.
                            QuestionAnswerVariants.Where(qav => qav.QuestionVariant.IsCorrect == true).Count() /
                         (double)firstUserAnsw.QuestionAnswerVariants.Count();

                    var secondUserAnsw = bqAnswers.OrderByDescending(bqa => bqa.Id).FirstOrDefault(bqa => bqa.UserId == battleQueue.SecondUserId && bqa.QuestionId == q.QuestionId);
                    secondUserMark += secondUserAnsw.QuestionAnswerVariants.Count() == 0 ?
                        0 :
                        (double)secondUserAnsw.
                            QuestionAnswerVariants.Where(qav => qav.QuestionVariant.IsCorrect == true).Count() /
                         (double)secondUserAnsw.QuestionAnswerVariants.Count();
                }

                if (firstUserMark == secondUserMark)
                {
                    battleQueue.BattleResultStatusId = 4; // ничья
                }
                else
                {
                    battleQueue.BattleResultStatusId = 3; // победа одного
                    battleQueue.WinnerId = firstUserMark > secondUserMark ? battleQueue.FirstUserId : battleQueue.SecondUserId;

                    //зачисление монеток на баланс
                    var coin = db.UserCoins.FirstOrDefault(uc => uc.UserId == battleQueue.WinnerId);
                    if (coin == null)
                    {
                        db.UserCoins.Add(new UserCoins { UserId = battleQueue.WinnerId, Balance = 1 });
                    }
                    else
                    {
                        coin.Balance += 1;
                    }
                }
            }

            db.SaveChanges();
            await _hubContext.Clients.Group(opponentUserName).SendAsync("NotifyAboutFinishing");
        }

        [HubMethodName("SayAboutQuitting")]
        public async Task SayAboutQuitting(string opponentUserName)
        {
            var httpContext = Context.GetHttpContext();
            var userName = httpContext.Request.Query["userName"];

            var battleQueue = db.BattleQueues.OrderByDescending(bq => bq.Id).
                Include(bq => bq.FirstUser).
                Include(bq => bq.SecondUser).
                FirstOrDefault(bq => (bq.FirstUser.UserName == userName || bq.SecondUser.UserName == userName) && (bq.BattleResultStatusId == 1 || bq.BattleResultStatusId == 2));

            battleQueue.BattleResultStatusId = 3;
            battleQueue.WinnerId = battleQueue.FirstUser.UserName == userName ? battleQueue.SecondUserId : battleQueue.FirstUserId;

            //зачисление монеток на баланс
            var coin = db.UserCoins.FirstOrDefault(uc => uc.UserId == battleQueue.WinnerId);
            if (coin == null)
            {
                db.UserCoins.Add(new UserCoins { UserId = battleQueue.WinnerId, Balance = 1 });
            }
            else
            {
                coin.Balance += 1;
            }

            db.SaveChanges();
            await _hubContext.Clients.Group(opponentUserName).SendAsync("NotifyAboutOpponentQuitting", userName);
        }

        [HubMethodName("SayAboutDeclining")]
        public async Task SayAboutDeclining(string opponentUserName)
        {
            var httpContext = Context.GetHttpContext();
            var userName = httpContext.Request.Query["userName"];

            var battleQueue = db.BattleQueues.OrderByDescending(bq => bq.Id).
                Include(bq => bq.FirstUser).
                Include(bq => bq.SecondUser).
                FirstOrDefault(bq => (bq.FirstUser.UserName == userName || bq.SecondUser.UserName == userName));

            battleQueue.BattleResultStatusId = 5;
            db.SaveChanges();

            await _hubContext.Clients.Group(opponentUserName).SendAsync("NotifyAboutOpponentDeclining");
        }


        public static async Task NotifyAboutAnswer(string userName, int mark)
        {
            await _hubContext.Clients.Group(userName).SendAsync("NotifyAboutAnswer", mark);
        }

        [HubMethodName("SayAboutFriendBattleAccepting")]
        public async Task SayAboutFriendBattleAccepting(string opponentUserName)
        {
            var httpContext = Context.GetHttpContext();
            var userName = httpContext.Request.Query["userName"];

            var battleQueue = db.BattleQueues.OrderByDescending(bq => bq.Id).
                Include(bq => bq.FirstUser).
                Include(bq => bq.SecondUser).
                FirstOrDefault(bq => (bq.FirstUser.UserName == userName || bq.SecondUser.UserName == userName));

            var user = db.ApplicationUsers.Where(u => u.UserName == userName).Select(u => new CustomerShortInfoViewModel
            {
                UserId = u.Id,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                AvatarUrl = u.AvatarUrl,
                BattleId = battleQueue.Id
            }).FirstOrDefault();

            await _hubContext.Clients.Group(opponentUserName).SendAsync("NotifyAboutFriendBattleAccepting", JsonConvert.SerializeObject(user));
        }


        [HubMethodName("SayAboutFriendBattleDeclining")]
        public async Task SayAboutFriendBattleDeclining(string opponentUserName)
        {
            var httpContext = Context.GetHttpContext();
            var userName = httpContext.Request.Query["userName"];

            var battleQueue = db.BattleQueues.OrderByDescending(bq => bq.Id).
                Include(bq => bq.FirstUser).
                Include(bq => bq.SecondUser).
                FirstOrDefault(bq => (bq.FirstUser.UserName == userName || bq.SecondUser.UserName == userName));

            var user = db.ApplicationUsers.Where(u => u.UserName == userName).Select(u => new CustomerShortInfoViewModel
            {
                UserId = u.Id,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                AvatarUrl = u.AvatarUrl,
                BattleId = battleQueue.Id
            }).FirstOrDefault();

            battleQueue.BattleResultStatusId = 5;
            db.SaveChanges();

            await _hubContext.Clients.Group(opponentUserName).SendAsync("NotifyAboutFriendBattleDeclining", JsonConvert.SerializeObject(user));
        }
    }
}
