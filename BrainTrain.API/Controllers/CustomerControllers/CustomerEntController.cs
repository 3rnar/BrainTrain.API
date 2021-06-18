using BrainTrain.API.Helpers.Learnosity;
using BrainTrain.API.Models;
using BrainTrain.Core.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [RoutePrefix("api/Customer/Ent")]
    public class CustomerEntController : BaseApiController
    {
        [HttpGet]
        [Route("Years")]
        public async Task<IEnumerable<CustomerEntYearViewModel>> GetYears(int subjectId)
        {
            var years = db.EntYears.Include(y => y.EntVariants).
                Where(y => y.EntVariants.Any(v => v.SubjectId == subjectId)).
                Select(y => new CustomerEntYearViewModel { Id = y.Id, Title = y.Title }).ToList();

            var userId = User.Identity.GetUserId();
            var usersToEntVariants = await db.UsersToEntVariants.Include(utv => utv.EntVariant).
                Where(utv => utv.UserId == userId && utv.EntVariant.SubjectId == subjectId).ToListAsync();

            foreach (var y in years)
            {
                y.EntVariants = new List<CustomerEntVariantViewModel>();
                var variants = await db.EntVariants.Include(ev => ev.EntVariantsToQuestions).
                    Where(v => v.SubjectId == subjectId && v.EntYearId == y.Id).ToListAsync();
                foreach (var varnt in variants)
                {
                    var item = new CustomerEntVariantViewModel
                    {
                        Id = varnt.Id,
                        EntYearId = varnt.EntYearId,
                        SubjectId = varnt.SubjectId,
                        Title = varnt.Title,
                        NumberOfQuestions = varnt.EntVariantsToQuestions.Count(),
                        KnowingPercentage = usersToEntVariants.FirstOrDefault(utv => utv.EntVariantId == varnt.Id)?.CurrentLearningRate ?? 0,
                        NumberOfCorrectAnswers = usersToEntVariants.FirstOrDefault(utv => utv.EntVariantId == varnt.Id)?.NumberOfCorrectAnswers ?? 0
                    };

                    y.EntVariants.Add(item);
                }
            }

            return years;
        }

        [HttpGet]
        [Route("VariantsByYear")]
        public async Task<IEnumerable<CustomerEntVariantViewModel>> GetVariantsByYear(int yearId, int subjectId)
        {
            var model = new List<CustomerEntVariantViewModel>();
            var userId = User.Identity.GetUserId();
            var usersToEntVariants = await db.UsersToEntVariants.Include(utv => utv.EntVariant).
                Where(utv => utv.UserId == userId && utv.EntVariant.SubjectId == subjectId).ToListAsync();
            var variants = await db.EntVariants.Include(ev => ev.EntVariantsToQuestions).
                Where(v => v.SubjectId == subjectId && v.EntYearId == yearId).ToListAsync();

            foreach (var varnt in variants)
            {
                var item = new CustomerEntVariantViewModel {
                    Id = varnt.Id,
                    EntYearId = varnt.EntYearId,
                    SubjectId = varnt.SubjectId,
                    Title = varnt.Title,
                    NumberOfQuestions = varnt.EntVariantsToQuestions.Count(),
                    KnowingPercentage = usersToEntVariants.FirstOrDefault(utv => utv.EntVariantId == varnt.Id)?.CurrentLearningRate ?? 0,
                    NumberOfCorrectAnswers = usersToEntVariants.FirstOrDefault(utv => utv.EntVariantId == varnt.Id)?.NumberOfCorrectAnswers ?? 0
                };

                model.Add(item);
            }

            return model;
        }

        [HttpGet]
        [Route("QuestionsByVariant")]
        public async Task<CustomerLearnosityEntQuestionViewModel> QuestionsByVariant(int variantId, bool isReview = false)
        {
            var model = new CustomerLearnosityEntQuestionViewModel { QuestionAnswers = new List<CustomerEntQuestionAnswerViewModel>() };
            var userId = User.Identity.GetUserId();
            var questionIds = await db.EntVariantsToQuestions.
                Where(ev => ev.EntVariantId == variantId).
                OrderBy(ev => ev.Question.QuestionDifficultyId).
                ThenBy(ev => ev.Question.Id).
                Select(ev => ev.QuestionId).ToListAsync();

            if (isReview == true)
            {
                var userToVariant = await db.UsersToEntVariants.FirstOrDefaultAsync(utv => utv.UserId == userId && utv.EntVariantId == variantId);
                if (userToVariant != null)
                {
                    model.KnowingPercentage = userToVariant.CurrentLearningRate;
                    var questionAnswers = await db.QuestionAnswers.
                        Include(qa => qa.Question).
                        Include(qa => qa.Question.QuestionsToThemes).
                        Include(qa => qa.Question.QuestionsToThemes.Select(qtt => qtt.Theme)).
                        Include(qa => qa.Question.Solutions).
                        Include(qa => qa.QuestionAnswerLrnVariants).
                        //Include(qa => qa.QuestionAnswerVariants).
                        //Include(qa => qa.QuestionAnswerVariants.Select(qav => qav.QuestionVariant)).
                        Where(qa => qa.UserId == userId && questionIds.Contains(qa.QuestionId)).
                        ToListAsync();

                    var grouping = questionAnswers.GroupBy(qa => qa.QuestionId).ToList();
                    foreach (var qa in grouping)
                    {
                        var maxAnswer = qa.OrderByDescending(a => a.Id).FirstOrDefault();
                        if (maxAnswer != null)
                        {
                            model.QuestionAnswers.Add(new CustomerEntQuestionAnswerViewModel
                            {
                                QuestionId = maxAnswer.QuestionId,
                                IsCorrect = maxAnswer.IsCorrect ?? false,
                                AnsweredValue = maxAnswer.QuestionAnswerLrnVariants.Count == 0 ? maxAnswer.Value : string.Join(", ", maxAnswer.QuestionAnswerLrnVariants.Select(qav => qav.VariantValue)),
                                CorrectAnswerValue = maxAnswer.Question.CorrectAnswerValue,
                                Solution = maxAnswer.Question.Solutions.Count == 0 ? "" : maxAnswer.Question.Solutions.FirstOrDefault().Text,
                                Themes = maxAnswer.Question.QuestionsToThemes.Select(qtt => new Theme { Id = qtt.Theme.Id, Title = qtt.Theme.Title }).ToList()
                            });
                        }
                    }
                }
            }

            var questions = db.Questions.Where(q => questionIds.Contains(q.Id)).Select(q => new
            {
                id = q.Id,
                text = q.Text,
                questionTypeId = q.QuestionTypeId,
                questionDifficultyId = q.QuestionDifficultyId,
                dateCreated = q.DateCreated,
                correctAnswerValue = q.CorrectAnswerValue,
                questionType = new { id = q.QuestionType.Id, title = q.QuestionType.Title },
                questionDifficulty = new { id = q.QuestionDifficulty.Id, title = q.QuestionDifficulty.Title, value = q.QuestionDifficulty.Value },
                questionsToThemes = q.QuestionsToThemes.Select(qtt => new { questionId = qtt.QuestionId, themeId = qtt.ThemeId, theme = new { id = qtt.Theme.Id, title = qtt.Theme.Title } }).ToList(),
                questionVariants = q.QuestionVariants.Select(qv => new { id = qv.Id, text = qv.Text, questionId = qv.QuestionId, isCorrect = qv.IsCorrect }).ToList(),
                solutions = q.Solutions.Select(s => new { id = s.Id, text = s.Text, questionId = s.QuestionId }).ToList(),
                jsonData = q.JsonData
            }).AsEnumerable().Select(q => new QuestionViewModel
            {
                Id = q.id,
                Text = q.text,
                QuestionTypeId = q.questionTypeId,
                QuestionDifficultyId = q.questionDifficultyId,
                DateCreated = q.dateCreated,
                CorrectAnswerValue = q.correctAnswerValue,
                QuestionType = new QuestionType { Id = q.questionType.id, Title = q.questionType.title },
                QuestionDifficulty = new QuestionDifficulty { Id = q.questionDifficulty.id, Title = q.questionDifficulty.title, Value = q.questionDifficulty.value },
                QuestionsToThemes = q.questionsToThemes.Select(qtt => new QuestionsToThemes { QuestionId = qtt.questionId, ThemeId = qtt.themeId, Theme = new Theme { Id = qtt.theme.id, Title = qtt.theme.title } }).ToList(),
                QuestionVariants = q.questionVariants.Select(qv => new QuestionVariant { Id = qv.id, Text = qv.text, QuestionId = qv.questionId, IsCorrect = qv.isCorrect }).ToList(),
                Solutions = q.solutions.Select(s => new Solution { Id = s.id, Text = s.text, QuestionId = s.questionId }).ToList(),
                JsonData = q.jsonData
            }).ToList();

            var uuid = "";

            //foreach (var question in questions)
            //{
            //    var listToSend = new List<QuestionViewModel>();
            //    listToSend.Add(question);
            //    var json = LQuestions.Simple(listToSend, out uuid, isReview);
            //    model.LearnosityJson.Add(json);
            //}

            var json = LQuestions.Simple(questions, out uuid, isReview);
            model.LearnosityJson = json;

            return model;
        }

        [HttpPost]
        [Route("PostEntAnswers")]
        public async Task<IEnumerable<Theme>> PostEntAnswers(List<QuestionAnswer> model, int variantId)
        {
            var userId = User.Identity.GetUserId();
            var dt = DateTime.Now;
            var notLearnedThemes = new List<Theme>();

            var variantsToQuestions = await db.EntVariantsToQuestions.
                Include(evq => evq.Question).
                Include(evq => evq.Question.QuestionDifficulty).
                Where(evq => evq.EntVariantId == variantId).
                ToListAsync();

            var totalDiffs = variantsToQuestions.Sum(vtq => vtq.Question.QuestionDifficulty.Value);
            var correctsDiffs = 0.0;
            var numberOfCorrectAnsw = 0;

            foreach (var answ in model)
            {               
                var qa = new QuestionAnswer
                {
                    QuestionId = answ.QuestionId,
                    UserId = userId,
                    Value = answ.Value,
                    QuestionAnswerLrnVariants = new List<QuestionAnswerLrnVariant>(),
                    DateCreated = dt,
                    AnswerSourceId = 4,
                    SolvingSeconds = answ.SolvingSeconds,
                    IsCorrect = answ.IsCorrect
                };

                if (answ.QuestionAnswerVariants != null)
                {
                    foreach (var variant in answ.QuestionAnswerVariants)
                    {
                        qa.QuestionAnswerLrnVariants.Add(new QuestionAnswerLrnVariant { VariantValue = variant.QuestionVariantId });
                    }
                }

                if (answ.IsCorrect == true)
                {
                    numberOfCorrectAnsw++;

                    var vtq = variantsToQuestions.FirstOrDefault(q => q.QuestionId == answ.QuestionId);
                    if (vtq != null)
                        correctsDiffs += vtq.Question.QuestionDifficulty.Value;
                }
                else
                {
                    notLearnedThemes.AddRange(await db.QuestionsToThemes./*Include(qtt => qtt.Theme).*/Where(qtt => qtt.QuestionId == answ.QuestionId).Select(qtt => qtt.Theme).ToListAsync());
                }

                db.QuestionAnswers.Add(qa);
            }
                        
            var userToVariant = await db.UsersToEntVariants.FirstOrDefaultAsync(utv => utv.UserId == userId && utv.EntVariantId == variantId);
            if (userToVariant == null)
            {
                var newUtv = new UsersToEntVariants {
                    UserId = userId,
                    EntVariantId = variantId,
                    IsCompleted = true,
                    CurrentLearningRate = (double)correctsDiffs / totalDiffs * 100,
                    NumberOfCorrectAnswers = numberOfCorrectAnsw
                };

                db.UsersToEntVariants.Add(newUtv);
            }
            else
            {
                userToVariant.CurrentLearningRate = (double)correctsDiffs / totalDiffs * 100;
                userToVariant.NumberOfCorrectAnswers = numberOfCorrectAnsw;
            }

            await db.SaveChangesAsync();

            return notLearnedThemes;
        }

        [HttpGet]
        [Route("ThemesWithGapsByVariantId")]
        public async Task<IEnumerable<Theme>> ThemesWithGapsByVariantId(int variantId)
        {
            var userId = User.Identity.GetUserId();
            var notLearnedThemes = new List<Theme>();

            var variant = await db.EntVariants.Include(e => e.EntVariantsToQuestions).FirstOrDefaultAsync(e => e.Id == variantId);
            if (variant == null)
                return notLearnedThemes;
            var qIds = variant.EntVariantsToQuestions.Select(e => e.QuestionId).ToList();
            var answers = await db.QuestionAnswers.
                Include(qa => qa.Question).
                Include(qa => qa.Question.QuestionsToThemes).
                Include(qa => qa.Question.QuestionsToThemes.Select(qtt => qtt.Theme)).
                Where(qa => qIds.Contains(qa.QuestionId) && qa.UserId == userId).ToListAsync();
            var grouping = answers.GroupBy(qa => qa.QuestionId).ToList();
            foreach (var g in grouping)
            {
                var maxAnswer = g.OrderByDescending(qa => qa.Id).FirstOrDefault();

                if (maxAnswer.IsCorrect == false)
                {
                    notLearnedThemes.AddRange(maxAnswer.Question.QuestionsToThemes.Select(qtt => new Theme { Id = qtt.Theme.Id, Title = qtt.Theme.Title }));
                }
            }

            return notLearnedThemes;
        }

        [HttpGet]
        [Route("EntVariantStats")]
        public async Task<CheckingTestStatsViewModel> EntVariantStats(int variantId)
        {
            var userId = User.Identity.GetUserId();
            var model = new CheckingTestStatsViewModel();

            var usersToEntVariants = await db.UsersToEntVariants.
                Include(utv => utv.EntVariant).
                Include(utv => utv.EntVariant.EntVariantsToQuestions).
                Include(utv => utv.EntVariant.EntVariantsToQuestions.Select(etq => etq.Question)).
                Include(utv => utv.EntVariant.EntVariantsToQuestions.Select(etq => etq.Question.QuestionDifficulty)).
                FirstOrDefaultAsync(utv => utv.UserId == userId && utv.EntVariantId == variantId);
            
            model.ThemeKnowledgePercent = usersToEntVariants.CurrentLearningRate;
            model.QuestionsNumber = usersToEntVariants.EntVariant.EntVariantsToQuestions.Count;
            model.CorrectAnswersNumber = usersToEntVariants.NumberOfCorrectAnswers;

            var questionIds = usersToEntVariants.EntVariant.EntVariantsToQuestions.
                Select(ut => ut.QuestionId).ToList();

            var totalExp = 0.0;

            var questionAnswers = await db.QuestionAnswers.
                Include(qa => qa.Question).
                Include(qa => qa.Question.QuestionDifficulty).
                Where(qa => qa.UserId == userId && questionIds.Contains(qa.QuestionId)).
                ToListAsync();

            var grouping = questionAnswers.GroupBy(qa => qa.QuestionId).ToList();
            foreach (var qa in grouping)
            {
                var maxAnswer = qa.OrderByDescending(a => a.Id).FirstOrDefault();
                if (maxAnswer != null)
                {
                    totalExp += maxAnswer.IsCorrect == true ? maxAnswer.Question.QuestionDifficulty.Value : 0;
                }
            }

            model.TotalExperience = totalExp;

            return model;
        }
    }
}
