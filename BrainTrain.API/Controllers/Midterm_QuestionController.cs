using BrainTrain.API.Helpers.Learnosity;
using BrainTrain.Core.Models;
using BrainTrain.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер")]
    public class Midterm_QuestionController : BaseApiController
    {

        public Midterm_QuestionController(BrainTrainContext _db) : base(_db)
        {
        }

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
        [HttpGet]
        [Route("api/Midterm_Question/{id:int}")]
        public async Task<IActionResult> GetMidterm_Question(int id)
        {
            var midterm_Question = await db.Midterm_Questions.Where(q => q.Id == id).ToListAsync();

            string uuid = "";

            var qJson = LRNQuestionsHelper.Simple(midterm_Question, out uuid);

            return Ok(qJson);
        }

        // PUT: api/Midterm_Question/5
        [HttpPut]
        [Route("api/Midterm_Question/{id:int}")]
        public async Task<IActionResult> PutMidterm_Question(int id, Midterm_Question midterm_Question)
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

            return NoContent();
        }

        [HttpPost]
        [Route("api/Midterm_Question/PostLrn")]
        public async Task<IActionResult> PostLrn(LrnMidtermQuestionsEditViewModel model)
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
        [HttpPost]
        [Route("api/Midterm_Question", Name = "PostMidterm_Question")]
        public async Task<IActionResult> PostMidterm_Question(Midterm_Question midterm_Question)
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
        [HttpDelete]
        [Route("api/Midterm_Question/{id:int}")]
        public async Task<IActionResult> DeleteMidterm_Question(int id)
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
        private bool Midterm_QuestionExists(int id)
        {
            return db.Midterm_Questions.Count(e => e.Id == id) > 0;
        }
    }
}