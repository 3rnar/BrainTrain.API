using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BrainTrain.Core.Models;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers
{
    public class QuestionTypesController : BaseApiController
    {
        public QuestionTypesController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/QuestionTypes
        [Authorize(Roles = "Контент-менеджер,Заполнение вопросов")]
        [HttpGet]
        [Route("api/QuestionTypes")]
        public IQueryable<QuestionType> GetQuestionTypes()
        {
            return db.QuestionTypes;
        }

        // GET: api/QuestionTypes/5
        [Authorize(Roles = "Контент-менеджер")]
        [HttpGet]
        [Route("api/QuestionTypes/{id:int}")]
        public async Task<IActionResult> GetQuestionType(int id)
        {
            QuestionType questionType = await db.QuestionTypes.FindAsync(id);
            if (questionType == null)
            {
                return NotFound();
            }

            return Ok(questionType);
        }

        // PUT: api/QuestionTypes/5
        [Authorize(Roles = "Контент-менеджер")]
        [HttpPut]
        [Route("api/QuestionTypes/{id:int}")]
        public async Task<IActionResult> PutQuestionType(int id, QuestionType questionType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != questionType.Id)
            {
                return BadRequest();
            }

            db.Entry(questionType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionTypeExists(id))
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

        // POST: api/QuestionTypes
        [Authorize(Roles = "Контент-менеджер")]
        [HttpPost]
        [Route("api/QuestionTypes", Name = "PostQuestionType")]
        public async Task<IActionResult> PostQuestionType(QuestionType questionType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.QuestionTypes.Add(questionType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostQuestionType", new { id = questionType.Id }, questionType);
        }

        // DELETE: api/QuestionTypes/5
        [Authorize(Roles = "Контент-менеджер")]
        [HttpDelete]
        [Route("api/QuestionTypes/{id:int}")]
        public async Task<IActionResult> DeleteQuestionType(int id)
        {
            QuestionType questionType = await db.QuestionTypes.FindAsync(id);
            if (questionType == null)
            {
                return NotFound();
            }

            db.QuestionTypes.Remove(questionType);
            await db.SaveChangesAsync();

            return Ok(questionType);
        }

        private bool QuestionTypeExists(int id)
        {
            return db.QuestionTypes.Count(e => e.Id == id) > 0;
        }
    }
}