using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers
{
    public class QuestionDifficultiesController : BaseApiController
    {
        public QuestionDifficultiesController(BrainTrainContext _db) : base(_db)
        {
        }

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
        [HttpGet]
        [Route("api/QuestionDifficulties/{id:int}")]
        public async Task<IActionResult> GetQuestionDifficulty(int id)
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
        [HttpPut]
        [Route("api/QuestionDifficulties/{id:int}")]
        public async Task<IActionResult> PutQuestionDifficulty(int id, QuestionDifficulty questionDifficulty)
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

            return NoContent();
        }

        // POST: api/QuestionDifficulties
        [Authorize(Roles = "Контент-менеджер")]
        [HttpPost]
        [Route("api/QuestionDifficulties", Name = "PostQuestionDifficulty")]
        public async Task<IActionResult> PostQuestionDifficulty(QuestionDifficulty questionDifficulty)
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
        [HttpDelete]
        [Route("api/QuestionDifficulties/{id:int}")]
        public async Task<IActionResult> DeleteQuestionDifficulty(int id)
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

        private bool QuestionDifficultyExists(int id)
        {
            return db.QuestionDifficulties.Count(e => e.Id == id) > 0;
        }
    }
}