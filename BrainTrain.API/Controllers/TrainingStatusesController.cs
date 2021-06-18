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
    public class TrainingStatusesController : BaseApiController
    {
        public TrainingStatusesController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/TrainingStatuses
        [HttpGet]
        [Route("api/TrainingStatuses")]
        public IQueryable<TrainingStatus> GetTrainingStatus()
        {
            return db.TrainingStatus;
        }

        // GET: api/TrainingStatuses/5
        [HttpGet]
        [Route("api/TrainingStatuses/{id:int}")]
        public async Task<IActionResult> GetTrainingStatus(int id)
        {
            TrainingStatus trainingStatus = await db.TrainingStatus.FindAsync(id);
            if (trainingStatus == null)
            {
                return NotFound();
            }

            return Ok(trainingStatus);
        }

        // PUT: api/TrainingStatuses/5
        [HttpPut]
        [Route("api/TrainingStatuses/{id:int}")]
        public async Task<IActionResult> PutTrainingStatus(int id, TrainingStatus trainingStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trainingStatus.Id)
            {
                return BadRequest();
            }

            db.Entry(trainingStatus).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingStatusExists(id))
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

        // POST: api/TrainingStatuses
        [HttpPost]
        [Route("api/TrainingStatuses", Name = "PostTrainingStatus")]
        public async Task<IActionResult> PostTrainingStatus(TrainingStatus trainingStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TrainingStatus.Add(trainingStatus);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostTrainingStatus", new { id = trainingStatus.Id }, trainingStatus);
        }

        // DELETE: api/TrainingStatuses/5
        [HttpDelete]
        [Route("api/TrainingStatuses/{id:int}")]
        public async Task<IActionResult> DeleteTrainingStatus(int id)
        {
            TrainingStatus trainingStatus = await db.TrainingStatus.FindAsync(id);
            if (trainingStatus == null)
            {
                return NotFound();
            }

            db.TrainingStatus.Remove(trainingStatus);
            await db.SaveChangesAsync();

            return Ok(trainingStatus);
        }

        private bool TrainingStatusExists(int id)
        {
            return db.TrainingStatus.Count(e => e.Id == id) > 0;
        }
    }
}