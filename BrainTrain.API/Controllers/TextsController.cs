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
    public class TextsController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/Texts
        [HttpGet]
        [Route("api/Texts")]
        public IQueryable<Text> GetTexts()
        {
            return db.Texts;
        }

        // GET: api/Texts/5
        [ResponseType(typeof(Text))]
        [HttpGet]
        [Route("api/Texts/{id:int}")]
        public async Task<IHttpActionResult> GetText(int id)
        {
            Text text = await db.Texts.FindAsync(id);
            if (text == null)
            {
                return NotFound();
            }

            return Ok(text);
        }

        // PUT: api/Texts/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/Texts/{id:int}")]
        public async Task<IHttpActionResult> PutText(int id, Text text)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != text.Id)
            {
                return BadRequest();
            }

            text.TextsToMaterials = null;

            db.Entry(text).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TextExists(id))
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

        // POST: api/Texts
        [ResponseType(typeof(Text))]
        [HttpPost]
        [Route("api/Texts", Name = "PostText")]
        public async Task<IHttpActionResult> PostText(Text text, int materialId)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            text.TextsToMaterials = new List<TextsToMaterials>();
            text.TextsToMaterials.Add(new TextsToMaterials { MaterialId = materialId });
            text.DateCreated = DateTime.Now;

            db.Texts.Add(text);

            await db.SaveChangesAsync();


            return CreatedAtRoute("PostText", new { id = text.Id }, text);
        }

        // DELETE: api/Texts/5
        [ResponseType(typeof(Text))]
        [HttpDelete]
        [Route("api/Texts/{id:int}")]
        public async Task<IHttpActionResult> DeleteText(int id)
        {
            Text text = await db.Texts.FindAsync(id);
            if (text == null)
            {
                return NotFound();
            }

            db.Texts.Remove(text);
            await db.SaveChangesAsync();

            return Ok(text);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TextExists(int id)
        {
            return db.Texts.Count(e => e.Id == id) > 0;
        }
    }
}