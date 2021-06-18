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
    public class MaterialTypesController : BaseApiController
    {
        public MaterialTypesController(BrainTrainContext _db) : base(_db)
        {
        }

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
        public async Task<IActionResult> GetMaterialType(int id)
        {
            MaterialType materialType = await db.MaterialTypes.FindAsync(id);
            if (materialType == null)
            {
                return NotFound();
            }

            return Ok(materialType);
        }

        // PUT: api/MaterialTypes/5
        [HttpPut]
        [Route("api/MaterialTypes/{id:int}")]
        public async Task<IActionResult> PutMaterialType(int id, MaterialType materialType)
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

            return NoContent();
        }

        // POST: api/MaterialTypes
        [HttpPost]
        [Route("api/MaterialTypes", Name = "PostMaterialType")]
        public async Task<IActionResult> PostMaterialType(MaterialType materialType)
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
        [HttpDelete]
        [Route("api/MaterialTypes/{id:int}")]
        public async Task<IActionResult> DeleteMaterialType(int id)
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

        private bool MaterialTypeExists(int id)
        {
            return db.MaterialTypes.Any(e => e.Id == id);
        }
    }
}