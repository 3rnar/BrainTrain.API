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
using BrainTrain.Core.Models;

namespace BrainTrain.API.Controllers
{
    public class QuestionDifficultiesController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/QuestionDifficulties
        [Authorize(Roles = "Контент-менеджер,Заполнение вопросов")]
        [HttpGet]
        [Route("api/QuestionDifficulties")]
        public IQueryable<QuestionDifficulty> GetQuestionDifficulties()
        {
            return db.QuestionDifficulties;
        }

        // GET: api/QuestionDifficulties/5
        [Authorize(Roles = "Контент-менеджер")]
        [ResponseType(typeof(QuestionDifficulty))]
        [HttpGet]
        [Route("api/QuestionDifficulties/{id:int}")]
        public async Task<IHttpActionResult> GetQuestionDifficulty(int id)
        {
            QuestionDifficulty questionDifficulty = await db.QuestionDifficulties.FindAsync(id);
            if (questionDifficulty == null)
            {
                return NotFound();
            }

            return Ok(questionDifficulty);
        }

        // PUT: api/QuestionDifficulties/5
        [Authorize(Roles = "Контент-менеджер")]
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/QuestionDifficulties/{id:int}")]
        public async Task<IHttpActionResult> PutQuestionDifficulty(int id, QuestionDifficulty questionDifficulty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != questionDifficulty.Id)
            {
                return BadRequest();
            }

            db.Entry(questionDifficulty).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionDifficultyExists(id))
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

        // POST: api/QuestionDifficulties
        [Authorize(Roles = "Контент-менеджер")]
        [ResponseType(typeof(QuestionDifficulty))]
        [HttpPost]
        [Route("api/QuestionDifficulties", Name = "PostQuestionDifficulty")]
        public async Task<IHttpActionResult> PostQuestionDifficulty(QuestionDifficulty questionDifficulty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.QuestionDifficulties.Add(questionDifficulty);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostQuestionDifficulty", new { id = questionDifficulty.Id }, questionDifficulty);
        }

        // DELETE: api/QuestionDifficulties/5
        [Authorize(Roles = "Контент-менеджер")]
        [ResponseType(typeof(QuestionDifficulty))]
        [HttpDelete]
        [Route("api/QuestionDifficulties/{id:int}")]
        public async Task<IHttpActionResult> DeleteQuestionDifficulty(int id)
        {
            QuestionDifficulty questionDifficulty = await db.QuestionDifficulties.FindAsync(id);
            if (questionDifficulty == null)
            {
                return NotFound();
            }

            db.QuestionDifficulties.Remove(questionDifficulty);
            await db.SaveChangesAsync();

            return Ok(questionDifficulty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionDifficultyExists(int id)
        {
            return db.QuestionDifficulties.Count(e => e.Id == id) > 0;
        }
    }
}