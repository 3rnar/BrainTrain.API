using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер")]
    public class ModuleTypesController : BaseApiController
    {
        public ModuleTypesController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/ModuleTypes
        [HttpGet]
        [Route("api/ModuleTypes")]
        public IQueryable<ModuleType> GetModuleTypes()
        {
            return db.ModuleTypes;
        }

        // GET: api/ModuleTypes/5
        [HttpGet]
        [Route("api/ModuleTypes/{id:int}")]
        public async Task<IActionResult> GetModuleType(int id)
        {
            ModuleType moduleType = await db.ModuleTypes.FindAsync(id);
            if (moduleType == null)
            {
                return NotFound();
            }

            return Ok(moduleType);
        }

        // PUT: api/ModuleTypes/5
        [HttpPut]
        [Route("api/ModuleTypes/{id:int}")]
        public async Task<IActionResult> PutModuleType(int id, ModuleType moduleType)
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

            return NoContent();
        }

        // POST: api/ModuleTypes
        [HttpPost]
        [Route("api/ModuleTypes", Name = "PostModuleType")]
        public async Task<IActionResult> PostModuleType(ModuleType moduleType)
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
        [HttpDelete]
        [Route("api/ModuleTypes/{id:int}")]
        public async Task<IActionResult> DeleteModuleType(int id)
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


        private bool ModuleTypeExists(int id)
        {
            return db.ModuleTypes.Count(e => e.Id == id) > 0;
        }
    }
}