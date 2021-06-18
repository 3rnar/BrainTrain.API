using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BrainTrain.API.Helpers.Learnosity;
using BrainTrain.Core.Models;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер,Заполнение вопросов")]
    public class LRNQuestionsController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/LRNQuestions
        [HttpGet]
        [Route("api/LRNQuestions")]
        public IQueryable<LRNQuestion> GetLRNQuestions()
        {
            return db.LRNQuestions;
        }

        [HttpGet]
        [Route("api/LRNQuestions/Authoring")]
        public async Task<IHttpActionResult> Authoring()
        {
            var json = LRNAuthoringHelper.Simple("item_edit");
            return Ok(json);
        }


        // GET: api/LRNQuestions/5
        [HttpGet]
        [Route("api/LRNQuestions/{id:int}")]
        [ResponseType(typeof(LRNQuestion))]
        public async Task<IHttpActionResult> GetLRNQuestion(int id)
        {
            var lRNQuestion = await db.LRNQuestions.Where(q => q.Id == id).ToListAsync();

            string uuid = "";

            var qJson = LRNQuestionsHelper.Simple(lRNQuestion, out uuid);

            return Ok(qJson);
        }

        // PUT: api/LRNQuestions/5
        [HttpPut]
        [Route("api/LRNQuestions/{id:int}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLRNQuestion(int id, LRNQuestion lRNQuestion)
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/LRNQuestions
        [HttpPost]
        [Route("api/LRNQuestions", Name = "PostLRNQuestion")]
        [ResponseType(typeof(LRNQuestion))]
        public async Task<IHttpActionResult> PostLRNQuestion(LRNQuestion lRNQuestion)
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
        [ResponseType(typeof(LRNQuestion))]
        public async Task<IHttpActionResult> DeleteLRNQuestion(int id)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LRNQuestionExists(int id)
        {
            return db.LRNQuestions.Count(e => e.Id == id) > 0;
        }
    }
}