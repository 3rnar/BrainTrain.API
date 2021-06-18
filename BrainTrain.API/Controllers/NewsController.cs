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
using Microsoft.AspNet.Identity;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер")]
    public class NewsController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/News
        [HttpGet]
        [Route("api/News")]
        public IQueryable<News> GetNews()
        {
            return db.News;
        }

        // GET: api/News/5
        [ResponseType(typeof(News))]
        [HttpGet]
        [Route("api/News/{id:int}")]
        public async Task<IHttpActionResult> GetNews(int id)
        {
            News News = await db.News.FindAsync(id);
            if (News == null)
            {
                return NotFound();
            }

            return Ok(News);
        }

        // PUT: api/News/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/News/{id:int}")]
        public async Task<IHttpActionResult> PutNews(int id, [FromBody]News News)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != News.Id)
            {
                return BadRequest();
            }

            db.Entry(News).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
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

        // POST: api/News
        [ResponseType(typeof(News))]
        [HttpPost]
        [Route("api/News", Name = "PostNews")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostNews([FromBody]News News)
        {
            var userId = User.Identity.GetUserId();

            News.DateCreated = DateTime.Now;
            News.ContentManagerId = userId;

            db.News.Add(News);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostNews", new { id = News.Id }, News);
        }

        // DELETE: api/News/5
        [ResponseType(typeof(News))]
        [HttpDelete]
        [Route("api/News/{id:int}")]
        public async Task<IHttpActionResult> DeleteNews(int id)
        {
            News News = await db.News.FindAsync(id);
            if (News == null)
            {
                return NotFound();
            }

            db.News.Remove(News);
            await db.SaveChangesAsync();

            return Ok(News);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NewsExists(int id)
        {
            return db.News.Count(e => e.Id == id) > 0;
        }
    }
}
