using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер")]
    public class EntrantTestQuestionsController : BaseApiController
    {
        public EntrantTestQuestionsController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/EntrantTestQuestions
        [HttpGet]
        [Route("api/EntrantTestQuestions/GetBySubject/{subjectId:int}")]
        public IEnumerable<EntrantTestQuestion> GetEntrantTestQuestions(int subjectId)
        {
            var qs = db.EntrantTestQuestions.Where(etq => etq.SubjectId == subjectId).Select(q => new {
                id = q.Id,
                subjectId = q.SubjectId,
                questionId = q.QuestionId,
                question = new { id = q.Question.Id, text = q.Question.Text },
                entrantTestQuestionsToThemes = q.EntrantTestQuestionsToThemes.Select(tq => new { entrantTestQuestionId = tq.EntrantTestQuestionId, themeId = tq.ThemeId, theme = new { id = tq.Theme.Id, title = tq.Theme.Title } }).ToList()
            }).AsEnumerable().Select(q => new EntrantTestQuestion {
                Id = q.id,
                SubjectId = q.subjectId,
                QuestionId = q.questionId,
                Question = new Question { Id = q.question.id, Text = q.question.text },
                EntrantTestQuestionsToThemes = q.entrantTestQuestionsToThemes.Select(tq => new EntrantTestQuestionsToThemes { EntrantTestQuestionId = tq.entrantTestQuestionId, ThemeId = tq.themeId, Theme = new Theme { Id = tq.theme.id, Title = tq.theme.title } }).ToList()
            }).ToList();
            return qs;
        }

        // GET: api/EntrantTestQuestions/5
        [HttpGet]
        [Route("api/EntrantTestQuestions/{id:int}")]
        public async Task<IActionResult> GetEntrantTestQuestion(int id)
        {
            EntrantTestQuestion entrantTestQuestion = await db.EntrantTestQuestions.FindAsync(id);
            if (entrantTestQuestion == null)
            {
                return NotFound();
            }

            return Ok(entrantTestQuestion);
        }

        // PUT: api/EntrantTestQuestions/5
        [HttpPut]
        [Route("api/EntrantTestQuestions/{id:int}")]
        public async Task<IActionResult> PutEntrantTestQuestion(int id, EntrantTestQuestion entrantTestQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entrantTestQuestion.Id)
            {
                return BadRequest();
            }

            db.Entry(entrantTestQuestion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntrantTestQuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EntrantTestQuestions
        [HttpPost]
        [Route("api/EntrantTestQuestions", Name = "PostEntrantTestQuestion")]
        public async Task<IActionResult> PostEntrantTestQuestion(EntrantTestQuestion entrantTestQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EntrantTestQuestions.Add(entrantTestQuestion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostEntrantTestQuestion", new { id = entrantTestQuestion.Id }, entrantTestQuestion);
        }

        // DELETE: api/EntrantTestQuestions/5
        [HttpDelete]
        [Route("api/EntrantTestQuestions/{id:int}")]
        public async Task<IActionResult> DeleteEntrantTestQuestion(int id)
        {
            EntrantTestQuestion entrantTestQuestion = await db.EntrantTestQuestions.FindAsync(id);
            if (entrantTestQuestion == null)
            {
                return NotFound();
            }

            db.EntrantTestQuestions.Remove(entrantTestQuestion);
            await db.SaveChangesAsync();

            return Ok(entrantTestQuestion);
        }


        private bool EntrantTestQuestionExists(int id)
        {
            return db.EntrantTestQuestions.Any(e => e.Id == id);
        }
    }
}