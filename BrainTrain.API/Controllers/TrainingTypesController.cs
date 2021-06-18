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
    [Authorize(Roles = "Контент-менеджер")]
    public class TrainingTypesController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/TrainingTypes
        [HttpGet]
        [Route("api/TrainingTypes")]
        public IQueryable<TrainingType> GetTrainingTypes()
        {
            return db.TrainingTypes;
        }

        // GET: api/TrainingTypes/5
        [ResponseType(typeof(TrainingType))]
        [HttpGet]
        [Route("api/TrainingTypes/{id:int}")]
        public async Task<IHttpActionResult> GetTrainingType(int id)
        {
            TrainingType trainingType = await db.TrainingTypes.FindAsync(id);
            if (trainingType == null)
            {
                return NotFound();
            }

            return Ok(trainingType);
        }

        // PUT: api/TrainingTypes/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/TrainingTypes/{id:int}")]
        public async Task<IHttpActionResult> PutTrainingType(int id, TrainingType trainingType)
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TrainingTypes
        [ResponseType(typeof(TrainingType))]
        [HttpPost]
        [Route("api/TrainingTypes", Name = "PostTrainingType")]
        public async Task<IHttpActionResult> PostTrainingType(TrainingType trainingType)
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
        [ResponseType(typeof(TrainingType))]
        [HttpDelete]
        [Route("api/TrainingTypes/{id:int}")]
        public async Task<IHttpActionResult> DeleteTrainingType(int id)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TrainingTypeExists(int id)
        {
            return db.TrainingTypes.Count(e => e.Id == id) > 0;
        }
    }
}