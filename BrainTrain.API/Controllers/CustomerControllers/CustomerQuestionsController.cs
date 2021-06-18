using BrainTrain.API.Helpers;
using BrainTrain.API.Helpers.Learnosity;
using BrainTrain.API.Models;
using BrainTrain.Core.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [RoutePrefix("api/Customer/Questions")]
    public class CustomerQuestionsController : BaseApiController
    {
        /// <summary>
        /// Один вопрос в формате лерносити по айди 
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("QuestionSityById")]
        public CustomerLearnosityQuestionViewModel QuestionSityById(int questionId)
        {
            var question = db.Questions.Where(q => q.Id == questionId).Select(q => new
            {
                id = q.Id,
                contentManagerId = q.ContentManagerId,
                text = q.Text,
                questionTypeId = q.QuestionTypeId,
                questionDifficultyId = q.QuestionDifficultyId,
                dateCreated = q.DateCreated,
                correctAnswerValue = q.CorrectAnswerValue,
                isChecked = q.IsChecked,
                questionType = new { id = q.QuestionType.Id, title = q.QuestionType.Title },
                questionDifficulty = new { id = q.QuestionDifficulty.Id, title = q.QuestionDifficulty.Title, value = q.QuestionDifficulty.Value },
                questionsToThemes = q.QuestionsToThemes.Select(qtt => new { questionId = qtt.QuestionId, themeId = qtt.ThemeId, theme = new { id = qtt.Theme.Id, title = qtt.Theme.Title } }).ToList(),
                questionVariants = q.QuestionVariants.Select(qv => new { id = qv.Id, text = qv.Text, questionId = qv.QuestionId, isCorrect = qv.IsCorrect }).ToList(),
                solutions = q.Solutions.Select(s => new { id = s.Id, text = s.Text, questionId = s.QuestionId }).ToList(),
                numberOfSolved = q.QuestionAnswers.Where(qa => qa.Value.Trim().ToLower() == q.CorrectAnswerValue.Trim().ToLower() || qa.QuestionAnswerVariants.Any(qav => qav.QuestionVariant.IsCorrect == true)).Count(),
                averageSolvingSeconds = q.QuestionAnswers.Where(qa => qa.SolvingSeconds != null).Average(qa => qa.SolvingSeconds),
                jsonData = q.JsonData
            }).AsEnumerable().Select(q => new QuestionViewModel
            {
                Id = q.id,
                ContentManagerId = q.contentManagerId,
                Text = q.text,
                QuestionTypeId = q.questionTypeId,
                QuestionDifficultyId = q.questionDifficultyId,
                DateCreated = q.dateCreated,
                CorrectAnswerValue = q.correctAnswerValue,
                IsChecked = q.isChecked,
                QuestionType = new QuestionType { Id = q.questionType.id, Title = q.questionType.title },
                QuestionDifficulty = new QuestionDifficulty { Id = q.questionDifficulty.id, Title = q.questionDifficulty.title, Value = q.questionDifficulty.value },
                QuestionsToThemes = q.questionsToThemes.Select(qtt => new QuestionsToThemes { QuestionId = qtt.questionId, ThemeId = qtt.themeId, Theme = new Theme { Id = qtt.theme.id, Title = qtt.theme.title } }).ToList(),
                QuestionVariants = q.questionVariants.Select(qv => new QuestionVariant { Id = qv.id, Text = qv.text, QuestionId = qv.questionId, IsCorrect = qv.isCorrect }).ToList(),
                Solutions = q.solutions.Select(s => new Solution { Id = s.id, Text = s.text, QuestionId = s.questionId }).ToList(),
                NumberOfSolved = q.numberOfSolved,
                AverageSolvingSeconds = q.averageSolvingSeconds ?? 0,
                QuestionAnswers = null,
                JsonData = q.jsonData
            }).ToList();

            var model = new CustomerLearnosityQuestionViewModel ();
            model.QuestionId = question[0].Id;
            model.QuestionExperience = question[0].QuestionDifficulty.Value;
            model.QuestionsToThemes = question[0].QuestionsToThemes.ToList();
            model.Solutions = question[0].Solutions.ToList();
            model.QuestionDifficulty = question[0].QuestionDifficulty;
            model.QuestionDifficultyId = question[0].QuestionDifficultyId;
            model.NumberOfSolved = question[0].NumberOfSolved;
            model.AverageSolvingSeconds = question[0].AverageSolvingSeconds;

            var uuid = "";

            var json = LQuestions.Simple(question, out uuid);

            model.LearnosityJson = json;

            return model;
        }

        /// <summary>
        /// Выборка вопросов для прохождения модулей with Learnosity
        /// </summary>
        /// <param name="themeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ThemeQuestionsSity")]
        public CustomerLearnosityModuleQuestionViewModel ThemeQuestionsLearnosity(int themeId, bool? isModuleQuestions)
        {
            var userId = User.Identity.GetUserId();
            var model = new CustomerLearnosityModuleQuestionViewModel();

            var qIds = new List<int>();
            IQueryable<Question> qs = db.Questions.Include(q => q.QuestionVariants).
                Include(q => q.QuestionVariants).Include(q => q.QuestionsToMaterials);
            var isNewAttempt = false;
            
            var lastAttempt = db.ModuleThemePassAttempts.
                Include(a => a.QuestionAnswers).
                Include(a => a.AttemptsToQuestions).
                Include(a => a.AttemptsToQuestions.Select(atq => atq.Question)).
                Include(a => a.AttemptsToQuestions.Select(atq => atq.Question.QuestionDifficulty)).
                OrderByDescending(a => a.Id).
                FirstOrDefault(a => a.ThemeId == themeId && a.UserId == userId);

            //если попытка не завершена
            if (lastAttempt != null && (lastAttempt.IsFinished == null || lastAttempt.IsFinished == false))
            {
                //получаем попытку с ответами и вопросами
                var questionsWithAnswersModel = lastAttempt.AttemptsToQuestions.
                    OrderBy(atq => atq.Question.QuestionDifficultyId).
                    ThenBy(atq => atq.QuestionId).
                    Select(qa => new CustomerQuestionWithDifficultyAndCorrectnessViewModel
                    {
                        QuestionId = qa.QuestionId,
                        DifficultyId = qa.Question.QuestionDifficultyId,
                        ScoreValue = qa.Question.QuestionDifficulty.Value,
                        IsCorrect = lastAttempt.QuestionAnswers.FirstOrDefault(qan => qan.QuestionId == qa.QuestionId)?.IsCorrect
                    }).ToList();

                model.AttemptId = lastAttempt.Id;
                model.NumberOfQuestions = questionsWithAnswersModel.Count;
                model.QuestionIds = questionsWithAnswersModel;
                model.Score = lastAttempt.CurrentScore;
            }
            else
            //если попытка новая
            {
                isNewAttempt = true;
                //подсчет количества неотвеченных вопросов - простые, средние и сложные
                var simpleQuestionCount = db.Questions.Where(q => q.IsModuleQuestion == isModuleQuestions && q.QuestionsToThemes.Any(qtt => qtt.ThemeId == themeId)
                    && !q.QuestionsToMaterials.Any()
                    && q.QuestionDifficultyId == 1
                    && !q.QuestionAnswers.Any(qa => qa.UserId == userId)).Count();
                var averageQuestionCount = db.Questions.Where(q => q.IsModuleQuestion == isModuleQuestions && q.QuestionsToThemes.Any(qtt => qtt.ThemeId == themeId)
                    && !q.QuestionsToMaterials.Any()
                    && q.QuestionDifficultyId == 2
                    && !q.QuestionAnswers.Any(qa => qa.UserId == userId)).Count();
                var hardQuestionCount = db.Questions.Where(q => q.IsModuleQuestion == isModuleQuestions && q.QuestionsToThemes.Any(qtt => qtt.ThemeId == themeId)
                    && !q.QuestionsToMaterials.Any()
                    && q.QuestionDifficultyId == 4
                    && !q.QuestionAnswers.Any(qa => qa.UserId == userId)).Count();

                //совместный список всех вопросов
                var questionSetIds = new List<CustomerQuestionWithDifficultyAndCorrectnessViewModel>();

                //простые вопросы выборка
                var questionSetQuerySimple = db.Questions.
                    Include(q => q.QuestionDifficulty).
                    Where(q => q.IsModuleQuestion == isModuleQuestions && q.QuestionsToThemes.Any(qtt => qtt.ThemeId == themeId)
                    && !q.QuestionsToMaterials.Any()
                    && q.QuestionDifficultyId == 1
                    && (q.QuestionTypeId != 7)
                    );
                if (simpleQuestionCount >= 5)
                    questionSetQuerySimple = questionSetQuerySimple.Where(q => !q.QuestionAnswers.Any(qa => qa.UserId == userId));
                questionSetIds.AddRange(questionSetQuerySimple.Select(q => new CustomerQuestionWithDifficultyAndCorrectnessViewModel
                {
                    QuestionId = q.Id,
                    DifficultyId = q.QuestionDifficultyId,
                    ScoreValue = q.QuestionDifficulty.Value,
                    IsCorrect = null
                } ).OrderBy(q => Guid.NewGuid()).Take(5).ToList());

                //средние вопросы выборка
                var questionSetQueryAverage = db.Questions.
                    Include(q => q.QuestionDifficulty).
                    Where(q => q.IsModuleQuestion == isModuleQuestions && q.QuestionsToThemes.Any(qtt => qtt.ThemeId == themeId)
                    && !q.QuestionsToMaterials.Any()
                    && q.QuestionDifficultyId == 2
                    && (q.QuestionTypeId != 7)
                    );
                if (averageQuestionCount >= 3)
                    questionSetQueryAverage = questionSetQueryAverage.Where(q => !q.QuestionAnswers.Any(qa => qa.UserId == userId));
                questionSetIds.AddRange(questionSetQueryAverage.Select(q => new CustomerQuestionWithDifficultyAndCorrectnessViewModel
                {
                    QuestionId = q.Id,
                    DifficultyId = q.QuestionDifficultyId,
                    ScoreValue = q.QuestionDifficulty.Value,
                    IsCorrect = null
                }).OrderBy(q => Guid.NewGuid()).Take(3).ToList());

                //сложные вопросы выборка
                var questionSetQueryHard = db.Questions.
                    Include(q => q.QuestionDifficulty).
                    Where(q => q.IsModuleQuestion == isModuleQuestions && q.QuestionsToThemes.Any(qtt => qtt.ThemeId == themeId)
                    && !q.QuestionsToMaterials.Any()
                    && q.QuestionDifficultyId == 4
                    && (q.QuestionTypeId != 7)
                    );
                if (hardQuestionCount >= 2)
                    questionSetQueryHard = questionSetQueryHard.Where(q => !q.QuestionAnswers.Any(qa => qa.UserId == userId));
                questionSetIds.AddRange(questionSetQueryHard.Select(q => new CustomerQuestionWithDifficultyAndCorrectnessViewModel
                {
                    QuestionId = q.Id,
                    DifficultyId = q.QuestionDifficultyId,
                    ScoreValue = q.QuestionDifficulty.Value,
                    IsCorrect = null
                }).OrderBy(q => Guid.NewGuid()).Take(2).ToList());

                //создание новой сущности попытки
                var attempt = new ModuleThemePassAttempt
                {
                    ThemeId = themeId,
                    UserId = userId,
                    DateCreated = DateTime.Now,
                    AttemptsToQuestions = new List<ModuleThemePassAttemptsToQuestions>()
                };

                questionSetIds = questionSetIds.OrderBy(qsi => qsi.DifficultyId).ThenBy(qsi => qsi.QuestionId).ToList();
                attempt.AttemptsToQuestions = questionSetIds.Select(qsi => new ModuleThemePassAttemptsToQuestions { QuestionId = qsi.QuestionId }).ToList();

                db.ModuleThemePassAttempts.Add(attempt);
                db.SaveChanges();

                model.AttemptId = attempt.Id;
                model.NumberOfQuestions = questionSetIds.Count();
                model.QuestionIds = questionSetIds;
                model.Score = 0;
            }

            qIds = model.QuestionIds.OrderBy(qwa => qwa.DifficultyId).
                    ThenBy(qwa => qwa.QuestionId).
                    Select(qwa => qwa.QuestionId).ToList();
            qs = qs.Where(q => qIds.Contains(q.Id));

            var returnQuestions = new List<QuestionViewModel>();

            if (!isNewAttempt)
            {
                qs = qs.Where(q => q.QuestionAnswers.Any(qa => qa.ModuleThemePassAttemptId == lastAttempt.Id) == false);
            }

            var question = qs.Select(q => new
            {
                id = q.Id,
                contentManagerId = q.ContentManagerId,
                text = q.Text,
                questionTypeId = q.QuestionTypeId,
                questionDifficultyId = q.QuestionDifficultyId,
                dateCreated = q.DateCreated,
                correctAnswerValue = q.CorrectAnswerValue,
                isChecked = q.IsChecked,
                questionType = new { id = q.QuestionType.Id, title = q.QuestionType.Title },
                questionDifficulty = new { id = q.QuestionDifficulty.Id, title = q.QuestionDifficulty.Title, value = q.QuestionDifficulty.Value },
                questionsToThemes = q.QuestionsToThemes.Select(qtt => new { questionId = qtt.QuestionId, themeId = qtt.ThemeId, theme = new { id = qtt.Theme.Id, title = qtt.Theme.Title } }).ToList(),
                questionVariants = q.QuestionVariants.Select(qv => new { id = qv.Id, text = qv.Text, questionId = qv.QuestionId, isCorrect = qv.IsCorrect }).ToList(),
                solutions = q.Solutions.Select(s => new { id = s.Id, text = s.Text, questionId = s.QuestionId }).ToList(),
                numberOfSolved = q.QuestionAnswers.Where(qa => qa.Value.Trim().ToLower() == q.CorrectAnswerValue.Trim().ToLower() || qa.QuestionAnswerVariants.Any(qav => qav.QuestionVariant.IsCorrect == true)).Count(),
                averageSolvingSeconds = q.QuestionAnswers.Where(qa => qa.SolvingSeconds != null).Average(qa => qa.SolvingSeconds),
                jsonData = q.JsonData
            }).AsEnumerable().Select(q => new QuestionViewModel
            {
                Id = q.id,
                ContentManagerId = q.contentManagerId,
                Text = q.text,
                QuestionTypeId = q.questionTypeId,
                QuestionDifficultyId = q.questionDifficultyId,
                DateCreated = q.dateCreated,
                CorrectAnswerValue = q.correctAnswerValue,
                IsChecked = q.isChecked,
                QuestionType = new QuestionType { Id = q.questionType.id, Title = q.questionType.title },
                QuestionDifficulty = new QuestionDifficulty { Id = q.questionDifficulty.id, Title = q.questionDifficulty.title, Value = q.questionDifficulty.value },
                QuestionsToThemes = q.questionsToThemes.Select(qtt => new QuestionsToThemes { QuestionId = qtt.questionId, ThemeId = qtt.themeId, Theme = new Theme { Id = qtt.theme.id, Title = qtt.theme.title } }).ToList(),
                QuestionVariants = q.questionVariants.Select(qv => new QuestionVariant { Id = qv.id, Text = qv.text, QuestionId = qv.questionId, IsCorrect = qv.isCorrect }).ToList(),
                Solutions = q.solutions.Select(s => new Solution { Id = s.id, Text = s.text, QuestionId = s.questionId }).ToList(),
                NumberOfSolved = q.numberOfSolved,
                AverageSolvingSeconds = q.averageSolvingSeconds ?? 0,
                QuestionAnswers = null,
                JsonData = q.jsonData
            }).OrderBy(q => qIds.IndexOf(q.Id))

            .FirstOrDefault();
            returnQuestions.Add(question);

            model.QuestionId = question.Id;
            model.QuestionNumber = qIds.IndexOf(model.QuestionId);
            model.QuestionExperience = question.QuestionDifficulty.Value;
            model.QuestionsToThemes = question.QuestionsToThemes.ToList();
            model.Solutions = question.Solutions.ToList();
            model.QuestionDifficulty = question.QuestionDifficulty;
            model.QuestionDifficultyId = question.QuestionDifficultyId;
            model.NumberOfSolved = question.NumberOfSolved;
            model.AverageSolvingSeconds = question.AverageSolvingSeconds;

            var uuid = "";

            var json = LQuestions.Simple(returnQuestions, out uuid);

            model.LearnosityJson = json;

            return model;
        }
        
        /// <summary>
        /// Выборка  вопросов для тестов в общем списке всех тем
        /// </summary>
        /// <param name="themeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ThemeAllQuestions")]
        public IEnumerable<CustomerQuestionWithDifficultyAndCorrectnessViewModel> ThemeAllQuestions(int themeId, bool? takeLimited = null)
        {
            var userId = User.Identity.GetUserId();

            var qs = db.Questions.Include(q => q.QuestionDifficulty).
                Where(q => q.QuestionsToThemes.Any(qtt => qtt.ThemeId == themeId)
                && (q.QuestionTypeId != 7)
                //&& q.IsModuleQuestion == null || q.IsModuleQuestion == false
                ).OrderBy(q => Guid.NewGuid())./*Take(10).*/ToList();

            if (takeLimited!=null && takeLimited.Value == true)
            {
                return qs.Take(10).OrderBy(q => q.QuestionDifficultyId).Select(q => new CustomerQuestionWithDifficultyAndCorrectnessViewModel
                {
                    QuestionId = q.Id,
                    DifficultyId = q.QuestionDifficultyId,
                    ScoreValue = q.QuestionDifficulty.Value,
                    IsCorrect = null
                }).ToList();
            }

            return qs.Select(q => new CustomerQuestionWithDifficultyAndCorrectnessViewModel
            {
                QuestionId = q.Id,
                DifficultyId = q.QuestionDifficultyId,
                ScoreValue = q.QuestionDifficulty.Value,
                IsCorrect = null
            }).ToList();
        }

        /// <summary>
        /// Выборка вопросов для целого раздела
        /// </summary>
        /// <param name="parentThemeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ThemeParentQuestions")]
        public IEnumerable<CustomerQuestionWithDifficultyViewModel> ThemeParentQuestions(int parentThemeId)
        {
            var qs = db.Questions.Include(q => q.QuestionDifficulty).Where(q => q.QuestionsToThemes.Any(qtt => qtt.Theme.ParentThemeId == parentThemeId)).Select(q => new CustomerQuestionWithDifficultyViewModel
            {
                QuestionId = q.Id,
                DifficultyId = q.QuestionDifficultyId,
                ScoreValue = q.QuestionDifficulty.Value
            }).OrderBy(q => Guid.NewGuid()).Take(20).ToList();

            return qs;
        }

        [HttpGet]
        [Route("AnswerSources")]
        public IEnumerable<AnswerSource> AnswerSources()
        {
            var s = db.AnswerSources.ToList();

            return s;
        }

        /// <summary>
        /// Отправка ответов на любой вопрос в формате лерносити
        /// </summary>
        /// <param name="model"></param>
        /// <param name="answerSourceId"></param>
        /// <param name="experience"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PostQuestionAnswers")]
        public async Task<IActionResult> PostQuestionAnswersAsync(QuestionAnswer model, int answerSourceId, double experience)
        {
            var userId = User.Identity.GetUserId();
            var dt = DateTime.Now;


            var qa = new QuestionAnswer
            {
                QuestionId = model.QuestionId,
                UserId = userId,
                Value = model.Value,
                QuestionAnswerVariants = new List<QuestionAnswerVariant>(),
                DateCreated = dt,
                AnswerSourceId = answerSourceId,
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


            //var ur = await db.UserRatings.FirstOrDefaultAsync(u => u.UserId == userId);
            //if (ur == null)
            //{
            //    db.UserRatings.Add(new UserRatings { UserId = userId, Rating = experience });
            //}
            //else
            //{
            //    ur.Rating += experience;
            //}

            CustomerLevelUpdateHandler.UpdateLevel(experience, userId);

            await db.SaveChangesAsync();

            var UserId = new SqlParameter("@UserId", userId);
            var Questions = new SqlParameter("@Questions", model.QuestionId.ToString());
            db.Database.ExecuteSqlCommand("dbo.UpdateThemeOverallLearningRate @Questions, @UserId", Questions, UserId);

            return Ok();
        }

        [HttpGet]
        [Route("QuestionDifficulties")]
        public IEnumerable<QuestionDifficulty> QuestionDifficulties()
        {
            var s = db.QuestionDifficulties.ToList();

            return s;
        }

        /// <summary>
        /// вопросы по сложности
        /// </summary>
        /// <param name="themeId"></param>
        /// <param name="difficultyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ThemeQuestionsByDifficulty")]
        public IEnumerable<CustomerQuestionWithDifficultyViewModel> ThemeQuestionsByDifficulty(int themeId, int difficultyId)
        {
            var userId = User.Identity.GetUserId();

            var qs = db.Questions.Include(q => q.QuestionDifficulty).Where(q => !q.QuestionAnswers.Any(qa => qa.UserId == userId) && q.QuestionsToThemes.Any(qtt => qtt.ThemeId == themeId) && q.QuestionDifficultyId == difficultyId).Select(q => new CustomerQuestionWithDifficultyViewModel {
                QuestionId = q.Id,
                DifficultyId = q.QuestionDifficultyId,
                ScoreValue = q.QuestionDifficulty.Value
            }).OrderBy(q => Guid.NewGuid()).
                Take(10).ToList();

            return qs;
        }

        [HttpGet]
        [Route("SprintQuestions")]
        public IEnumerable<int> SprintQuestions(int subjectId)
        {
            var userId = User.Identity.GetUserId();
            var qs = db.Questions.Where(q => !q.QuestionAnswers.Any(qa => qa.UserId == userId) && q.QuestionsToThemes.Any(qtt => qtt.Theme.SubjectId == subjectId)).Select(q => q.Id).OrderBy(q => Guid.NewGuid()).Take(10).ToList();

            return qs;
        }

        [HttpGet]
        [Route("TrainingQuestions")]
        public IEnumerable<int> TrainingQuestions(int subjectId)
        {
            var userId = User.Identity.GetUserId();

            //old version
            //var UserId = new SqlParameter("@UserId", userId);
            //var SubjectId = new SqlParameter("@SubjectId", subjectId);
            //var AnswerSourceId = new SqlParameter("@AnswerSourceId", 2);
            //var themes = db.Database.SqlQuery<CustomerThemeKnowledgeLevelViewModel>("GetThemesKnowledgeLevels @UserId, @SubjectId, @AnswerSourceId", UserId, SubjectId, AnswerSourceId).ToList();
            //var knownThemeIds = themes.Where(t => t.IsThemeKnown == 1).Select(t => t.ThemeId).ToList();

            var knownThemeIds = db.UsersToThemes.Where(utt => utt.UserId == userId && utt.Theme.SubjectId == subjectId && utt.IsThemeLearned == true).Select(utt => utt.ThemeId).ToList();

            var questions = db.QuestionsToThemes.Where(qtt => knownThemeIds.Contains(qtt.ThemeId) && qtt.Theme.SubjectId == subjectId).Select(qtt => qtt.QuestionId).OrderBy(q => Guid.NewGuid()).Take(10).ToList();

            return questions;
        }


        /// <summary>
        /// Выборка вопросов контрольной работы для нескольких модулей
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ModuleControlWorkQuestions")]
        public IEnumerable<CustomerControlWorkQuestionViewModel> ModuleControlWorkQuestions(int controlWorkId)
        {
            var userId = User.Identity.GetUserId();
            var themeIds = new List<int>();

            var existingUserToCw = db.UsersToControlWorks.
                Include(u => u.UsersToControlWorksToQuestions).
                Include(u => u.UsersToControlWorksToQuestions.Select(utc => utc.Question)).
                Include(u => u.UsersToControlWorksToQuestions.Select(utc => utc.Question.QuestionDifficulty)).
                OrderByDescending(u => u.Id).
                FirstOrDefault(utc => utc.ControlWorkId == controlWorkId && utc.UserId == userId && utc.IsCompleted == false);
            if (existingUserToCw != null && existingUserToCw.IsCompleted == false)
            {
                return existingUserToCw.UsersToControlWorksToQuestions.
                    OrderBy(u => u.Question.QuestionDifficultyId).
                    ThenBy(u => u.QuestionId).
                    Select(a => new CustomerControlWorkQuestionViewModel {
                        QuestionId = a.QuestionId,
                        IsAnswered = a.IsAnswered,
                        DifficultyId = a.Question.QuestionDifficultyId,
                        ScoreValue = a.Question.QuestionDifficulty.Value,
                        IsCorrect = a.IsAnswered ? db.QuestionAnswers.OrderByDescending(qa => qa.Id).FirstOrDefault(qa => qa.UserId == userId && qa.QuestionId == a.QuestionId).IsCorrect : null
                    }).ToList();
            }
                        
            var controlWorkModules = db.ControlWorksToModules.
                Include(cw => cw.Module.ThemesToModules).
                Where(cw => cw.ControlWorkId == controlWorkId).
                Select(cw => new {
                    Id = cw.Module.Id,
                    Title = cw.Module.Title,
                    ThemesToModules = cw.Module.ThemesToModules.Where(ttm => ttm.IsDominant == true).Select(ttm => new  {
                        ModuleId = ttm.ModuleId,
                        ThemeId = ttm.ThemeId }).ToList()
                }).AsEnumerable().Select(m => new Module
                {
                    Id = m.Id,
                    Title = m.Title,
                    ThemesToModules = m.ThemesToModules.Select(ttm => new ThemesToModules
                    {
                        ModuleId = ttm.ModuleId,
                        ThemeId = ttm.ThemeId
                    }).ToList()
                }).ToList();

            foreach (var mod in controlWorkModules)
            {
                themeIds.AddRange(mod.ThemesToModules.Select(ttm => ttm.ThemeId).ToList());
            }

            var questions = new List<Question>();
            foreach (var tid in themeIds)
            {
                questions.AddRange(
                db.Questions.Include(q => q.QuestionDifficulty).
                    Where(q => q.QuestionsToThemes.Any(qtt => qtt.ThemeId == tid)
                    && q.IsModuleQuestion == true && q.QuestionDifficultyId == 2
                    && (q.QuestionTypeId != 7)
                    ).OrderBy(q => Guid.NewGuid()).Take(2).ToList());
            }


            var qs = questions.OrderBy(q => q.QuestionDifficultyId).ThenBy(q => q.Id).
                Select(q => new CustomerControlWorkQuestionViewModel
                {
                    QuestionId = q.Id,
                    IsAnswered = false,
                    DifficultyId = q.QuestionDifficultyId,
                    IsCorrect = null,
                    ScoreValue = q.QuestionDifficulty.Value
                }).ToList();

            var usersToCw = new UsersToControlWorks {
                UserId = userId,
                ControlWorkId = controlWorkId,
                IsCompleted = false,
                CurrentLearningRate = 0.0,
                UsersToControlWorksToQuestions = new List<UsersToControlWorksToQuestions>()
            };
            foreach (var item in qs)
            {
                usersToCw.UsersToControlWorksToQuestions.Add(new UsersToControlWorksToQuestions { QuestionId = item.QuestionId, IsAnswered = false });
            }
            db.UsersToControlWorks.Add(usersToCw);
            db.SaveChanges();

            return qs;
        }
    }
}
