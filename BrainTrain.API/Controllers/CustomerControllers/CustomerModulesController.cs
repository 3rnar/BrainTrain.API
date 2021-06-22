using BrainTrain.API.Dapper;
using BrainTrain.API.Helpers;
using BrainTrain.Core.Models;
using BrainTrain.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [Route("api/Customer/Modules")]
    public class CustomerModulesController : BaseApiController
    {
        private readonly IConfiguration Configuration;
        public CustomerModulesController(BrainTrainContext _db, IConfiguration configuration) : base(_db)
        {
            Configuration = configuration;
        }

        [HttpGet]
        [Route("UserModulesTiny")]
        public async Task<CustomerModulesAndControlWorksViewModel> GetModulesTiny(int subjectId)
        {
            var modules = new StoredProcedure<SqlServer, CustomerModulesTinyViewModel>("GetUserModulesTiny").ExecResult(
                new FunctionParameter("@UserId", UserId),
                new FunctionParameter("@SubjectId", subjectId)
                );


            //var usersToControlWorks = db.UsersToControlWorks.Where(u => u.UserId == userId).ToList();
            var userToSubject = db.UsersToSubjects.Include(uts => uts.Subject).FirstOrDefault(uts => uts.SubjectId == subjectId);
            double subjectKnowingBorder = ((double) userToSubject.DesiredScore / userToSubject.Subject.MaximumScore * 100) ?? 0;

            var controlWorks = db.ControlWorks.OrderBy(cw => cw.Id).Include(cw => cw.UsersToControlWorks).
                Where(c => c.SubjectId == subjectId && c.UserId == UserId).
                Select(c => new CustomerControlWorkViewModel {
                ControlWorkId = c.Id,
                ControlWorkTypeId = c.TypeId,
                ControlWorkTitle = c.Title,
                ModuleIds = c.ControlWorksToModules.Select(cw => cw.ModuleId).ToList(),
                KnowingPercentage = c.UsersToControlWorks.Any(utc => utc.ControlWorkId == c.Id && utc.UserId == UserId) ? 
                    c.UsersToControlWorks.OrderByDescending(utc => utc.Id).FirstOrDefault(utc => utc.ControlWorkId == c.Id && utc.UserId == UserId).CurrentLearningRate : 0,
                IsAttemptCompleted = c.UsersToControlWorks.Any(utc => utc.ControlWorkId == c.Id && utc.UserId == UserId) ?
                    c.UsersToControlWorks.OrderByDescending(utc => utc.Id).FirstOrDefault(utc => utc.ControlWorkId == c.Id && utc.UserId == UserId).IsCompleted : false,
                BestResult = c.UsersToControlWorks.Any(utc => utc.ControlWorkId == c.Id && utc.UserId == UserId) ?
                    c.UsersToControlWorks.OrderByDescending(utc => utc.CurrentLearningRate).FirstOrDefault(utc => utc.ControlWorkId == c.Id && utc.UserId == UserId).CurrentLearningRate : 0,
                NumberOfAttempts = c.UsersToControlWorks.Count(utc => utc.ControlWorkId == c.Id && utc.UserId == UserId)
            }).ToList();

            foreach (var cw in controlWorks)
            {
                cw.IsKnown = cw.BestResult >= subjectKnowingBorder;
                var lastModules = modules.Where(m => cw.ModuleIds.Contains(m.ModuleId)).ToList();
                cw.Deadline = lastModules.OrderByDescending(l => l.ModuleDeadline).ToList()[0].ModuleDeadline.AddDays(1);
                cw.GoalResult = subjectKnowingBorder;
            }

            var model = new CustomerModulesAndControlWorksViewModel
            {
                Modules = modules,
                ControlWorks = controlWorks
            };

            return model;
        }

        [HttpGet]
        [Route("UpdateDeadlines")]
        public async Task<IActionResult> UpdateModuleDeadlines(int subjectId)
        {
            new StoredProcedure<SqlServer, object>("UpdateNotLearnedModuleDeadlines").Exec(
                new FunctionParameter("@UserId", UserId),
                new FunctionParameter("@SubjectId", subjectId)
                );

            return Ok();
        }

        [HttpGet]
        [Route("AddUserModule")]
        public async Task<IActionResult> AddUserModule(int moduleId)
        {
            var userId = UserId;

            if (!await db.UsersToModules.AnyAsync(utm => utm.ModuleId == moduleId && utm.UserId == userId))
            {
                db.UsersToModules.Add(new UsersToModules { UserId = userId, ModuleId = moduleId });
                await db.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost]
        [Route("PostCheckingTestAnswersSity")]
        public async Task<IActionResult> PostQuestionAnswersLearnosity(QuestionAnswer model, int themeId, int moduleId, int attemptId)
        {
            var userId = UserId;
            var dt = DateTime.Now;

            var attempt = await db.ModuleThemePassAttempts.
                Include(m => m.QuestionAnswers).
                Include(m => m.AttemptsToQuestions).
                Include(m => m.AttemptsToQuestions.Select(atq => atq.Question)).
                FirstOrDefaultAsync(m => m.Id == attemptId);
            if (attempt == null)
            {
                return NotFound();
            }

            var questionIds = attempt.AttemptsToQuestions.OrderBy(a => a.QuestionId).ThenBy(a => a.Question.QuestionDifficultyId).Select(a => a.QuestionId).ToList();

            if (questionIds.IndexOf(model.QuestionId) == questionIds.Count() - 1)
                attempt.IsFinished = true;

            var qa = new QuestionAnswer
            {
                QuestionId = model.QuestionId,
                UserId = userId,
                Value = model.Value,
                QuestionAnswerVariants = new List<QuestionAnswerVariant>(),
                DateCreated = dt,
                AnswerSourceId = 2, // проверочные работы в модулях
                SolvingSeconds = model.SolvingSeconds,
                NumberOfAttempts = model.NumberOfAttempts,
                IsViewed = model.IsViewed,
                IsCorrect = model.IsCorrect
            };

            if (model.IsCorrect == true)
            {
                var dbQuestion = await db.Questions.Include(q => q.QuestionDifficulty).FirstOrDefaultAsync();
                new CustomerLevelUpdateHandler(db).UpdateLevel(dbQuestion.QuestionDifficulty.Value, userId);
            }

            if (model.QuestionAnswerVariants != null)
            {
                foreach (var variant in model.QuestionAnswerVariants)
                {
                    qa.QuestionAnswerVariants.Add(new QuestionAnswerVariant { QuestionVariantId = variant.QuestionVariantId });
                }
            }

            if (attempt.QuestionAnswers == null)
                attempt.QuestionAnswers = new List<QuestionAnswer>();

            attempt.QuestionAnswers.Add(qa);

            if (questionIds.IndexOf(model.QuestionId) == questionIds.Count() - 1)
            {               
                //var ur = await db.UserRatings.FirstOrDefaultAsync(u => u.UserId == userId);
                //if (ur == null)
                //{
                //    db.UserRatings.Add(new UserRatings { UserId = userId, Rating = experience });
                //}
                //else
                //{
                //    ur.Rating += experience;
                //}

                attempt.IsFinished = true;
            }

            await db.SaveChangesAsync();

            new StoredProcedure<SqlServer, object>("UpdateThemeAndModuleLearningRate").Exec(
                new FunctionParameter("@UserId", UserId),
                new FunctionParameter("@ThemeId", themeId),
                new FunctionParameter("@ModuleId", moduleId),
                new FunctionParameter("@AttemptId", attemptId)
                );

            new StoredProcedure<SqlServer, object>("UpdateNotLearnedModuleDeadlines").Exec(
                new FunctionParameter("@UserId", UserId),
                new FunctionParameter("@Questions", model.QuestionId.ToString())
                );
            return Ok(db.ModuleThemePassAttempts.FirstOrDefault(mtp => mtp.Id == attemptId).CurrentScore);
        }

        [HttpPost]
        [Route("PostControlWorkAnswers")]
        public async Task<IActionResult> PostControlWorkAnswers(QuestionAnswer model, int controlWorkId, double experience)
        {
            var userId = UserId;
            var dt = DateTime.Now;

            var qa = new QuestionAnswer
            {
                QuestionId = model.QuestionId,
                UserId = userId,
                Value = model.Value,
                QuestionAnswerVariants = new List<QuestionAnswerVariant>(),
                DateCreated = dt,
                AnswerSourceId = 7,
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

            if (model.IsCorrect != null && model.IsCorrect == true)
            {
                new CustomerLevelUpdateHandler(db).UpdateLevel(experience, userId);
                //var ur = await db.UserRatings.FirstOrDefaultAsync(u => u.UserId == userId);
                //if (ur == null)
                //{
                //    db.UserRatings.Add(new UserRatings { UserId = userId, Rating = experience });
                //}
                //else
                //{
                //    ur.Rating += experience;
                //}
            }


            var userToCw = await db.UsersToControlWorks.
                Include(u => u.ControlWork).
                Include(u=> u.UsersToControlWorksToQuestions).
                Include(u=> u.UsersToControlWorksToQuestions.Select(tq => tq.Question)).
                Include(u=> u.UsersToControlWorksToQuestions.Select(tq => tq.Question.QuestionDifficulty)).
                OrderByDescending(u => u.Id).
                FirstOrDefaultAsync(utc => utc.UserId == userId && utc.ControlWorkId == controlWorkId);

            if (userToCw == null)
            {
                return BadRequest();
            }

            var userToSubject = db.UsersToSubjects.Include(uts => uts.Subject).FirstOrDefault(uts => uts.SubjectId == userToCw.ControlWork.SubjectId);
            double subjectKnowingBorder = ((double)userToSubject.DesiredScore / userToSubject.Subject.MaximumScore * 100) ?? 0;


            var qids = userToCw.UsersToControlWorksToQuestions.OrderBy(ut => ut.Question.QuestionDifficultyId).ThenBy(ut => ut.QuestionId).Select(ut => ut.QuestionId).ToList();
            
            //завершил контрольную работу - ответил на все вопросы
            if (qids.IndexOf(model.QuestionId) == qids.Count - 1)
            {
                userToCw.IsCompleted = true;
            }

            var utctq = db.UsersToControlWorksToQuestions.FirstOrDefault(a => a.UsersToControlWorksId == userToCw.Id && a.QuestionId == model.QuestionId);
            if (utctq != null)
            {
                utctq.IsAnswered = true;
            }

            await db.SaveChangesAsync();

            var qas = db.QuestionAnswers.
                Include(a => a.Question).
                Include(a => a.Question.QuestionDifficulty).
                Where(a => qids.Contains(a.QuestionId) && a.UserId == userId).
                GroupBy(a => a.QuestionId).ToList();

            var totalDiffValue = 0.0;
            var totalCorrValue = 0.0;
            
            foreach (var q in userToCw.UsersToControlWorksToQuestions.OrderBy(u => u.Question.QuestionDifficultyId).ThenBy(u => u.QuestionId).ToList())
            {
                var answers = qas.FirstOrDefault(a => a.Key == q.QuestionId);
                var maxAnswer = answers?.OrderByDescending(a => a.Id).FirstOrDefault();

                if (qids.IndexOf(q.QuestionId) <= qids.IndexOf(model.QuestionId))
                {
                    totalCorrValue += maxAnswer.IsCorrect==true ? maxAnswer.Question.QuestionDifficulty.Value : 0.0;
                }

                totalDiffValue += q.Question.QuestionDifficulty.Value;
            }   

            var percent = (double)totalCorrValue / totalDiffValue * 100;
            userToCw.CurrentLearningRate = percent;

            //прошел контрольную работу - достиг балла
            if (userToCw.CurrentLearningRate >= subjectKnowingBorder)
            {
                db.Events.Add(new Event
                {
                    DateCreated = DateTime.Now,
                    UserId = userId,
                    TypeId = 4,
                    Url = "",
                    Text = $"Поздравляем! Ты прошел контрольную работу!"
                });
            }

            await db.SaveChangesAsync();

            //var UserId = new SqlParameter("@UserId", userId);
            //var Questions = new SqlParameter("@Questions", model.QuestionId.ToString());
            //db.Database.ExecuteSqlCommand("dbo.UpdateThemeOverallLearningRate @Questions, @UserId", Questions, UserId);

            return Ok(percent);
        }

        [HttpGet]
        [Route("CheckingTestScore")]
        public async Task<double> CheckingTestScore(int attemptId)
        {
            var attempt = await db.ModuleThemePassAttempts.
                FirstOrDefaultAsync(m => m.Id == attemptId);

            return attempt.CurrentScore;
        }

        [HttpGet]
        [Route("CheckingTestStats")]
        public async Task<CheckingTestStatsViewModel> CheckingTestStats(int attemptId)
        {
            var model = new CheckingTestStatsViewModel();

            var attempt = await db.ModuleThemePassAttempts.
                Include(m => m.AttemptsToQuestions).
                Include(m => m.QuestionAnswers).
                Include(m => m.QuestionAnswers.Select(a => a.Question)).
                Include(m => m.QuestionAnswers.Select(a => a.Question.QuestionDifficulty)).
                FirstOrDefaultAsync(m => m.Id == attemptId);

            model.ThemeKnowledgePercent = attempt.CurrentScore;
            model.QuestionsNumber = attempt.AttemptsToQuestions.Count();
            model.CorrectAnswersNumber = attempt.QuestionAnswers.Count(qa => qa.IsCorrect == true);
            model.TotalExperience = attempt.QuestionAnswers.Where(qa => qa.IsCorrect == true).Sum(qa => qa.Question.QuestionDifficulty.Value);

            return model;
        }

        [HttpGet]
        [Route("ControlWorkStats")]
        public async Task<CheckingTestStatsViewModel> ControlWorkStats(int controlWorkId)
        {
            var userId = UserId;
            var model = new CheckingTestStatsViewModel();

            var userToCw = db.UsersToControlWorks.
                Include(u => u.UsersToControlWorksToQuestions).
                Include(u => u.UsersToControlWorksToQuestions.Select(utc => utc.Question)).
                Include(u => u.UsersToControlWorksToQuestions.Select(utc => utc.Question.QuestionDifficulty)).
                OrderByDescending(u => u.Id).
                FirstOrDefault(utc => utc.ControlWorkId == controlWorkId && utc.UserId == userId /*&& utc.IsCompleted == true*/);


            model.ThemeKnowledgePercent = userToCw.CurrentLearningRate;
            model.QuestionsNumber = userToCw.UsersToControlWorksToQuestions.Count();

            var qids = userToCw.UsersToControlWorksToQuestions.
                Select(ut => ut.QuestionId).ToList();

            var qas = db.QuestionAnswers.
                Include(a => a.Question).
                Include(a => a.Question.QuestionDifficulty).
                Where(a => qids.Contains(a.QuestionId) && a.UserId == userId).
                GroupBy(a => a.QuestionId).ToList();

            var totalExp = 0.0;
            var correctAnswNumber = 0;

            foreach (var q in userToCw.UsersToControlWorksToQuestions.OrderBy(u => u.Question.QuestionDifficultyId).ThenBy(u => u.QuestionId).ToList())
            {
                var answers = qas.FirstOrDefault(a => a.Key == q.QuestionId);
                var maxAnswer = answers?.OrderByDescending(a => a.Id).FirstOrDefault();


                totalExp += maxAnswer?.IsCorrect == true ? q.Question.QuestionDifficulty.Value : 0.0;
                correctAnswNumber += maxAnswer?.IsCorrect == true ? 1 : 0;

            }

            model.CorrectAnswersNumber = correctAnswNumber;
            model.TotalExperience = totalExp;

            return model;
        }
    }
}
