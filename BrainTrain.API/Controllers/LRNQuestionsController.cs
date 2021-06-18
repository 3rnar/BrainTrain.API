using BrainTrain.API.Helpers.Learnosity;
using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер,Заполнение вопросов")]
    public class LRNQuestionsController : BaseApiController
    {
        public LRNQuestionsController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/LRNQuestions
        [HttpGet]
        [Route("api/LRNQuestions")]
        public IQueryable<LRNQuestion> GetLRNQuestions()
        {
            return db.LRNQuestions;
        }

        [HttpGet]
        [Route("api/LRNQuestions/Authoring")]
        public async Task<IActionResult> Authoring()
        {
            var json = LRNAuthoringHelper.Simple("item_edit");
            return Ok(json);
        }


        // GET: api/LRNQuestions/5
        [HttpGet]
        [Route("api/LRNQuestions/{id:int}")]
        public async Task<IActionResult> GetLRNQuestion(int id)
        {
            var lRNQuestion = await db.LRNQuestions.Where(q => q.Id == id).ToListAsync();

            string uuid = "";

            var qJson = LRNQuestionsHelper.Simple(lRNQuestion, out uuid);

            return Ok(qJson);
        }

        // PUT: api/LRNQuestions/5
        [HttpPut]
        [Route("api/LRNQuestions/{id:int}")]
        public async Task<IActionResult> PutLRNQuestion(int id, LRNQuestion lRNQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lRNQuestion.Id)
            {
                return BadRequest();
            }

            db.Entry(lRNQuestion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LRNQuestionExists(id))
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

        // POST: api/LRNQuestions
        [HttpPost]
        [Route("api/LRNQuestions", Name = "PostLRNQuestion")]
        public async Task<IActionResult> PostLRNQuestion(LRNQuestion lRNQuestion)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            lRNQuestion.DateCreated = DateTime.Now;

            db.LRNQuestions.Add(lRNQuestion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostLRNQuestion", new { id = lRNQuestion.Id }, lRNQuestion);
        }

        // DELETE: api/LRNQuestions/5
        [HttpDelete]
        [Route("api/LRNQuestions/{id:int}")]
        public async Task<IActionResult> DeleteLRNQuestion(int id)
        {
            LRNQuestion lRNQuestion = await db.LRNQuestions.FindAsync(id);
            if (lRNQuestion == null)
            {
                return NotFound();
            }

            db.LRNQuestions.Remove(lRNQuestion);
            await db.SaveChangesAsync();

            return Ok(lRNQuestion);
        }

        private bool LRNQuestionExists(int id)
        {
            return db.LRNQuestions.Count(e => e.Id == id) > 0;
        }
    }
}