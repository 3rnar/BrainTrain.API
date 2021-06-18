using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер,Заполнение вопросов")]
    public class EntVariantsController : BaseApiController
    {
        private readonly BrainTrainContext db;
        public EntVariantsController(BrainTrainContext _db)
        {
            db = _db;
        }

        // GET: api/EntVariants
        [HttpGet]
        [Route("api/EntVariants")]
        public IQueryable<EntVariant> GetEntVariants()
        {
            return db.EntVariants;
        }

        // GET: api/EntVariants/5
        [HttpGet]
        [Route("api/EntVariants/{id:int}")]
        public async Task<IActionResult> GetEntVariant(int id)
        {
            EntVariant entVariant = await db.EntVariants.FindAsync(id);
            if (entVariant == null)
            {
                return NotFound();
            }

            return Ok(entVariant);
        }

        // PUT: api/EntVariants/5
        [HttpPut]
        [Route("api/EntVariants/{id:int}")]
        public async Task<IActionResult> PutEntVariant(int id, EntVariant entVariant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entVariant.Id)
            {
                return BadRequest();
            }

            db.Entry(entVariant).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntVariantExists(id))
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

        // POST: api/EntVariants
        [HttpPost]
        [Route("api/EntVariants", Name = "PostEntVariant")]
        public async Task<IActionResult> PostEntVariant(EntVariant entVariant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EntVariants.Add(entVariant);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostEntVariant", new { id = entVariant.Id }, entVariant);
        }

        // DELETE: api/EntVariants/5
        [HttpDelete]
        [Route("api/EntVariants/{id:int}")]
        public async Task<IActionResult> DeleteEntVariant(int id)
        {
            EntVariant entVariant = await db.EntVariants.FindAsync(id);
            if (entVariant == null)
            {
                return NotFound();
            }

            db.EntVariants.Remove(entVariant);
            await db.SaveChangesAsync();

            return Ok(entVariant);
        }

        private bool EntVariantExists(int id)
        {
            return db.EntVariants.Any(e => e.Id == id);
        }
    }
}