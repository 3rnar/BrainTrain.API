using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers
{
    public class ThemesController : BaseApiController
    {
        public ThemesController(BrainTrainContext _db) : base(_db)
        {
        }

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
        [HttpGet]
        [Route("api/Themes/{id:int}")]
        public async Task<IActionResult> GetTheme(int id)
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
        [HttpPut]
        [Route("api/Themes/{id:int}")]
        public async Task<IActionResult> PutTheme(int id, [FromBody]Theme theme)
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

            return NoContent();
        }

        // POST: api/Themes
        [Authorize(Roles = "Контент-менеджер")]
        [HttpPost]
        [Route("api/Themes", Name = "PostTheme")]
        public async Task<IActionResult> PostTheme(Theme theme)
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
        [HttpDelete]
        [Route("api/Themes/{id:int}")]
        public async Task<IActionResult> DeleteTheme(int id)
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

        private bool ThemeExists(int id)
        {
            return db.Themes.Count(e => e.Id == id) > 0;
        }
    }
}