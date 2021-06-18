using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер")]
    public class NewsController : BaseApiController
    {
        public NewsController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/News
        [HttpGet]
        [Route("api/News")]
        public IQueryable<News> GetNews()
        {
            return db.News;
        }

        // GET: api/News/5
        [HttpGet]
        [Route("api/News/{id:int}")]
        public async Task<IActionResult> GetNews(int id)
        {
            News News = await db.News.FindAsync(id);
            if (News == null)
            {
                return NotFound();
            }

            return Ok(News);
        }

        // PUT: api/News/5
        [HttpPut]
        [Route("api/News/{id:int}")]
        public async Task<IActionResult> PutNews(int id, [FromBody]News News)
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

            return NoContent();
        }

        // POST: api/News
        [HttpPost]
        [Route("api/News", Name = "PostNews")]
        [AcceptVerbs("POST")]
        public async Task<IActionResult> PostNews([FromBody]News News)
        {
            var userId = User.Identity.GetUserId();

            News.DateCreated = DateTime.Now;
            News.ContentManagerId = userId;

            db.News.Add(News);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostNews", new { id = News.Id }, News);
        }

        // DELETE: api/News/5
        [HttpDelete]
        [Route("api/News/{id:int}")]
        public async Task<IActionResult> DeleteNews(int id)
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

        private bool NewsExists(int id)
        {
            return db.News.Count(e => e.Id == id) > 0;
        }
    }
}
