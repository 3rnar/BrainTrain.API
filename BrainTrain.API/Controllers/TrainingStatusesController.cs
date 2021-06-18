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
    public class TrainingStatusesController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/TrainingStatuses
        [HttpGet]
        [Route("api/TrainingStatuses")]
        public IQueryable<TrainingStatus> GetTrainingStatus()
        {
            return db.TrainingStatus;
        }

        // GET: api/TrainingStatuses/5
        [ResponseType(typeof(TrainingStatus))]
        [HttpGet]
        [Route("api/TrainingStatuses/{id:int}")]
        public async Task<IHttpActionResult> GetTrainingStatus(int id)
        {
            TrainingStatus trainingStatus = await db.TrainingStatus.FindAsync(id);
            if (trainingStatus == null)
            {
                return NotFound();
            }

            return Ok(trainingStatus);
        }

        // PUT: api/TrainingStatuses/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/TrainingStatuses/{id:int}")]
        public async Task<IHttpActionResult> PutTrainingStatus(int id, TrainingStatus trainingStatus)
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TrainingStatuses
        [ResponseType(typeof(TrainingStatus))]
        [HttpPost]
        [Route("api/TrainingStatuses", Name = "PostTrainingStatus")]
        public async Task<IHttpActionResult> PostTrainingStatus(TrainingStatus trainingStatus)
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
        [ResponseType(typeof(TrainingStatus))]
        [HttpDelete]
        [Route("api/TrainingStatuses/{id:int}")]
        public async Task<IHttpActionResult> DeleteTrainingStatus(int id)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TrainingStatusExists(int id)
        {
            return db.TrainingStatus.Count(e => e.Id == id) > 0;
        }
    }
}