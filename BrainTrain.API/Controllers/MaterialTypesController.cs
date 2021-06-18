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
    public class MaterialTypesController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/MaterialTypes
        [HttpGet]
        [Route("api/MaterialTypes")]
        public IQueryable<MaterialType> GetMaterialTypes()
        {
            return db.MaterialTypes;
        }

        // GET: api/MaterialTypes/5
        [HttpGet]
        [Route("api/MaterialTypes/{id:int}")]
        [ResponseType(typeof(MaterialType))]
        public async Task<IHttpActionResult> GetMaterialType(int id)
        {
            MaterialType materialType = await db.MaterialTypes.FindAsync(id);
            if (materialType == null)
            {
                return NotFound();
            }

            return Ok(materialType);
        }

        // PUT: api/MaterialTypes/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/MaterialTypes/{id:int}")]
        public async Task<IHttpActionResult> PutMaterialType(int id, MaterialType materialType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != materialType.Id)
            {
                return BadRequest();
            }

            db.Entry(materialType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialTypeExists(id))
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

        // POST: api/MaterialTypes
        [ResponseType(typeof(MaterialType))]
        [HttpPost]
        [Route("api/MaterialTypes", Name = "PostMaterialType")]
        public async Task<IHttpActionResult> PostMaterialType(MaterialType materialType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MaterialTypes.Add(materialType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostMaterialType", new { id = materialType.Id }, materialType);
        }

        // DELETE: api/MaterialTypes/5
        [ResponseType(typeof(MaterialType))]
        public async Task<IHttpActionResult> DeleteMaterialType(int id)
        {
            MaterialType materialType = await db.MaterialTypes.FindAsync(id);
            if (materialType == null)
            {
                return NotFound();
            }

            db.MaterialTypes.Remove(materialType);
            await db.SaveChangesAsync();

            return Ok(materialType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MaterialTypeExists(int id)
        {
            return db.MaterialTypes.Count(e => e.Id == id) > 0;
        }
    }
}