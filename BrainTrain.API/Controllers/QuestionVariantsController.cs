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
    [Authorize(Roles = "Контент-менеджер,Заполнение вопросов")]
    public class QuestionVariantsController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/QuestionVariants
        [HttpGet]
        [Route("api/QuestionVariants")]
        public IQueryable<QuestionVariant> GetQuestionVariants()
        {
            return db.QuestionVariants;
        }

        // GET: api/QuestionVariants/5
        [HttpGet]
        [ResponseType(typeof(QuestionVariant))]
        [Route("api/QuestionVariants/{id:int}")]
        public async Task<IHttpActionResult> GetQuestionVariant(int id)
        {
            QuestionVariant questionVariant = await db.QuestionVariants.FindAsync(id);
            if (questionVariant == null)
            {
                return NotFound();
            }

            return Ok(questionVariant);
        }

        // PUT: api/QuestionVariants/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/QuestionVariants/{id:int}")]
        public async Task<IHttpActionResult> PutQuestionVariant(int id, QuestionVariant questionVariant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != questionVariant.Id)
            {
                return BadRequest();
            }

            db.Entry(questionVariant).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionVariantExists(id))
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

        // POST: api/QuestionVariants
        [ResponseType(typeof(QuestionVariant))]
        [HttpPost]
        [Route("api/QuestionVariants", Name = "PostQuestionVariant")]
        public async Task<IHttpActionResult> PostQuestionVariant(QuestionVariant questionVariant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.QuestionVariants.Add(questionVariant);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostQuestionVariant", new { id = questionVariant.Id }, questionVariant);
        }

        // DELETE: api/QuestionVariants/5
        [ResponseType(typeof(QuestionVariant))]
        [HttpDelete]
        [Route("api/QuestionVariants/{id:int}")]
        public async Task<IHttpActionResult> DeleteQuestionVariant(int id)
        {
            QuestionVariant questionVariant = await db.QuestionVariants.FindAsync(id);
            if (questionVariant == null)
            {
                return NotFound();
            }

            db.QuestionVariants.Remove(questionVariant);
            await db.SaveChangesAsync();

            return Ok(questionVariant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionVariantExists(int id)
        {
            return db.QuestionVariants.Count(e => e.Id == id) > 0;
        }
    }
}