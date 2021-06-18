using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер,Заполнение вопросов")]
    public class QuestionVariantsController : BaseApiController
    {
        public QuestionVariantsController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/QuestionVariants
        [HttpGet]
        [Route("api/QuestionVariants")]
        public IQueryable<QuestionVariant> GetQuestionVariants()
        {
            return db.QuestionVariants;
        }

        // GET: api/QuestionVariants/5
        [HttpGet]
        [Route("api/QuestionVariants/{id:int}")]
        public async Task<IActionResult> GetQuestionVariant(int id)
        {
            QuestionVariant questionVariant = await db.QuestionVariants.FindAsync(id);
            if (questionVariant == null)
            {
                return NotFound();
            }

            return Ok(questionVariant);
        }

        // PUT: api/QuestionVariants/5
        [HttpPut]
        [Route("api/QuestionVariants/{id:int}")]
        public async Task<IActionResult> PutQuestionVariant(int id, QuestionVariant questionVariant)
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

            return NoContent();
        }

        // POST: api/QuestionVariants
        [HttpPost]
        [Route("api/QuestionVariants", Name = "PostQuestionVariant")]
        public async Task<IActionResult> PostQuestionVariant(QuestionVariant questionVariant)
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
        [HttpDelete]
        [Route("api/QuestionVariants/{id:int}")]
        public async Task<IActionResult> DeleteQuestionVariant(int id)
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

        private bool QuestionVariantExists(int id)
        {
            return db.QuestionVariants.Count(e => e.Id == id) > 0;
        }
    }
}