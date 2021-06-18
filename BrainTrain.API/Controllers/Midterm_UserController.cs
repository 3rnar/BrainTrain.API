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
using BrainTrain.API.Models;
using BrainTrain.Core.Models;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер")]
    public class Midterm_UserController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/Midterm_User
        [Route("api/Midterm_User")]
        [HttpGet]
        public IQueryable<Midterm_User> GetMidterm_Users()
        {
            return db.Midterm_Users;
        }

        [HttpGet]
        [Route("api/Midterm_User/Paging")]
        public async Task<IHttpActionResult> GetMidterm_UsersPaging(int eventId, int languageId, int pageNum = 1, int perPage = 20)
        {
            //if (search == null)
            //    search = "";
            var skip = (pageNum - 1) * perPage;
            var users = await db.Midterm_Users.
                Include(u => u.Midterm_UserEvents).
                Include(u => u.Midterm_UserEvents.Select(e => e.Midterm_UserEventQuestions)).
                Include(u => u.Midterm_UserEvents.Select(e => e.Midterm_UserEventQuestions.Select(q => q.Midterm_Question))).
                Where(u => u.Midterm_UserEvents.Any(e => e.EventId == eventId) && u.LanguageId == languageId
                    //&& ( u.FullName.ToLower().Contains(search.ToLower()) )
                ).
                OrderBy(u => u.FullName).
                Skip(skip).Take(perPage).
                ToListAsync();
            var count = await db.Midterm_Users.
                 CountAsync(u => u.Midterm_UserEvents.Any(e => e.EventId == eventId) && u.LanguageId == languageId
                    //&& u.FullName.ToLower().Contains(search.ToLower())
            );

            var list = new List<CustomerMidtermUserModel>();

            foreach (var u in users)
            {
                var item = new CustomerMidtermUserModel
                {
                    Id = u.Id,
                    Name = u.FullName,
                    IIN = u.SystemId,
                    NumberOfComplaints = u.Midterm_UserEvents.FirstOrDefault(e => e.EventId == eventId)?.Midterm_UserEventQuestions.Where(q => q.StatusId == 1).Count(),
                    Maths = u.Midterm_UserEvents.FirstOrDefault(e => e.EventId == eventId)?.Midterm_UserEventQuestions.Where(q => q.Midterm_Question?.SubjectId == 1 && q.IsCorrect == true).Sum(q => q.Midterm_Question?.Weight),
                    Physics = u.Midterm_UserEvents.FirstOrDefault(e => e.EventId == eventId)?.Midterm_UserEventQuestions.Where(q => q.Midterm_Question?.SubjectId == 2 && q.IsCorrect == true).Sum(q => q.Midterm_Question?.Weight),
                    LanguageId = u.LanguageId,
                    DateStart = u.Midterm_UserEvents.FirstOrDefault(e => e.EventId == eventId)?.DateStart,
                    DateFinish = u.Midterm_UserEvents.FirstOrDefault(e => e.EventId == eventId)?.DateFinish,
                    IsPrinted = u.Midterm_UserEvents.FirstOrDefault(e => e.EventId == eventId)?.IsPrinted
                };

                list.Add(item);
            }

            return Ok(new { Data = list, Count = count });
        }

        // GET: api/Midterm_User/5
        [ResponseType(typeof(Midterm_User))]
        [HttpGet]
        [Route("api/Midterm_User/{id:int}")]
        public async Task<IHttpActionResult> GetMidterm_User(int id)
        {
            Midterm_User midterm_User = await db.Midterm_Users.FindAsync(id);
            if (midterm_User == null)
            {
                return NotFound();
            }

            return Ok(midterm_User);
        }

        // PUT: api/Midterm_User/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/Midterm_User/{id:int}")]
        public async Task<IHttpActionResult> PutMidterm_User(int id, Midterm_User midterm_User)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != midterm_User.Id)
            {
                return BadRequest();
            }

            db.Entry(midterm_User).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Midterm_UserExists(id))
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

        // POST: api/Midterm_User
        [ResponseType(typeof(Midterm_User))]
        [HttpPost]
        [Route("api/Midterm_User", Name = "PostMidterm_User")]
        public async Task<IHttpActionResult> PostMidterm_User(Midterm_User midterm_User)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Midterm_Users.Add(midterm_User);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = midterm_User.Id }, midterm_User);
        }

        // DELETE: api/Midterm_User/5
        [ResponseType(typeof(Midterm_User))]
        [HttpDelete]
        [Route("api/Midterm_User/{id:int}")]
        public async Task<IHttpActionResult> DeleteMidterm_User(int id)
        {
            Midterm_User midterm_User = await db.Midterm_Users.FindAsync(id);
            if (midterm_User == null)
            {
                return NotFound();
            }

            db.Midterm_Users.Remove(midterm_User);
            await db.SaveChangesAsync();

            return Ok(midterm_User);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Midterm_UserExists(int id)
        {
            return db.Midterm_Users.Count(e => e.Id == id) > 0;
        }
    }
}