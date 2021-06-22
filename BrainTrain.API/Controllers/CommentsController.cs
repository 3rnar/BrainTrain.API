using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrainTrain.API.Controllers
{
    [Authorize]
    public class CommentsController : BaseApiController
    {
        public CommentsController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/Comments
        [HttpGet]
        [Route("api/Comments")]
        public IQueryable<Comment> GetComments()
        {
            var usr = User;
            var user = UserId;
            return db.Comments;
        }

        // GET: api/Comments/5
        [HttpGet]
        [Route("api/Comments/{id:int}")]
        public async Task<IActionResult> GetComment(int id)
        {
            Comment Comment = await db.Comments.FindAsync(id);
            if (Comment == null)
            {
                return NotFound();
            }

            return Ok(Comment);
        }

        // PUT: api/Comments/5
        [HttpPut]
        [Route("api/Comments/{id:int}")]
        public async Task<IActionResult> PutComment(int id, [FromBody]Comment Comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Comment.Id)
            {
                return BadRequest();
            }

            db.Entry(Comment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comments
        [HttpPost]
        [Route("api/Comments", Name = "PostComment")]
        [AcceptVerbs("POST")]
        public async Task<IActionResult> PostComment([FromBody]Comment Comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Comments.Add(Comment);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostComment", new { id = Comment.Id }, Comment);
        }

        // DELETE: api/Comments/5
        [HttpDelete]
        [Route("api/Comments/{id:int}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            Comment Comment = await db.Comments.FindAsync(id);
            if (Comment == null)
            {
                return NotFound();
            }

            db.Comments.Remove(Comment);
            await db.SaveChangesAsync();

            return Ok(Comment);
        }

        private bool CommentExists(int id)
        {
            return db.Comments.Any(e => e.Id == id);
        }
    }
}
