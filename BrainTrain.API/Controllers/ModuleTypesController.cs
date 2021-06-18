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
    public class ModuleTypesController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/ModuleTypes
        [HttpGet]
        [Route("api/ModuleTypes")]
        public IQueryable<ModuleType> GetModuleTypes()
        {
            return db.ModuleTypes;
        }

        // GET: api/ModuleTypes/5
        [ResponseType(typeof(ModuleType))]
        [HttpGet]
        [Route("api/ModuleTypes/{id:int}")]
        public async Task<IHttpActionResult> GetModuleType(int id)
        {
            ModuleType moduleType = await db.ModuleTypes.FindAsync(id);
            if (moduleType == null)
            {
                return NotFound();
            }

            return Ok(moduleType);
        }

        // PUT: api/ModuleTypes/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/ModuleTypes/{id:int}")]
        public async Task<IHttpActionResult> PutModuleType(int id, ModuleType moduleType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != moduleType.Id)
            {
                return BadRequest();
            }

            db.Entry(moduleType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleTypeExists(id))
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

        // POST: api/ModuleTypes
        [ResponseType(typeof(ModuleType))]
        [HttpPost]
        [Route("api/ModuleTypes", Name = "PostModuleType")]
        public async Task<IHttpActionResult> PostModuleType(ModuleType moduleType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ModuleTypes.Add(moduleType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostModuleType", new { id = moduleType.Id }, moduleType);
        }

        // DELETE: api/ModuleTypes/5
        [ResponseType(typeof(ModuleType))]
        [HttpDelete]
        [Route("api/ModuleTypes/{id:int}")]
        public async Task<IHttpActionResult> DeleteModuleType(int id)
        {
            ModuleType moduleType = await db.ModuleTypes.FindAsync(id);
            if (moduleType == null)
            {
                return NotFound();
            }

            db.ModuleTypes.Remove(moduleType);
            await db.SaveChangesAsync();

            return Ok(moduleType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModuleTypeExists(int id)
        {
            return db.ModuleTypes.Count(e => e.Id == id) > 0;
        }
    }
}