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
    public class TrainingTypesController : BaseApiController
    {
        public TrainingTypesController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/TrainingTypes
        [HttpGet]
        [Route("api/TrainingTypes")]
        public IQueryable<TrainingType> GetTrainingTypes()
        {
            return db.TrainingTypes;
        }

        // GET: api/TrainingTypes/5
        [HttpGet]
        [Route("api/TrainingTypes/{id:int}")]
        public async Task<IActionResult> GetTrainingType(int id)
        {
            TrainingType trainingType = await db.TrainingTypes.FindAsync(id);
            if (trainingType == null)
            {
                return NotFound();
            }

            return Ok(trainingType);
        }

        // PUT: api/TrainingTypes/5
        [HttpPut]
        [Route("api/TrainingTypes/{id:int}")]
        public async Task<IActionResult> PutTrainingType(int id, TrainingType trainingType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trainingType.Id)
            {
                return BadRequest();
            }

            db.Entry(trainingType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingTypeExists(id))
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

        // POST: api/TrainingTypes
        [HttpPost]
        [Route("api/TrainingTypes", Name = "PostTrainingType")]
        public async Task<IActionResult> PostTrainingType(TrainingType trainingType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TrainingTypes.Add(trainingType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostTrainingType", new { id = trainingType.Id }, trainingType);
        }

        // DELETE: api/TrainingTypes/5
        [HttpDelete]
        [Route("api/TrainingTypes/{id:int}")]
        public async Task<IActionResult> DeleteTrainingType(int id)
        {
            TrainingType trainingType = await db.TrainingTypes.FindAsync(id);
            if (trainingType == null)
            {
                return NotFound();
            }

            db.TrainingTypes.Remove(trainingType);
            await db.SaveChangesAsync();

            return Ok(trainingType);
        }


        private bool TrainingTypeExists(int id)
        {
            return db.TrainingTypes.Count(e => e.Id == id) > 0;
        }
    }
}