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
    public class MaterialAuthorsController : BaseApiController
    {
        public MaterialAuthorsController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/MaterialAuthors
        [HttpGet]
        [Route("api/MaterialAuthors")]
        public IQueryable<MaterialAuthor> GetMaterialAuthors()
        {
            return db.MaterialAuthors;
        }

        // GET: api/MaterialAuthors/5
        [HttpGet]
        [Route("api/MaterialAuthors/{id:int}")]
        public async Task<IActionResult> GetMaterialAuthor(int id)
        {
            MaterialAuthor materialAuthor = await db.MaterialAuthors.FindAsync(id);
            if (materialAuthor == null)
            {
                return NotFound();
            }

            return Ok(materialAuthor);
        }

        // PUT: api/MaterialAuthors/5
        [HttpPut]
        [Route("api/MaterialAuthors/{id:int}")]
        public async Task<IActionResult> PutMaterialAuthor(int id, MaterialAuthor materialAuthor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != materialAuthor.Id)
            {
                return BadRequest();
            }

            db.Entry(materialAuthor).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialAuthorExists(id))
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

        // POST: api/MaterialAuthors
        [HttpPost]
        [Route("api/MaterialAuthors", Name = "PostMaterialAuthor")]
        public async Task<IActionResult> PostMaterialAuthor(MaterialAuthor materialAuthor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MaterialAuthors.Add(materialAuthor);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostMaterialAuthor", new { id = materialAuthor.Id }, materialAuthor);
        }

        // DELETE: api/MaterialAuthors/5
        [HttpDelete]
        [Route("api/MaterialAuthors/{id:int}")]
        public async Task<IActionResult> DeleteMaterialAuthor(int id)
        {
            MaterialAuthor materialAuthor = await db.MaterialAuthors.FindAsync(id);
            if (materialAuthor == null)
            {
                return NotFound();
            }

            db.MaterialAuthors.Remove(materialAuthor);
            await db.SaveChangesAsync();

            return Ok(materialAuthor);
        }

        private bool MaterialAuthorExists(int id)
        {
            return db.MaterialAuthors.Any(e => e.Id == id);
        }
    }
}