using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер")]
    public class GoalsController : BaseApiController
    {
        public GoalsController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/Goals
        [HttpGet]
        [Route("api/Goals")]
        public IQueryable<Goal> GetGoals()
        {
            return db.Goals;
        }

        // GET: api/Goals/5
        [HttpGet]
        [Route("api/Goals/{id:int}")]
        public async Task<IActionResult> GetGoal(int id)
        {
            Goal goal = await db.Goals.FindAsync(id);
            if (goal == null)
            {
                return NotFound();
            }

            return Ok(goal);
        }

        // PUT: api/Goals/5
        [HttpPut]
        [Route("api/Goals/{id:int}")]
        public async Task<IActionResult> PutGoal(int id, [FromBody]Goal goal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != goal.Id)
            {
                return BadRequest();
            }

            db.Entry(goal).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoalExists(id))
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

        // POST: api/Goals
        [HttpPost]
        [Route("api/Goals", Name = "PostGoal")]
        [AcceptVerbs("POST")]
        public async Task<IActionResult> PostGoal([FromBody]Goal goal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Goals.Add(goal);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostGoal", new { id = goal.Id }, goal);
        }

        // DELETE: api/Goals/5
        [HttpDelete]
        [Route("api/Goals/{id:int}")]
        public async Task<IActionResult> DeleteGoal(int id)
        {
            Goal goal = await db.Goals.FindAsync(id);
            if (goal == null)
            {
                return NotFound();
            }

            db.Goals.Remove(goal);
            await db.SaveChangesAsync();

            return Ok(goal);
        }

        private bool GoalExists(int id)
        {
            return db.Goals.Any(e => e.Id == id);
        }
    }
}