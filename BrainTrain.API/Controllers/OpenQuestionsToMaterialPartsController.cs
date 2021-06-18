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
    public class OpenQuestionsToMaterialPartsController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/OpenQuestionsToMaterialParts
        [HttpGet]
        [Route("api/OpenQuestionsToMaterialParts")]
        public IEnumerable<OpenQuestionsToMaterialParts> GetOpenQuestionsToMaterialParts(int materialPartId, string materialPartType)
        {
            var qs = db.OpenQuestionsToMaterialParts.Where(oq => (materialPartType == "text" && oq.TextId == materialPartId) ||
                (materialPartType == "video" && oq.VideoId == materialPartId) || (materialPartType == "file" && oq.FileId == materialPartId)).Select(oq => new
                {
                    id = oq.Id,
                    questionId = oq.QuestionId,
                    textId = oq.TextId,
                    videoId = oq.VideoId,
                    fileId = oq.FileId,
                    question = new { id = oq.Question.Id, text = oq.Question.Text }
                }).AsEnumerable().
                Select(oq => new OpenQuestionsToMaterialParts { Id = oq.id, QuestionId = oq.questionId, TextId = oq.textId, VideoId = oq.videoId, FileId = oq.fileId, Question = new Question { Id = oq.question.id, Text = oq.question.text } }).ToList();

            return qs;
        }

        // GET: api/OpenQuestionsToMaterialParts/5
        [HttpGet]
        [Route("api/OpenQuestionsToMaterialParts/{id:int}")]
        [ResponseType(typeof(OpenQuestionsToMaterialParts))]
        public async Task<IHttpActionResult> GetOpenQuestionsToMaterialParts(int id)
        {
            OpenQuestionsToMaterialParts openQuestionsToMaterialParts = await db.OpenQuestionsToMaterialParts.FindAsync(id);
            if (openQuestionsToMaterialParts == null)
            {
                return NotFound();
            }

            return Ok(openQuestionsToMaterialParts);
        }

        // PUT: api/OpenQuestionsToMaterialParts/5
        [HttpPut]
        [Route("api/OpenQuestionsToMaterialParts/{id:int}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOpenQuestionsToMaterialParts(int id, OpenQuestionsToMaterialParts openQuestionsToMaterialParts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != openQuestionsToMaterialParts.Id)
            {
                return BadRequest();
            }

            db.Entry(openQuestionsToMaterialParts).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpenQuestionsToMaterialPartsExists(id))
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

        // POST: api/OpenQuestionsToMaterialParts
        [HttpPost]
        [Route("api/OpenQuestionsToMaterialParts", Name = "PostOpenQuestionsToMaterialParts")]
        [ResponseType(typeof(OpenQuestionsToMaterialParts))]
        public async Task<IHttpActionResult> PostOpenQuestionsToMaterialParts(OpenQuestionsToMaterialParts openQuestionsToMaterialParts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OpenQuestionsToMaterialParts.Add(openQuestionsToMaterialParts);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

            return CreatedAtRoute("PostOpenQuestionsToMaterialParts", new { id = openQuestionsToMaterialParts.Id }, openQuestionsToMaterialParts);
        }

        // DELETE: api/OpenQuestionsToMaterialParts/5
        [HttpDelete]
        [Route("api/OpenQuestionsToMaterialParts/{id:int}")]
        [ResponseType(typeof(OpenQuestionsToMaterialParts))]
        public async Task<IHttpActionResult> DeleteOpenQuestionsToMaterialParts(int id)
        {
            OpenQuestionsToMaterialParts openQuestionsToMaterialParts = await db.OpenQuestionsToMaterialParts.FindAsync(id);
            if (openQuestionsToMaterialParts == null)
            {
                return NotFound();
            }

            db.OpenQuestionsToMaterialParts.Remove(openQuestionsToMaterialParts);
            await db.SaveChangesAsync();

            return Ok(openQuestionsToMaterialParts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OpenQuestionsToMaterialPartsExists(int id)
        {
            return db.OpenQuestionsToMaterialParts.Count(e => e.Id == id) > 0;
        }
    }
}