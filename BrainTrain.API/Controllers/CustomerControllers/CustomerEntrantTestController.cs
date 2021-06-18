using BrainTrain.API.Helpers;
using BrainTrain.Core.Models;
using BrainTrain.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [Route("api/Customer/EntrantTest")]
    public class CustomerEntrantTestController : BaseApiController
    {
        public CustomerEntrantTestController(BrainTrainContext _db) : base(_db)
        {
        }

        [HttpGet]
        [Route("EntrantTestQuestions")]
        public async Task<List<CustomerEntrantTestQuestionViewModel>> EntrantTestQuestions(int subjectId)
        {
            var userId = UserId;
            var userToSubj = await db.UsersToSubjects.Include(UsersToSubjects => UsersToSubjects.Subject).FirstOrDefaultAsync(uts => uts.UserId == userId && uts.SubjectId == subjectId);
            if (userToSubj == null)
                throw new Exception("Subject not found");
            var learningBorder = userToSubj.DesiredScore / userToSubj.Subject.MaximumScore * 100;

            //var difficultyId = /*(int?) null;*/ learningBorder <= 50 ? 1 : (learningBorder <= 80 ? 2 : 4);

            //var entrantQuestionIds = db.EntrantTestQuestions.Where(eq => eq.SubjectId == subjectId && (difficultyId == null || eq.Question.QuestionDifficultyId == difficultyId)).Select(eq => eq.QuestionId).ToList();
            var entrantQuestionIds = db.EntrantTestQuestions.Where(eq => eq.SubjectId == subjectId).Select(eq => eq.QuestionId).ToList();

            var answers = db.QuestionAnswers.Where(qa => qa.UserId == userId && entrantQuestionIds.Contains(qa.QuestionId)).ToList();
            if (answers.Count == 0)
            {
                // не начал проходить тест
                return entrantQuestionIds.Select(q => new CustomerEntrantTestQuestionViewModel { QuestionId = q, IsAnswered = false, IsCorrect = null }).ToList();                
            }

            //начал проходить тест
            return entrantQuestionIds.Select(q => new CustomerEntrantTestQuestionViewModel {
                QuestionId = q,
                IsAnswered = answers.Any(a => a.QuestionId == q) ? true : false,
                IsCorrect = answers.FirstOrDefault(a => a.QuestionId == q)?.IsCorrect
            }).ToList();
        }

        [HttpPost]
        [Route("PostAnswers")]
        public async Task<IActionResult> PostEntrantTestAnswers(QuestionAnswer model, int subjectId)
        {
            var userId = UserId;
            var dt = DateTime.Now;
            var checkingTestQuestions = await db.EntrantTestQuestions.Where(etq => etq.SubjectId == subjectId).Select(etq => etq.QuestionId).ToListAsync();

            var qa = new QuestionAnswer
            {
                QuestionId = model.QuestionId,
                UserId = userId,
                Value = model.Value,
                QuestionAnswerVariants = new List<QuestionAnswerVariant>(),
                DateCreated = dt,
                AnswerSourceId = 1,
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

            //if (model.IsCorrect == true)
            //{
            //    var question = await db.Questions.Include(q => q.QuestionDifficulty).FirstOrDefaultAsync(q => q.Id == model.QuestionId);
            //    if (question != null)
            //    {
            //        var userRating = await db.UserRatings.FirstOrDefaultAsync(u => u.UserId == userId);
            //        if (userRating != null)
            //        {
            //            userRating.Rating += question.QuestionDifficulty.Value;
            //        }
            //        else
            //        {
            //            db.UserRatings.Add(new UserRatings { UserId = userId, Rating = question.QuestionDifficulty.Value });
            //        }
            //    }
            //}
            
            await db.SaveChangesAsync();

            if (model.IsCorrect == false || checkingTestQuestions.IndexOf(model.QuestionId) == checkingTestQuestions.Count-1)
            {
                var pastQuestionAnswers = await db.QuestionAnswers.Where(a => a.UserId == userId && checkingTestQuestions.Contains(a.QuestionId)).ToListAsync();
                await ProcessEntrantThemes(pastQuestionAnswers, userId, subjectId, model.QuestionId);
            }

            return Ok();
        }

        /// <summary>
        /// Для обработки ответов на проверочный тест и опускание тем/модулей в пути прохождения
        /// </summary>
        /// <param name="qas"></param>
        /// <returns></returns>
        private async Task ProcessEntrantThemes(List<QuestionAnswer> qas, string userId, int subjectId, int currentQuestionId) 
        {
            var modulesToSkip = new List<int>();
            var currentModule = 0;
            var experience = 0.0;

            var learnedThemeIds = new List<int>();

            foreach (var answ in qas)
            {
                if (answ.IsCorrect == true)
                {
                    learnedThemeIds.AddRange(await db.EntrantTestQuestionsToThemes.Where(e => e.EntrantTestQuestion.QuestionId == answ.QuestionId).Select(e => e.ThemeId).ToListAsync());
                }
            }

            var currentQuestionThemeIds = await db.EntrantTestQuestionsToThemes.Where(e => e.EntrantTestQuestion.QuestionId == currentQuestionId).Select(e => e.ThemeId).ToListAsync();

            var modules = await db.Modules.Include(m => m.ThemesToModules).Include(m => m.ThemesToModules.Select(ttm => ttm.Theme)).
                Where(m => m.ThemesToModules.All(ttm => (learnedThemeIds.Contains(ttm.ThemeId) && ttm.IsDominant == true) || ttm.IsDominant == false)).ToListAsync();

            currentModule = (await db.Modules.FirstOrDefaultAsync(m => m.ThemesToModules.Any(ttm => ttm.IsDominant == true && currentQuestionThemeIds.Contains(ttm.ThemeId)))).Id;

            foreach (var module in modules)
            {
                foreach (var themeToM in module.ThemesToModules.Where(ttm => ttm.IsDominant == true).ToList())
                {
                    experience += 10;
                }

                modulesToSkip.Add(module.Id);
            }

            //foreach (var answ in qas)
            //{
            //    if (answ.IsCorrect == true)
            //    {
            //        var themeIds = await db.EntrantTestQuestionsToThemes.Where(e => e.EntrantTestQuestion.QuestionId == answ.QuestionId).Select(e => e.ThemeId).ToListAsync();

            //        //var module = await db.Modules.Include(m => m.ThemesToModules).Include(m => m.ThemesToModules.Select(ttm => ttm.Theme)).
            //        //    FirstOrDefaultAsync(m => m.ThemesToModules.All(ttm => themeIds.Contains(ttm.ThemeId)) == true);
            //        var module = await db.Modules.Include(m => m.ThemesToModules).Include(m => m.ThemesToModules.Select(ttm => ttm.Theme)).
            //            FirstOrDefaultAsync(m => m.ThemesToModules.Any(ttm => themeIds.Contains(ttm.ThemeId) && ttm.IsDominant == true));

            //        if (module != null)
            //        {
            //            if (modulesToSkip.Contains(module.Id) == false)
            //            {
            //                modulesToSkip.Add(module.Id);

            //                foreach (var themeToM in module.ThemesToModules.Where(ttm => ttm.IsDominant == true).ToList())
            //                {
            //                    experience += 10;
            //                }
            //            }
            //        }

            //        if (answ.QuestionId == currentQuestionId)
            //            currentModule = module.Id;
            //    }
            //}

            var UserId = new SqlParameter("@UserId", userId);
            var SubjectId = new SqlParameter("@SubjectId", subjectId);
            var ModulesToSkip = new SqlParameter("@ModulesToSkip", string.Join("|", modulesToSkip.Where(m => m != currentModule).ToList()));

            db.Database.ExecuteSqlRaw("dbo.InsertUsersToQuickModules @SubjectId, @UserId, @ModulesToSkip", SubjectId, UserId, ModulesToSkip);

            var userModules = await db.UsersToModules.Where(utm => utm.UserId == userId).ToListAsync();
            var cwCounter = 0;
            var cwModules = new List<int>();

            foreach (var mod in userModules)
            {
                if (cwCounter == 3 || userModules.IndexOf(mod) == userModules.Count-1)
                {
                    var newControlWork = new ControlWork
                    {
                        UserId = userId,
                        TypeId = 1,
                        SubjectId = subjectId,
                        Title = "Контрольная работа",
                        ControlWorksToModules = cwModules.Select(cwm => new ControlWorksToModules { ModuleId = cwm }).ToList()
                    };
                    db.ControlWorks.Add(newControlWork);

                    cwModules = new List<int>();
                    cwCounter = 0;

                }

                cwCounter++;
                cwModules.Add(mod.ModuleId);
            }


            new CustomerLevelUpdateHandler(db).UpdateLevel(experience, userId);
            //var ur = await db.UserRatings.FirstOrDefaultAsync(ut => ut.UserId == userId);
            //if (ur == null)
            //{
            //    db.UserRatings.Add(new UserRatings { Rating = experience, UserId = userId });
            //}
            //else
            //{
            //    ur.Rating += experience;
            //}

            await db.SaveChangesAsync();
        }
    }
}
