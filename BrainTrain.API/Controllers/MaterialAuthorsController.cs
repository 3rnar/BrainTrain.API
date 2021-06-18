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
    public class MaterialAuthorsController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

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
        [ResponseType(typeof(MaterialAuthor))]
        public async Task<IHttpActionResult> GetMaterialAuthor(int id)
        {
            MaterialAuthor materialAuthor = await db.MaterialAuthors.FindAsync(id);
            if (materialAuthor == null)
            {
                return NotFound();
            }

            return Ok(materialAuthor);
        }

        // PUT: api/MaterialAuthors/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/MaterialAuthors/{id:int}")]
        public async Task<IHttpActionResult> PutMaterialAuthor(int id, MaterialAuthor materialAuthor)
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/MaterialAuthors
        [ResponseType(typeof(MaterialAuthor))]
        [HttpPost]
        [Route("api/MaterialAuthors", Name = "PostMaterialAuthor")]
        public async Task<IHttpActionResult> PostMaterialAuthor(MaterialAuthor materialAuthor)
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
        [ResponseType(typeof(MaterialAuthor))]
        public async Task<IHttpActionResult> DeleteMaterialAuthor(int id)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MaterialAuthorExists(int id)
        {
            return db.MaterialAuthors.Count(e => e.Id == id) > 0;
        }
    }
}