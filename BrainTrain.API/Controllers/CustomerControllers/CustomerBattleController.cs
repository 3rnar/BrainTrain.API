using BrainTrain.Core.Models;
using BrainTrain.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [Route("api/Customer/Battles")]
    public class CustomerBattleController : BaseApiController
    {
        public CustomerBattleController(BrainTrainContext _db) : base(_db)
        {
        }

        [HttpGet]
        [Route("RequestCompetitor")]
        public async Task<BattleQueue> RequestCompetitor(int subjectId)
        {
            var userId = UserId;

            //var UserId = new SqlParameter("@UserId", userId);
            //var SubjectId = new SqlParameter("@SubjectId", subjectId);
            //var AnswerSourceId = new SqlParameter("@AnswerSourceId", 2);
            //var themes = db.Database.SqlQuery<CustomerThemeKnowledgeLevelViewModel>("GetThemesKnowledgeLevels @UserId, @SubjectId, @AnswerSourceId", UserId, SubjectId, AnswerSourceId).ToList();

            using (var transaction = db.Database.BeginTransaction())
            {
                var secondUser = db.ApplicationUsers.SingleOrDefault(x => x.Id == userId);
                var existingBQs = await db.BattleQueues.Include(bq => bq.FirstUser).Include(bq => bq.FirstUser).Where(bq => string.IsNullOrEmpty(bq.SecondUserId) &&
                bq.SubjectId == subjectId &&
                EF.Functions.DateDiffDay(bq.DateCreated, DateTime.Now) <= 20 &&
                bq.FirstUserId != userId).OrderByDescending(bq => bq.Id).ToListAsync();

                var existingBQ = existingBQs.FirstOrDefault(bq => StatsHandler.ConnectedIds.Any(c => c.UserName == bq.FirstUser.UserName));

                if (existingBQ == null)
                {
                    existingBQ = await db.BattleQueues.FirstOrDefaultAsync(bq => string.IsNullOrEmpty(bq.SecondUserId) &&
                    bq.SubjectId == subjectId &&
                    EF.Functions.DateDiffDay(bq.DateCreated, DateTime.Now) <= 20 &&
                    bq.FirstUserId == userId);

                    if (existingBQ != null)
                        return existingBQ;

                    var newBq = new BattleQueue { FirstUserId = userId, SubjectId = subjectId, DateCreated = DateTime.Now };
                    db.BattleQueues.Add(newBq);
                    await db.SaveChangesAsync();
                    transaction.Commit();
                    return newBq;
                }
                else
                {
                    existingBQ.SecondUserId = userId;
                    var firstUser = db.ApplicationUsers.SingleOrDefault(x => x.Id == existingBQ.FirstUserId);

                    var newBq = new BattleQueue
                    {
                        FirstUserId = existingBQ.FirstUserId,
                        SecondUserId = existingBQ.SecondUserId,
                        DateCreated = existingBQ.DateCreated,
                        SubjectId = existingBQ.SubjectId,
                        FirstUser = new ApplicationUser { Id = existingBQ.FirstUserId, UserName = firstUser.UserName, AvatarUrl = firstUser.AvatarUrl, FirstName = firstUser.FirstName, LastName = firstUser.LastName }
                    };


                    var questions = db.Questions.Where(q => q.QuestionsToThemes.Any(qtt => qtt.Theme.SubjectId == subjectId) && (q.QuestionTypeId != 7)).OrderBy(q => Guid.NewGuid()).Take(10).ToList();

                    existingBQ.BattleQueuesToQuestions = new List<BattleQueuesToQuestions>();
                    foreach (var q in questions)
                    {
                        existingBQ.BattleQueuesToQuestions.Add(new BattleQueuesToQuestions { QuestionId = q.Id });
                    }

                    await db.SaveChangesAsync();
                    transaction.Commit();

                    BattleHub.NotifyFirstUser(firstUser.UserName, JsonConvert.SerializeObject(new CustomerShortInfoViewModel
                    {
                        UserId = secondUser.Id,
                        AvatarUrl = secondUser.AvatarUrl,
                        FirstName = secondUser.FirstName,
                        LastName = secondUser.LastName,
                        UserName = secondUser.UserName
                    }));


                    return newBq;
                }
            }
        }

        [HttpGet]
        [Route("RequestExactCompetitor")]
        public async Task<BattleQueue> RequestExactCompetitor(int subjectId, string opponentId)
        {
            var userId = UserId;

            var requestor = db.ApplicationUsers.SingleOrDefault(x => x.Id == userId);  
            var opponent = db.ApplicationUsers.SingleOrDefault(x => x.Id == opponentId);

            var newBq = new BattleQueue
            {
                FirstUserId = userId,
                SecondUserId = opponentId,
                SubjectId = subjectId,
                DateCreated = DateTime.Now,
                BattleQueuesToQuestions = new List<BattleQueuesToQuestions>()
            };
            var questions = db.Questions.Where(q => q.QuestionsToThemes.Any(qtt => qtt.Theme.SubjectId == subjectId) && (q.QuestionTypeId == 1 || q.QuestionTypeId == 2)).OrderBy(q => Guid.NewGuid()).Take(10).ToList();

            foreach (var q in questions)
            {
                newBq.BattleQueuesToQuestions.Add(new BattleQueuesToQuestions { QuestionId = q.Id });
            }

            db.BattleQueues.Add(newBq);
            await db.SaveChangesAsync();

            BattleHub.NotifyFriendBattleOpponent(opponent.UserName, JsonConvert.SerializeObject(new CustomerShortInfoViewModel
            {
                UserId = requestor.Id,
                AvatarUrl = requestor.AvatarUrl,
                FirstName = requestor.FirstName,
                LastName = requestor.LastName,
                UserName = requestor.UserName,
                BattleId = newBq.Id
            }));

            return newBq;

        }

        [HttpGet]
        [Route("BattleQuestions")]
        public IEnumerable<int> BattleQuestions(int battleId)
        {
            var battle = db.BattleQueues.FirstOrDefault(bq => bq.Id == battleId);
            battle.BattleResultStatusId = 1;

            var qs = db.Questions.Where(q => q.BattleQueuesToQuestions.Any(bqtq => bqtq.BattleQueueId == battleId)).Select(q => q.Id).ToList();

            db.SaveChanges();

            return qs;
        }

        [HttpPost]
        [Route("SendBattleQuestionAnswer")]
        public async Task<IActionResult> PostBattleQuestionAnswersAsync(QuestionAnswer model, int battleId)
        {
            var userId = UserId;
            var dt = DateTime.Now;

            var question = await db.Questions.Include(q => q.QuestionVariants).Include(q => q.QuestionDifficulty).FirstOrDefaultAsync(q => q.Id == model.QuestionId);
            if (question == null)
                return NotFound();

            var qa = new QuestionAnswer
            {
                QuestionId = model.QuestionId,
                UserId = userId,
                Value = model.Value,
                QuestionAnswerVariants = new List<QuestionAnswerVariant>(),
                DateCreated = dt,
                AnswerSourceId = 5,
                SolvingSeconds = model.SolvingSeconds,
                IsCorrect = model.IsCorrect
            };

            if (model.QuestionAnswerVariants != null)
            {
                foreach (var variant in model.QuestionAnswerVariants)
                {
                    qa.QuestionAnswerVariants.Add(new QuestionAnswerVariant { QuestionVariantId = variant.QuestionVariantId });
                }
            }

            db.QuestionAnswers.Add(qa);

            await db.SaveChangesAsync();

            var battleQueue = await db.BattleQueues.Include(bq => bq.FirstUser).Include(bq => bq.SecondUser).FirstOrDefaultAsync(bq => bq.Id == battleId);
            if (battleQueue == null)
                return NotFound();

            //var correctVariants = question.QuestionVariants.Where(qv => qv.IsCorrect == true).ToList();
            //var mark = 0;
            //var userCorrectVariantsNumber = 0;
            //foreach (var qv in correctVariants)
            //{
            //    if (model.QuestionAnswerVariants.Any(qav => qav.QuestionVariantId == qv.Id))
            //    {
            //        userCorrectVariantsNumber++;
            //    }
            //}

            //mark = userCorrectVariantsNumber / correctVariants.Count;

            //var rating = mark * question.QuestionDifficulty.Value;

            if (model.IsCorrect == true)
            {
                CustomerLevelUpdateHandler.UpdateLevel(question.QuestionDifficulty.Value, userId);
            }


            //var ur = await db.UserRatings.FirstOrDefaultAsync(u => u.UserId == userId);
            //if (ur == null)
            //{
            //    db.UserRatings.Add(new UserRatings { UserId = userId, Rating = rating });
            //}
            //else
            //{
            //    ur.Rating += rating;
            //}
            await db.SaveChangesAsync();

            BattleHub.NotifyAboutAnswer(battleQueue.FirstUserId == userId ? battleQueue.SecondUser.UserName : battleQueue.FirstUser.UserName, model.IsCorrect == true ? question.QuestionDifficulty.Value : 0);

            var sqlUserId = new SqlParameter("@UserId", userId);
            var Questions = new SqlParameter("@Questions", model.QuestionId.ToString());
            db.Database.ExecuteSqlRaw("dbo.UpdateThemeOverallLearningRate @Questions, @UserId", Questions, sqlUserId);

            return Ok(question.QuestionDifficulty.Value);
        }

        [HttpGet]
        [Route("BattlesHistory")]
        public async Task<IEnumerable<BattleHistoryViewModel>> GetBattlesHistory()
        {
            var userId = UserId;
            var user = db.ApplicationUsers.SingleOrDefault(x => x.Id == userId);


            var battleQueues = db.BattleQueues.Where(bq => bq.FirstUserId == userId || bq.SecondUserId == userId).Select(bq => new
            {
                id = bq.Id,
                firstUserId = bq.FirstUserId,
                secondUserId = bq.SecondUserId,
                subjectId = bq.SubjectId,
                dateCreated = bq.DateCreated,
                resultStatusId = bq.BattleResultStatusId,
                winnerId = bq.WinnerId,

                firstUser = bq.FirstUser == null ? null : new { id = bq.FirstUser.Id, userName = bq.FirstUser.UserName, avatarUrl = bq.FirstUser.AvatarUrl, firstName = bq.FirstUser.FirstName, lastName = bq.FirstUser.LastName },
                secondUser = bq.SecondUser == null ? null : new { id = bq.SecondUser.Id, userName = bq.SecondUser.UserName, avatarUrl = bq.SecondUser.AvatarUrl, firstName = bq.SecondUser.FirstName, lastName = bq.SecondUser.LastName },
                winner = bq.Winner == null ? null : new { id = bq.Winner.Id, userName = bq.Winner.UserName },
                subject = new { id = bq.Subject.Id, title = bq.Subject.Title },
                resultStatus = bq.BattleResultStatus == null ? null : new { id = bq.BattleResultStatus.Id, title = bq.BattleResultStatus.Title }
            }).AsEnumerable().Select(bq => new BattleQueue
            {
                Id = bq.id,
                FirstUserId = bq.firstUserId,
                SecondUserId = bq.secondUserId,
                SubjectId = bq.subjectId,
                DateCreated = bq.dateCreated,
                BattleResultStatusId = bq.resultStatusId,
                WinnerId = bq.winnerId,

                FirstUser = bq.firstUser == null ? null : new ApplicationUser { Id = bq.firstUser.id, UserName = bq.firstUser.userName, AvatarUrl = bq.firstUser.avatarUrl, FirstName = bq.firstUser.firstName, LastName = bq.firstUser.lastName },
                SecondUser = bq.secondUser == null ? null : new ApplicationUser { Id = bq.secondUser.id, UserName = bq.secondUser.userName, AvatarUrl = bq.secondUser.avatarUrl, FirstName = bq.secondUser.firstName, LastName = bq.secondUser.lastName },
                Winner = bq.winner == null ? null : new ApplicationUser { Id = bq.winner.id, UserName = bq.winner.userName },
                Subject = new Subject { Id = bq.subject.id, Title = bq.subject.title },
                BattleResultStatus = bq.resultStatus == null ? null : new BattleResultStatus { Id = bq.resultStatus.id, Title = bq.resultStatus.title }
            }).ToList();


            return battleQueues.Select(bq => new BattleHistoryViewModel
            {
                BattleId = bq.Id,
                OpponentName = (bq.FirstUserId == userId ? (bq.SecondUser == null ? "" : bq.SecondUser.UserName) : bq.FirstUser.UserName),
                DateCreated = bq.DateCreated,
                BattleResult = bq.BattleResultStatus == null ? "" : bq.BattleResultStatus.Title,
                WinnerName = bq.Winner == null ? "" : bq.Winner.UserName,
                SubjectTitle = bq.Subject.Title,
                OpponentAvatarUrl = (bq.FirstUserId == userId ? (bq.SecondUser == null ? "" : bq.SecondUser.AvatarUrl) : bq.FirstUser.AvatarUrl),
                OpponentFirstName = (bq.FirstUserId == userId ? (bq.SecondUser == null ? "" : bq.SecondUser.FirstName) : bq.FirstUser.FirstName),
                OpponentLastName = (bq.FirstUserId == userId ? (bq.SecondUser == null ? "" : bq.SecondUser.LastName) : bq.FirstUser.LastName)
            }).ToList();
        }

        [HttpGet]
        [Route("BattleDetalization")]
        public async Task<IEnumerable<BattleQuestionViewModel>> GetBattleDetalization(int battleId)
        {
            var qs = new List<BattleQuestionViewModel>();

            var battleQueue = db.BattleQueues.
                Include(bq => bq.FirstUser).
                Include(bq => bq.SecondUser).
                Include(bq => bq.BattleQueuesToQuestions).
                Include(bq => bq.BattleQueuesToQuestions.Select(bqtq => bqtq.Question)).
                Include(bq => bq.BattleQueuesToQuestions.Select(bqtq => bqtq.Question.QuestionVariants)).
                Include(bq => bq.BattleQueuesToQuestions.Select(bqtq => bqtq.Question.Solutions)).
                Include(bq => bq.BattleQueuesToQuestions.Select(bqtq => bqtq.Question.QuestionsToThemes.Select(qtt => qtt.Theme))).
                OrderByDescending(bq => bq.Id).
                FirstOrDefault(bq => bq.Id == battleId);

            var qas = db.QuestionAnswers.
                Include(qa => qa.QuestionAnswerVariants).
                Include(qa => qa.QuestionAnswerVariants.Select(qav => qav.QuestionVariant)).
                Where(qa => qa.Question.BattleQueuesToQuestions.Any(b => b.BattleQueueId == battleId)).ToList();

            if (battleQueue.BattleQueuesToQuestions != null)
            {
                foreach (var bqtq in battleQueue.BattleQueuesToQuestions)
                {
                    var model = new BattleQuestionViewModel
                    {
                        Id = bqtq.QuestionId,
                        Text = bqtq.Question.Text,
                        Variants = bqtq.Question.QuestionVariants.Select(qv => new QuestionVariant { Id = qv.Id, Text = qv.Text, IsCorrect = qv.IsCorrect }).ToList(),
                        Solution = bqtq.Question.Solutions.Select(s => new Solution { Id = s.Id, Text = s.Text }).FirstOrDefault(),
                        ThemeId = bqtq.Question.QuestionsToThemes.FirstOrDefault().ThemeId,
                        ThemeTitle = bqtq.Question.QuestionsToThemes.FirstOrDefault().Theme.Title
                    };

                    var firstUserAnsw = qas.OrderByDescending(bqa => bqa.Id).FirstOrDefault(bqa => bqa.UserId == battleQueue.FirstUserId && bqa.QuestionId == bqtq.QuestionId);
                    var firstUserMark = (firstUserAnsw == null || firstUserAnsw.QuestionAnswerVariants == null || firstUserAnsw.QuestionAnswerVariants.Count() == 0) ?
                        0 :
                        ((double)firstUserAnsw.
                            QuestionAnswerVariants.Where(qav => qav.QuestionVariant.IsCorrect == true).Count() /
                         (double)firstUserAnsw.QuestionAnswerVariants.Count());

                    var secondUserAnsw = qas.OrderByDescending(bqa => bqa.Id).FirstOrDefault(bqa => bqa.UserId == battleQueue.SecondUserId && bqa.QuestionId == bqtq.QuestionId);
                    var secondUserMark = (secondUserAnsw == null || secondUserAnsw.QuestionAnswerVariants == null || secondUserAnsw.QuestionAnswerVariants.Count() == 0) ?
                        0 :
                        ((double)secondUserAnsw.
                            QuestionAnswerVariants.Where(qav => qav.QuestionVariant.IsCorrect == true).Count() /
                         (double)secondUserAnsw.QuestionAnswerVariants.Count());

                    if (firstUserAnsw != null)
                    {
                        model.FirstUserMark = firstUserMark;
                        model.FirstUserAnswer = new QuestionAnswer
                        {
                            Id = firstUserAnsw.Id,
                            QuestionId = firstUserAnsw.QuestionId,
                            UserId = firstUserAnsw.UserId,
                            Value = firstUserAnsw.Value,
                            QuestionAnswerVariants =
                            firstUserAnsw.QuestionAnswerVariants.Select(qav => new QuestionAnswerVariant
                            {
                                QuestionAnswerId = qav.QuestionAnswerId,
                                QuestionVariantId = qav.QuestionVariantId
                            }).ToList()
                        };
                    }

                    if (secondUserAnsw != null)
                    {
                        model.SecondUserMark = secondUserMark;
                        model.SecondUserAnswer = new QuestionAnswer
                        {
                            Id = secondUserAnsw.Id,
                            QuestionId = secondUserAnsw.QuestionId,
                            UserId = secondUserAnsw.UserId,
                            Value = secondUserAnsw.Value,
                            QuestionAnswerVariants =
                            secondUserAnsw.QuestionAnswerVariants.Select(qav => new QuestionAnswerVariant
                            {
                                QuestionAnswerId = qav.QuestionAnswerId,
                                QuestionVariantId = qav.QuestionVariantId
                            }).ToList()
                        };
                    }

                    qs.Add(model);
                }
            }

            return qs;

        }
    }
}
