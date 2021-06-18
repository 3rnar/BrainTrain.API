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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace BrainTrain.API.Controllers
{    
    public class SubjectsController : BaseApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/Subjects
        [Authorize(Roles = "Контент-менеджер")]
        [HttpGet]
        [Route("api/UserSubjects")]
        public async Task<IHttpActionResult> GetUserSubjects()
        {
            ApplicationUser user = await UserManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return Unauthorized();
            }

            var subjects = user.UsersToSubjects.Select(uts => uts.Subject).ToList();

            return Ok(subjects);
        }

        // GET: api/Subjects
        [Authorize(Roles = "Контент-менеджер,Заполнение вопросов")]
        [HttpGet]
        [Route("api/Subjects")]
        public IEnumerable<Subject> GetSubjects()
        {
            var subjects = db.Subjects.Select(s => new {
                id =s.Id,
                title = s.Title,
                entrantQuestions = s.EntrantQuestions.Select(eq => new { id = eq.Id, subjectId = eq.SubjectId, questionId = eq.QuestionId }).ToList()
            }).AsEnumerable().Select(s => new Subject {
                Id = s.id,
                Title = s.title,
                EntrantQuestions = s.entrantQuestions.Select(eq => new EntrantTestQuestion { Id = eq.id, SubjectId = eq.subjectId, QuestionId = eq.questionId }).ToList()
            }).ToList();
            return subjects;
        }

        // GET: api/Subjects/5
        [Authorize(Roles = "Контент-менеджер")]
        [ResponseType(typeof(Subject))]
        [HttpGet]
        [Route("api/Subjects/{id:int}")]
        public async Task<IHttpActionResult> GetSubject(int id)
        {
            Subject subject = await db.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            return Ok(subject);
        }

        // PUT: api/Subjects/5
        [Authorize(Roles = "Контент-менеджер")]
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/Subjects/{id:int}")]
        public async Task<IHttpActionResult> PutSubject(int id, Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subject.Id)
            {
                return BadRequest();
            }

            var subj = await db.Subjects.FindAsync(id);
            if (subj == null)
                return NotFound();
            if (subj.SubjectsToGoals.Count > 0)
            {
                var toDel = subj.SubjectsToGoals.Where(subjSubjectsToGoal => !subject.SubjectsToGoals.Any(sg => sg.GoalId == subjSubjectsToGoal.GoalId && sg.SubjectId == subjSubjectsToGoal.SubjectId)).ToList();
                db.SubjectsToGoals.RemoveRange(toDel);
            }

            foreach (var subjectsToGoal in subject.SubjectsToGoals)
            {
                if (
                    !subj.SubjectsToGoals.Any(
                        sg => sg.SubjectId == subjectsToGoal.SubjectId && sg.GoalId == subjectsToGoal.GoalId))
                {
                    db.SubjectsToGoals.Add(subjectsToGoal);
                }
            }

            db.Entry(subj).State = EntityState.Detached;
            await db.SaveChangesAsync();


            subject.SubjectsToGoals = null;
            db.Entry(subject).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(id))
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

        // POST: api/Subjects
        [Authorize(Roles = "Контент-менеджер")]
        [ResponseType(typeof(Subject))]
        [HttpPost]
        [Route("api/Subjects", Name = "PostSubject")]
        public async Task<IHttpActionResult> PostSubject(Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Subjects.Add(subject);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostSubject", new { id = subject.Id }, subject);
        }

        // DELETE: api/Subjects/5
        [Authorize(Roles = "Контент-менеджер")]
        [ResponseType(typeof(Subject))]
        [HttpDelete]
        [Route("api/Subjects/{id:int}")]
        public async Task<IHttpActionResult> DeleteSubject(int id)
        {
            Subject subject = await db.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            db.Subjects.Remove(subject);
            await db.SaveChangesAsync();

            return Ok(subject);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubjectExists(int id)
        {
            return db.Subjects.Count(e => e.Id == id) > 0;
        }
    }
}