using BrainTrain.API.Helpers.Learnosity;
using BrainTrain.API.Models;
using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [RoutePrefix("api/Customer/Midterm")]
    public class CustomerMidtermController : BaseApiController
    {
        [HttpGet]
        [Route("GetTeachers")]
        public async Task<IActionResult> GetTeachers()
        {
            return Ok(await db.Midterm_Teachers.OrderBy(t => t.Title).ToListAsync());
        }

        [HttpGet]
        [Route("GetLanguages")]
        public async Task<IActionResult> GetLanguages()
        {
            return Ok(await db.Midterm_Languages.ToListAsync());
        }

        [HttpPost]
        [Route("PostNewUser")]
        public async Task<IActionResult> PostNewUser(Midterm_User user, int eventId)
        {
            var existingUser = await db.Midterm_Users.Include(u => u.Midterm_UserEvents).FirstOrDefaultAsync(u => u.SystemId == user.SystemId);
            if (existingUser != null)
            {
                if (StatsHandler.ExamConnectedIds.Any(a => a.UserName == existingUser.Id.ToString()))
                {
                    return BadRequest("Пользователь уже залогинен");
                }

                if (!existingUser.Midterm_UserEvents.Any(e => e.EventId == eventId))
                {
                    var midtermUserEventnew = new Midterm_UserEvent
                    {
                        UserId = existingUser.Id,
                        EventId = eventId,
                        Midterm_UserEventQuestions = (await ChooseQuestionsAsync(eventId, user.LanguageId)).Select(l => new Midterm_UserEventQuestion
                        {
                            QuestionId = l
                        }).ToList(),
                        DateStart = DateTime.Now,
                        StatusId = 1,
                        IsPrinted = false
                    };

                    db.Midterm_UserEvents.Add(midtermUserEventnew);

                    await db.SaveChangesAsync();

                    return Ok(new CustomerMidtermAuthorizedUserModel { Id = existingUser.Id, SecondsSpent = null, StatusId = 1, FullName = existingUser.FullName });
                }
                else
                {
                    return Ok(new CustomerMidtermAuthorizedUserModel
                    {
                        Id = existingUser.Id,
                        SecondsSpent = existingUser.Midterm_UserEvents.OrderByDescending(a => a.Id).FirstOrDefault(e => e.EventId == eventId)?.SecondsSpent,
                        FullName = existingUser.FullName,
                        StatusId = existingUser.Midterm_UserEvents.OrderByDescending(a => a.Id).FirstOrDefault(e => e.EventId == eventId)?.StatusId,
                        IsPrinted = existingUser.Midterm_UserEvents.OrderByDescending(a => a.Id).FirstOrDefault(e => e.EventId == eventId)?.IsPrinted
                    });
                }
            }

            var kaznituUser = db.KaznituUsers.FirstOrDefault(k => k.IIN == user.SystemId);
            if (kaznituUser != null)
            {
                user.FullName = kaznituUser.LastName + ' ' + kaznituUser.FirstName + ' ' + kaznituUser.MiddleName;
            }
            if (kaznituUser == null)
            {
                var functionName = "FN_CheckIIN";
                if (eventId == 2 || eventId == 3)
                {
                    functionName = "FN_CheckIIN_Student";
                }
                var parameters = new List<SqlParameter> {
                    new SqlParameter("@IIN", user.SystemId),
                };
                var response = db.Database.SqlQuery<string>($"select dbo.{functionName} (@IIN)", parameters.ToArray()).FirstOrDefault();

                if (string.IsNullOrEmpty(response))
                {
                    return BadRequest("Пользователь не существует");
                }

                user.FullName = response;
            }
            

            user.DateCreated = DateTime.Now;
            

            var midtermUserEvent = new Midterm_UserEvent {
                Midterm_User = user,
                EventId = eventId,
                Midterm_UserEventQuestions = (await ChooseQuestionsAsync(eventId, user.LanguageId)).Select(l => new Midterm_UserEventQuestion
                {
                    QuestionId = l
                }).ToList(),
                DateStart = DateTime.Now,
                StatusId = 1,
                IsPrinted = false
            };

            db.Midterm_UserEvents.Add(midtermUserEvent);

            await db.SaveChangesAsync();

            return Ok(new CustomerMidtermAuthorizedUserModel { Id = user.Id, SecondsSpent = null, StatusId = 1, FullName = user.FullName });
        }

        private async Task<List<int>> ChooseQuestionsAsync(int eventId, int languageId)
        {
            var list = new List<Midterm_Question>();

            switch (eventId)
            {
                default:
                case 1:
                    list.AddRange(await ChooseMaths(eventId, languageId));
                    list.AddRange(await ChoosePhysics(eventId, languageId));
                    break;
                case 2:
                    list.AddRange(await ChoosePhysicsIndividual(eventId, languageId));
                    break;
                case 3:
                    list.AddRange(await ChooseMaths(eventId, languageId));
                    break;
            }

            return list.Select(l => l.Id).ToList();
        }

        private async Task<List<Midterm_Question>> ChooseMaths(int eventId, int languageId)
        {
            var list = new List<Midterm_Question>();
            //maths
            var allMathQuestions = await db.Midterm_Questions.Where(q => /*q.Midterm_Variant.EventId == eventId && */ q.LanguageId == languageId && q.SubjectId == 1 && q.IsActive != false).ToListAsync();
            var mathGrouping = allMathQuestions.GroupBy(q => q.Order).ToList();

            foreach (var gr in mathGrouping)
            {
                list.Add(gr.OrderBy(g => Guid.NewGuid()).FirstOrDefault());
            }

            return list;
        }

        private async Task<List<Midterm_Question>> ChoosePhysics(int eventId, int languageId)
        {
            var list = new List<Midterm_Question>();

            //physics
            var allPhysQuestions = await db.Midterm_Questions.Where(q => /*q.Midterm_Variant.EventId == eventId && */q.LanguageId == languageId && q.SubjectId == 2 && q.IsActive != false).ToListAsync();
            var physGrouping = allPhysQuestions.GroupBy(q => q.VariantId).ToList();
            foreach (var gr in physGrouping.ToList())
            {
                list.AddRange(gr.OrderBy(g => Guid.NewGuid()).Take(5).ToList());
            }

            return list;
        }

        private async Task<List<Midterm_Question>> ChoosePhysicsIndividual(int eventId, int languageId)
        {
            var list = new List<Midterm_Question>();

            //physics
            var allPhysQuestions = await db.Midterm_Questions.Where(q => /*q.Midterm_Variant.EventId == eventId && */q.LanguageId == languageId && q.SubjectId == 2 && q.IsActive != false).ToListAsync();
            var physGrouping = allPhysQuestions.GroupBy(q => q.VariantId).ToList();
            foreach (var gr in physGrouping.Where(p => p.Key == 8 || p.Key == 9).ToList())
            {
                list.AddRange(gr.OrderBy(g => Guid.NewGuid()).Take(10).ToList());
            }

            return list;
        }

        [HttpGet]
        [Route("GetQuestions")]
        public async Task<IActionResult> GetQuestions(int userId, int eventId, int subjectId)
        {
            var list = new List<Midterm_Question>();

            var userEvent = await db.Midterm_UserEvents.
                Include(a => a.Midterm_UserEventQuestions).
                Include(a => a.Midterm_UserEventQuestions.Select(e => e.Midterm_Question)).
                OrderByDescending(a => a.Id).
                FirstOrDefaultAsync(a => a.UserId == userId && a.EventId == eventId);
            
            if (userEvent == null)
            {
                BadRequest("Необходимо повторно авторизоваться");
            }
            else
            {
                if (userEvent.StatusId == 4)
                {
                    return BadRequest("Пользователь уже сдал экзамен");
                }

                list = userEvent.Midterm_UserEventQuestions.Where(a => a.Midterm_Question.SubjectId == subjectId).Select(a => a.Midterm_Question).ToList();
                if (subjectId == 1)
                {
                    userEvent.StatusId = 2;
                }
                if (subjectId == 2)
                {
                    userEvent.StatusId = 3;
                }

                await db.SaveChangesAsync();
            }

            var uuid = "";
            var json = LRNQuestionsHelper.Simple(list.OrderBy(l => l.Order).ToList(), out uuid);

            return Ok(json);
        }

        [HttpPost]
        [Route("PostUserAnswers")]
        public async Task<IActionResult> PostUserAnswers(List<CustomerMidtermQuestionAnswerViewModel> answers, int userId, int eventId, int subjectId)
        {
            var userEvent = await db.Midterm_UserEvents.
                Include(a => a.Midterm_UserEventQuestions).
                Include(a => a.Midterm_UserEventQuestions.Select(e => e.Midterm_Question)).
                OrderByDescending(a => a.Id).
                FirstOrDefaultAsync(a => a.UserId == userId && a.EventId == eventId);

            if (userEvent == null)
            {
                return BadRequest("Event and user not found");
            }

            var totalValue = 0.0;

            foreach (var answ in answers)
            {
                var questionToEvent = userEvent.Midterm_UserEventQuestions.FirstOrDefault(q => q.QuestionId == answ.QuestionId);
                if (questionToEvent == null)
                {
                    return BadRequest("Question not found");
                }

                questionToEvent.IsCorrect = answ.IsCorrect;
                questionToEvent.Answer = answ.Answer;
                if (answ.IsCorrect)
                {
                    totalValue += questionToEvent.Midterm_Question.Weight;
                }
            }

            if (userEvent.FinalResult != null)
            {
                userEvent.FinalResult += totalValue;
            }
            else
            {
                userEvent.FinalResult = totalValue;
            }
            
            
            switch (eventId)
            {
                case 1:
                    if (subjectId == 2)
                    {
                        userEvent.StatusId = 4;
                        userEvent.DateFinish = DateTime.Now;
                    }                    
                    break;
                case 2:
                    userEvent.StatusId = 4;
                    break;
                case 3:
                    userEvent.StatusId = 4;
                    break;
            }


            await db.SaveChangesAsync();

            return Ok(totalValue);
        }


        [HttpGet]
        [Route("GetUserAnswersReview")]
        public async Task<IActionResult> GetUserAnswersReview(int userId, int eventId)
        {
            var userEvent = await db.Midterm_UserEvents.
                Include(a => a.Midterm_UserEventQuestions).
                Include(a => a.Midterm_UserEventQuestions.Select(e => e.Midterm_Question)).
                OrderByDescending(a => a.Id).
                FirstOrDefaultAsync(a => a.UserId == userId && a.EventId == eventId);

            if (userEvent == null)
            {
                return BadRequest("Event and user not found");
            }

            var list = userEvent.Midterm_UserEventQuestions.Select(q => new CustomerMidtermReviewQuestionViewModel
            {
                Id = q.QuestionId,
                LrnJson = q.Midterm_Question.LrnJson,
                Order = q.Midterm_Question.Order,
                VariantId = q.Midterm_Question.VariantId,
                Weight = q.Midterm_Question.Weight,
                LanguageId = q.Midterm_Question.LanguageId,
                SubjectId = q.Midterm_Question.SubjectId,
                IsActive = q.Midterm_Question.IsActive,
                UserAnswer = q.Answer,
                IsUserAnswerCorrect = q.IsCorrect,
                StatusId = q.StatusId
            }).ToList();

            var uuid = "";
            var json = LRNQuestionsHelper.Simple(list.OrderBy(l => l.Order).ToList(), out uuid, true);

            return Ok(json);
        }

        [HttpGet]
        [Route("GetAllUsersStats")]
        public async Task<IActionResult> GetAllUsersStats(int eventId, int languageId, DateTime? date = null)
        {
            if (date == null)
            {
                date = new DateTime(2020, 1, 1);
            }
            var users = await db.Midterm_Users.                
                Include(u => u.Midterm_UserEvents).
                Include(u => u.Midterm_UserEvents.Select(e => e.Midterm_UserEventQuestions)).
                Include(u => u.Midterm_UserEvents.Select(e => e.Midterm_UserEventQuestions.Select(q => q.Midterm_Question))).
                Where(u => u.Midterm_UserEvents.Any(e => e.EventId == eventId) && u.LanguageId == languageId && u.Midterm_UserEvents.Any(e => e.DateStart >= date)).
                ToListAsync();

            var list = new List<CustomerMidtermUserModel>();

            foreach (var u in users)
            {
                var item = new CustomerMidtermUserModel
                {
                    Id = u.Id,
                    Name = u.FullName,
                    IIN = u.SystemId,
                    NumberOfComplaints = u.Midterm_UserEvents.FirstOrDefault(e => e.EventId == eventId)?.Midterm_UserEventQuestions.Where(q => q.StatusId == 1).Count(),
                    Maths = u.Midterm_UserEvents.FirstOrDefault(e => e.EventId == eventId)?.Midterm_UserEventQuestions.Where(q => q.Midterm_Question?.SubjectId == 1 && q.IsCorrect == true).Sum(q => q.Midterm_Question?.Weight),
                    Physics = u.Midterm_UserEvents.FirstOrDefault(e => e.EventId == eventId)?.Midterm_UserEventQuestions.Where(q => q.Midterm_Question?.SubjectId == 2 && q.IsCorrect == true).Sum(q => q.Midterm_Question?.Weight),
                    LanguageId = u.LanguageId,
                    DateStart = u.Midterm_UserEvents.FirstOrDefault(e => e.EventId == eventId)?.DateStart,
                    DateFinish = u.Midterm_UserEvents.FirstOrDefault(e => e.EventId == eventId)?.DateFinish,
                    IsPrinted = u.Midterm_UserEvents.FirstOrDefault(e => e.EventId == eventId)?.IsPrinted
                };

                list.Add(item);
            }

            return Ok(list);
        }

        [HttpGet]
        [Route("RegisterComplaint")]
        public async Task<IActionResult> RegisterComplaint(int userId, int questionId, int eventId = 1)
        {
            var question = await db.Midterm_UserEventQuestions.Include(q => q.Midterm_UserEvent).FirstOrDefaultAsync(e => e.QuestionId == questionId && e.Midterm_UserEvent.UserId == userId && e.Midterm_UserEvent.EventId == eventId);
            if (question == null)
            {
                return BadRequest("Вопрос не найден");
            }
            if (question.Midterm_UserEvent?.IsPrinted == true)
            {
                return BadRequest("Ваш результат уже окончательный");
            }

            question.StatusId = 1;

            await db.SaveChangesAsync();

            return Ok();
        }


        [HttpGet]
        [Route("AcceptComplaint")]
        public async Task<IActionResult> AcceptComplaint(int userId, int questionId, int eventId = 1)
        {
            var question = await db.Midterm_UserEventQuestions.Include(q => q.Midterm_UserEvent).FirstOrDefaultAsync(e => e.QuestionId == questionId && e.Midterm_UserEvent.UserId == userId && e.Midterm_UserEvent.EventId == eventId);
            if (question == null)
            {
                return BadRequest("Вопрос не найден");
            }

            question.StatusId = 2;
            question.IsCorrect = true;

            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("DeclineComplaint")]
        public async Task<IActionResult> DeclineComplaint(int userId, int questionId, int eventId = 1)
        {
            var question = await db.Midterm_UserEventQuestions.Include(q => q.Midterm_UserEvent).FirstOrDefaultAsync(e => e.QuestionId == questionId && e.Midterm_UserEvent.UserId == userId && e.Midterm_UserEvent.EventId == eventId);
            if (question == null)
            {
                return BadRequest("Вопрос не найден");
            }

            question.StatusId = 3;

            await db.SaveChangesAsync();

            return Ok();
        }



        [HttpGet]
        [Route("GetAllQuestions")]
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await db.Midterm_Questions.OrderBy(q => q.VariantId).ThenBy(q => q.Order).ToListAsync();

            var uuid = "";
            var json = LRNQuestionsHelper.Simple(questions, out uuid);

            return Ok(json);
        }

        [HttpPost]
        [Route("RegisterMidtermComplaint")]
        public async Task<IActionResult> RegisterMidtermComplaint(Midterm_UserComplaint complaint)
        {
            complaint.DateCreated = DateTime.Now;
            db.Midterm_UserComplaints.Add(complaint);
            await db.SaveChangesAsync();
            return Ok(complaint.Id);
        }
    }
}
