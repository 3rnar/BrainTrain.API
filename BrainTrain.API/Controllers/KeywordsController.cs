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
    public class KeywordsController : BaseApiController
    {
        private readonly BrainTrainContext db;
        public KeywordsController(BrainTrainContext _db)
        {
            db = _db;
        }

        // GET: api/Keywords
        [HttpGet]
        [Route("api/Keywords")]
        public IQueryable<Keyword> GetKeyWords()
        {
            return db.KeyWords;
        }

        // GET: api/Keywords/5
        [HttpGet]
        [Route("api/Keywords/{id:int}")]
        public async Task<IActionResult> GetKeyword(int id)
        {
            Keyword keyword = await db.KeyWords.FindAsync(id);
            if (keyword == null)
            {
                return NotFound();
            }

            return Ok(keyword);
        }

        // PUT: api/Keywords/5
        [HttpPut]
        [Route("api/Keywords/{id:int}")]
        public async Task<IActionResult> PutKeyword(int id, Keyword keyword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != keyword.Id)
            {
                return BadRequest();
            }

            db.Entry(keyword).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KeywordExists(id))
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

        // POST: api/Keywords
        [HttpPost]
        [Route("api/Keywords", Name = "PostKeyword")]
        public async Task<IActionResult> PostKeyword(Keyword keyword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.KeyWords.Add(keyword);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostKeyword", new { id = keyword.Id }, keyword);
        }

        // DELETE: api/Keywords/5
        [HttpDelete]
        [Route("api/Keywords/{id:int}")]
        public async Task<IActionResult> DeleteKeyword(int id)
        {
            Keyword keyword = await db.KeyWords.FindAsync(id);
            if (keyword == null)
            {
                return NotFound();
            }

            db.KeyWords.Remove(keyword);
            await db.SaveChangesAsync();

            return Ok(keyword);
        }

        private bool KeywordExists(int id)
        {
            return db.KeyWords.Any(e => e.Id == id);
        }
    }
}