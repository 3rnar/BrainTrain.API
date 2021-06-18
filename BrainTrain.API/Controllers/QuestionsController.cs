using BrainTrain.API.Helpers.Learnosity;
using BrainTrain.Core.Models;
using BrainTrain.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер,Заполнение вопросов")]
    public class QuestionsController : BaseApiController
    {
        public QuestionsController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/Questions
        [HttpGet]
        [Route("api/Questions")]
        public QuestionsPagingViewModel GetQuestions(int pageNum, int perPage, int? themeId, int? subjectId, int? questionTypeId, string searchStr, string userId, int? moduleId, bool? isModule, int? gradeId)
        {
            var qs = db.Questions.Where(q => 
                (string.IsNullOrEmpty(searchStr) || q.Text.ToLower().Contains(searchStr.ToLower().Trim())) && 
                (themeId == null || q.QuestionsToThemes.Any(mtt => mtt.ThemeId == themeId)) &&
                (subjectId == null || q.QuestionsToThemes.Any(qtt => qtt.Theme.SubjectId == subjectId)) &&
                (questionTypeId == null || q.QuestionTypeId == questionTypeId) &&
                (string.IsNullOrEmpty(userId) || q.ContentManagerId == userId) &&
                (moduleId == null || q.QuestionsToThemes.Any(qtt => qtt.Theme.ThemesToModules.Any(ttm => ttm.ModuleId == moduleId && ttm.IsDominant == true))) &&
                (isModule == null || q.IsModuleQuestion == true) &&
                (gradeId == null || q.QuestionsToThemes.Any(qtt => qtt.Theme.ThemesToModules.Any(ttm => ttm.Theme.GradeId == gradeId)))
                
            ).OrderByDescending(m => m.Id).Skip((pageNum - 1) * perPage).Take(perPage).Select(q => new
            {
                id = q.Id,
                contentManagerId = q.ContentManagerId,
                text = q.Text,
                questionTypeId = q.QuestionTypeId,
                questionDifficultyId = q.QuestionDifficultyId,
                dateCreated = q.DateCreated,
                correctAnswerValue = q.CorrectAnswerValue,
                isChecked = q.IsChecked,
                isEnt = q.IsEnt,
                isModuleQuestion = q.IsModuleQuestion,
                questionType = new { id = q.QuestionType.Id, title = q.QuestionType.Title },
                questionDifficulty = new { id = q.QuestionDifficulty.Id, title = q.QuestionDifficulty.Title, value = q.QuestionDifficulty.Value },
                questionsToThemes = q.QuestionsToThemes.Select(qtt => new { questionId = qtt.QuestionId, themeId = qtt.ThemeId, theme = new { id = qtt.Theme.Id, title = qtt.Theme.Title } }).ToList(),
                questionVariants = q.QuestionVariants.Select(qv => new { id = qv.Id, text = qv.Text, questionId = qv.QuestionId, isCorrect = qv.IsCorrect }).ToList(),
                solutions = q.Solutions.Select(s => new { id = s.Id, text = s.Text }),
                jsonData = q.JsonData
            }).AsEnumerable().Select(q => new Question
            {
                Id = q.id,
                ContentManagerId = q.contentManagerId,
                Text = q.text,
                QuestionTypeId = q.questionTypeId,
                QuestionDifficultyId = q.questionDifficultyId,
                DateCreated = q.dateCreated,
                CorrectAnswerValue = q.correctAnswerValue,
                IsEnt = q.isEnt,
                IsChecked = q.isChecked,
                IsModuleQuestion = q.isModuleQuestion,
                QuestionType = new QuestionType { Id = q.questionType.id, Title = q.questionType.title },
                QuestionDifficulty = new QuestionDifficulty { Id = q.questionDifficulty.id, Title = q.questionDifficulty.title, Value = q.questionDifficulty.value },
                QuestionsToThemes = q.questionsToThemes.Select(qtt => new QuestionsToThemes { QuestionId = qtt.questionId, ThemeId = qtt.themeId, Theme = new Theme { Id = qtt.theme.id, Title = qtt.theme.title } }).ToList(),
                QuestionVariants = q.questionVariants.Select(qv => new QuestionVariant { Id = qv.id, Text = qv.text, QuestionId = qv.questionId, IsCorrect = qv.isCorrect }).ToList(),
                Solutions = q.solutions.Select(s => new Solution { Id = s.id, Text = s.text }).ToList(),
                JsonData =q.jsonData
            }).ToList();

            var qModel = new QuestionsPagingViewModel
            {
                Questions = qs,
                QuestionsCount = db.Questions.Where(q => string.IsNullOrEmpty(searchStr) || q.Text.ToLower().Contains(searchStr.ToLower().Trim()) 
                && (themeId == null || q.QuestionsToThemes.Any(mtt => mtt.ThemeId == themeId)) 
                && (subjectId == null || q.QuestionsToThemes.Any(qtt => qtt.Theme.SubjectId == subjectId)) 
                && (questionTypeId == null || q.QuestionTypeId == questionTypeId) &&
                (string.IsNullOrEmpty(userId) || q.ContentManagerId == userId) &&
                (moduleId == null || q.QuestionsToThemes.Any(qtt => qtt.Theme.ThemesToModules.Any(ttm => ttm.ModuleId == moduleId && ttm.IsDominant == true))) &&
                (isModule == null || q.IsModuleQuestion == true) &&
                (gradeId == null || q.QuestionsToThemes.Any(qtt => qtt.Theme.ThemesToModules.Any(ttm => ttm.Theme.GradeId == gradeId)))
                ).Count()
            };
            
            return qModel;
        }

        [HttpGet]
        [Route("api/Questions/CheckQuestion/{id:int}")]
        public IActionResult CheckQuestion(int id)
        {
            var question = db.Questions.FirstOrDefault(q => q.Id == id);
            if (question != null)
            {
                if (question.IsChecked == null || question.IsChecked == false)
                    question.IsChecked = true;
                else
                    question.IsChecked = false;

                db.SaveChanges();
            }

            return Ok();
        }

        // GET: api/Questions/5
        [HttpGet]
        [Route("api/Questions/{id:int}")]
        public async Task<IActionResult> GetQuestion(int id)
        {
            Question question = await db.Questions.          
                Include(q => q.QuestionType).
                Include(q => q.QuestionDifficulty).
                Include(q => q.QuestionsToThemes).
                Include(q => q.QuestionVariants).
                Include(q => q.Solutions) . FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        [HttpGet]
        [Route("api/Questions/Lrn/{id:int}")]
        public async Task<IActionResult> GetLRNQuestion(int id)
        {
            var lRNQuestion = await db.Questions.Where(q => q.Id == id).ToListAsync();

            string uuid = "";

            var qJson = LRNQuestionsHelper.Simple(lRNQuestion, out uuid);

            return Ok(qJson);
        }

        // PUT: api/Questions/5
        [HttpPut]
        [Route("api/Questions/{id:int}")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {

            if (id != question.Id)
            {
                return BadRequest();
            }

            var dbQuestion = await db.Questions.Include(q => q.QuestionsToThemes).Include(q => q.QuestionVariants).Include(q => q.Solutions).FirstOrDefaultAsync(q => q.Id == id);
            if (dbQuestion == null)
                return BadRequest();

            //THEMES
            if (dbQuestion.QuestionsToThemes.Count > 0)
            {
                var toDel =
                    dbQuestion.QuestionsToThemes.Where(
                        mtt =>
                            !question.QuestionsToThemes.Any(
                                qtt =>
                                    qtt.ThemeId == mtt.ThemeId &&
                                    qtt.QuestionId == mtt.QuestionId)).ToList();
                db.QuestionsToThemes.RemoveRange(toDel);
            }
            foreach (var questionsToThemes in question.QuestionsToThemes)
            {
                if (
                    !dbQuestion.QuestionsToThemes.Any(
                        mtt => mtt.ThemeId == questionsToThemes.ThemeId && mtt.QuestionId == questionsToThemes.QuestionId))
                {
                    db.QuestionsToThemes.Add(questionsToThemes);
                }
            }

            //Solutions
            if (question.Solutions != null && question.Solutions.Count > 0)
            {
                if (dbQuestion.Solutions != null && dbQuestion.Solutions.Count > 0)
                {
                    if (dbQuestion.Solutions.FirstOrDefault().Text != question.Solutions.FirstOrDefault().Text)
                    {
                        dbQuestion.Solutions.FirstOrDefault().Text = question.Solutions.FirstOrDefault().Text;
                    }
                }
                else
                {
                    db.Solutions.Add(new Solution { QuestionId = question.Id, Text = question.Solutions.FirstOrDefault().Text, ContentManagerId = UserId });
                }
            }
            else
            {
                if (dbQuestion.Solutions != null && dbQuestion.Solutions.Count > 0)
                {
                    db.Solutions.Remove(dbQuestion.Solutions.FirstOrDefault());
                }
            }

            //question variants
            if (question.QuestionVariants != null && question.QuestionVariants.Count > 0)
            {
                if (dbQuestion.QuestionVariants != null && dbQuestion.QuestionVariants.Count > 0)
                {
                    foreach (var qv in question.QuestionVariants)
                    {
                        if (dbQuestion.QuestionVariants.Any(a => a.Id == qv.Id))
                        {
                            var dbQv = dbQuestion.QuestionVariants.FirstOrDefault(a => a.Id == qv.Id);
                            if (qv.Id == dbQv.Id)
                            {
                                if (qv.Text != dbQv.Text)
                                {
                                    dbQv.Text = qv.Text;
                                }
                                if (qv.IsCorrect != dbQv.IsCorrect)
                                {
                                    dbQv.IsCorrect = qv.IsCorrect;
                                }
                            }
                        }
                        else
                        {
                            db.QuestionVariants.Add(qv);
                        }
                    }

                    var variantsToDel = new List<QuestionVariant>();

                    foreach (var dbQv in dbQuestion.QuestionVariants)
                    {
                        if (question.QuestionVariants.Any(a => a.Id == dbQv.Id) == false)
                        {
                            variantsToDel.Add(dbQv);
                        }
                    }

                    if (variantsToDel.Count > 0)
                    {
                        db.QuestionVariants.RemoveRange(variantsToDel);
                    }
                }
                else
                {
                    db.QuestionVariants.AddRange(question.QuestionVariants);
                }
            }
            else
            {
                if (dbQuestion.QuestionVariants != null && dbQuestion.QuestionVariants.Count > 0)
                {
                    db.QuestionVariants.RemoveRange(dbQuestion.QuestionVariants);
                }
            }



            await db.SaveChangesAsync();     

            db.Entry(dbQuestion).State = EntityState.Detached;

            question.QuestionsToThemes = null;
            question.QuestionVariants = null;
            question.Solutions = null;

            try
            {
                db.Entry(question).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {

            }

            return NoContent();
        }

        [HttpPost]
        [Route("api/Questions/PostLrn")]
        public async Task<IActionResult> PostLrn(LrnQuestionsEditViewModel model)
        {
            var question = db.Questions.FirstOrDefault(q => q.Id == model.Id);
            var qtts = db.QuestionsToThemes.Where(q => q.QuestionId == model.Id).ToList();
            var solution = db.Solutions.FirstOrDefault(s => s.QuestionId == model.Id);
            if (question == null)
                return NotFound();
                
    		var details = JObject.Parse(model.LrnJson);
    		var jsonQuestion = details["questions"][0] as JObject;
            jsonQuestion.Remove("response_id");
            jsonQuestion.Remove("questionId");
    		
    		var str = jsonQuestion.ToString();
    		
    		var first = str.IndexOf("{");
    		var last = str.LastIndexOf("}");
    		str = str.Remove(last, 1).Remove(first, 1);

            question.JsonData = str;
            question.Text = jsonQuestion["stimulus"].ToString();
            question.IsEnt = model.IsEnt;
            question.IsModuleQuestion = model.IsModuleQuestion;
            question.QuestionDifficultyId = model.QuestionDifficultyId;
            question.QuestionTypeId = model.QuestionTypeId;

            if (solution == null)
            {
                db.Solutions.Add(new Solution { Text = model.Solution, ContentManagerId = UserId, QuestionId = model.Id });
            }
            else
            {
                solution.Text = model.Solution;
            }

            if (model.ThemeIds.Count > 0)
            {
                var toDel =
                    qtts.Where(
                        mtt =>
                            !model.ThemeIds.Any(
                                qtt => qtt == mtt.ThemeId)).ToList();
                db.QuestionsToThemes.RemoveRange(toDel);
            }
            foreach (var tid in model.ThemeIds)
            {
                if (
                    !qtts.Any(
                        mtt => mtt.ThemeId == tid))
                {
                    db.QuestionsToThemes.Add(new QuestionsToThemes { QuestionId = model.Id, ThemeId = tid });
                }
            }
            
            db.SaveChanges();

            return Ok();
        }

        // POST: api/Questions
        [HttpPost]
        [Route("api/Questions", Name = "PostQuestion")]
        public async Task<IActionResult> PostQuestion(Question question)
        {
            //if (!ModelState.IsValid)
            //{
                //return BadRequest(ModelState);
            //}

            question.ContentManagerId = UserId;
            question.DateCreated = DateTime.Now;

            if (question.Solutions != null)
            {
                foreach (var sol in question.Solutions)
                {
                    sol.ContentManagerId = UserId;
                }
            }

            db.Questions.Add(question);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostQuestion", new { id = question.Id }, question);
        }

        // DELETE: api/Questions/5
        [HttpDelete]
        [Route("api/Questions/{id:int}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            Question question = await db.Questions.Include(q => q.Solutions).Include(q => q.QuestionVariants).Include(q => q.QuestionsToThemes).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            if (question.Solutions != null)
            {
                db.Solutions.RemoveRange(question.Solutions);
            }

            if (question.QuestionVariants != null)
            {
                db.QuestionVariants.RemoveRange(question.QuestionVariants);
            }
            if (question.QuestionsToThemes != null)
            {
                db.QuestionsToThemes.RemoveRange(question.QuestionsToThemes);
            }
            

            db.Questions.Remove(question);
            await db.SaveChangesAsync();

            return Ok(question);
        }


        private bool QuestionExists(int id)
        {
            return db.Questions.Count(e => e.Id == id) > 0;
        }
    }
}