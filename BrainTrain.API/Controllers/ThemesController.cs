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
    public class ThemesController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/Themes
        [Authorize(Roles = "Контент-менеджер,Заполнение вопросов")]
        [HttpGet]
        [Route("api/Themes")]
        public List<Theme> GetThemes()
        {
            var result = db.Themes.Select(t => new
            {
                id = t.Id,
                title = t.Title,
                subjectId = t.SubjectId,
                ParentThemeId = t.ParentThemeId,
                gradeId = t.GradeId,
                imageUrl = t.ImageUrl,
                order = t.Order,
                subject = new { id = t.Subject.Id, title = t.Subject.Title },
                parentTheme = t.ParentTheme == null ? null : new { id = t.ParentTheme.Id, title = t.ParentTheme.Title },
                grade = new { id = t.Grade.Id, title = t.Grade.Title }
            }).AsEnumerable().Select(t => new Theme
            {
                Id = t.id,
                Title = t.title,
                SubjectId = t.subjectId,
                ParentThemeId = t.ParentThemeId,
                GradeId = t.gradeId,
                ImageUrl = t.imageUrl,
                Order = t.order,
                Subject = new Subject {Id = t.subject.id, Title = t.subject.title },
                ParentTheme = t.parentTheme == null ? null : new Theme { Id = t.parentTheme.id, Title = t.parentTheme.title },
                Grade = new Grade { Id = t.grade.id, Title = t.grade.title}
            }).OrderBy(t => t.GradeId).ThenBy(t => t.Order).ToList();

            return result;
        }

        // GET: api/Themes/5
        [Authorize(Roles = "Контент-менеджер")]
        [ResponseType(typeof(Theme))]
        [HttpGet]
        [Route("api/Themes/{id:int}")]
        public async Task<IHttpActionResult> GetTheme(int id)
        {
            Theme theme = await db.Themes.FindAsync(id);
            if (theme == null)
            {
                return NotFound();
            }

            return Ok(theme);
        }

        // PUT: api/Themes/5
        [Authorize(Roles = "Контент-менеджер")]
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/Themes/{id:int}")]
        public async Task<IHttpActionResult> PutTheme(int id, [FromBody]Theme theme)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != theme.Id)
            {
                return BadRequest();
            }

            db.Entry(theme).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThemeExists(id))
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

        // POST: api/Themes
        [Authorize(Roles = "Контент-менеджер")]
        [ResponseType(typeof(Theme))]
        [HttpPost]
        [Route("api/Themes", Name = "PostTheme")]
        public async Task<IHttpActionResult> PostTheme(Theme theme)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Themes.Add(theme);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostTheme", new { id = theme.Id }, theme);
        }

        [Authorize(Roles = "Контент-менеджер")]
        [HttpPost]
        [Route("api/Themes/PostOrder")]
        public async Task PostThemeOrder(int[] themes)
        {
            var dbThemes = await db.Themes.Where(t => themes.Contains(t.Id)).ToListAsync();
            foreach (var dbTheme in dbThemes)
            {
                dbTheme.Order = themes.ToList().IndexOf(dbTheme.Id);
            }

            await db.SaveChangesAsync();
        }

        // DELETE: api/Themes/5
        [Authorize(Roles = "Контент-менеджер")]
        [ResponseType(typeof(Theme))]
        [HttpDelete]
        [Route("api/Themes/{id:int}")]
        public async Task<IHttpActionResult> DeleteTheme(int id)
        {
            Theme theme = await db.Themes.FindAsync(id);
            if (theme == null)
            {
                return NotFound();
            }

            db.Themes.Remove(theme);
            await db.SaveChangesAsync();

            return Ok(theme);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ThemeExists(int id)
        {
            return db.Themes.Count(e => e.Id == id) > 0;
        }
    }
}