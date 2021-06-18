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
using BrainTrain.API.Helpers.Learnosity;
using BrainTrain.Core.Models;
using BrainTrain.API.Models;
using Newtonsoft.Json.Linq;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер")]
    public class Midterm_QuestionController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        [HttpGet]
        [Route("api/Midterm_Event")]
        public IQueryable<Midterm_Event> GetMidterm_Events()
        {
            return db.Midterm_Events;
        }

        [HttpGet]
        [Route("api/Midterm_Variant")]
        public IQueryable<Midterm_Variant> GetMidterm_Variants()
        {
            return db.Midterm_Variants;
        }

        [HttpGet]
        [Route("api/Midterm_Language")]
        public IQueryable<Midterm_Language> GetMidterm_Languages()
        {
            return db.Midterm_Languages;
        }

        [HttpGet]
        [Route("api/Midterm_Subjects")]
        public IQueryable<Midterm_Subject> GetMidterm_Subjects()
        {
            return db.Midterm_Subjects;
        }

        // GET: api/Midterm_Question
        [HttpGet]
        [Route("api/Midterm_Question")]
        public IQueryable<Midterm_Question> GetMidterm_Questions()
        {
            return db.Midterm_Questions;
        }

        // GET: api/Midterm_Question/5
        [ResponseType(typeof(Midterm_Question))]
        [HttpGet]
        [Route("api/Midterm_Question/{id:int}")]
        public async Task<IHttpActionResult> GetMidterm_Question(int id)
        {
            var midterm_Question = await db.Midterm_Questions.Where(q => q.Id == id).ToListAsync();

            string uuid = "";

            var qJson = LRNQuestionsHelper.Simple(midterm_Question, out uuid);

            return Ok(qJson);
        }

        // PUT: api/Midterm_Question/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/Midterm_Question/{id:int}")]
        public async Task<IHttpActionResult> PutMidterm_Question(int id, Midterm_Question midterm_Question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != midterm_Question.Id)
            {
                return BadRequest();
            }

            db.Entry(midterm_Question).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Midterm_QuestionExists(id))
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

        [HttpPost]
        [Route("api/Midterm_Question/PostLrn")]
        public async Task<IHttpActionResult> PostLrn(LrnMidtermQuestionsEditViewModel model)
        {
            var question = db.Midterm_Questions.FirstOrDefault(q => q.Id == model.Id);
            if (question == null)
                return NotFound();

            var details = JObject.Parse(model.LrnJson);
            var jsonQuestion = details["questions"][0] as JObject;
            jsonQuestion.Remove("response_id");
            jsonQuestion.Remove("questionId");
            jsonQuestion.Remove("questionID");

            var str = jsonQuestion.ToString();

            var first = str.IndexOf("{");
            var last = str.LastIndexOf("}");
            str = str.Remove(last, 1).Remove(first, 1);

            question.LrnJson = str;


            db.SaveChanges();

            return Ok();
        }

        // POST: api/Midterm_Question
        [ResponseType(typeof(Midterm_Question))]
        [HttpPost]
        [Route("api/Midterm_Question", Name = "PostMidterm_Question")]
        public async Task<IHttpActionResult> PostMidterm_Question(Midterm_Question midterm_Question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Midterm_Questions.Add(midterm_Question);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostMidterm_Question", new { id = midterm_Question.Id }, midterm_Question);
        }

        // DELETE: api/Midterm_Question/5
        [ResponseType(typeof(Midterm_Question))]
        [HttpDelete]
        [Route("api/Midterm_Question/{id:int}")]
        public async Task<IHttpActionResult> DeleteMidterm_Question(int id)
        {
            Midterm_Question midterm_Question = await db.Midterm_Questions.FindAsync(id);
            if (midterm_Question == null)
            {
                return NotFound();
            }

            db.Midterm_Questions.Remove(midterm_Question);
            await db.SaveChangesAsync();

            return Ok(midterm_Question);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Midterm_QuestionExists(int id)
        {
            return db.Midterm_Questions.Count(e => e.Id == id) > 0;
        }
    }
}