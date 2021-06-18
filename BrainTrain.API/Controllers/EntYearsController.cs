using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер,Заполнение вопросов")]
    public class EntYearsController : BaseApiController
    {
        private readonly BrainTrainContext db;
        public EntYearsController(BrainTrainContext _db)
        {
            db = _db;
        }

        // GET: api/EntYears
        [HttpGet]
        [Route("api/EntYears")]
        public IEnumerable<EntYear> GetEntYears()
        {
            
            return db.EntYears.Select(e => new {
                id = e.Id,
                title = e.Title,
                entVariants = e.EntVariants.Select(ev => new {
                    id = ev.Id,
                    title = ev.Title,
                    subjectId = ev.SubjectId,
                    entYearId = ev.EntYearId
                }).ToList()
            }).AsEnumerable().Select(e => new EntYear {
                Id = e.id,
                Title = e.title,
                EntVariants = e.entVariants.Select(ev => new EntVariant {
                    Id = ev.id,
                    Title = ev.title,
                    SubjectId = ev.subjectId,
                    EntYearId = ev.entYearId
                }).ToList()
            }).ToList();
        }

        // GET: api/EntYears/5
        [HttpGet]
        [Route("api/EntYears/{id:int}")]
        public async Task<IActionResult> GetEntYear(int id)
        {
            EntYear entYear = await db.EntYears.FindAsync(id);
            if (entYear == null)
            {
                return NotFound();
            }

            return Ok(entYear);
        }

        // PUT: api/EntYears/5
        [HttpPut]
        [Route("api/EntYears/{id:int}")]
        public async Task<IActionResult> PutEntYear(int id, EntYear entYear)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entYear.Id)
            {
                return BadRequest();
            }

            db.Entry(entYear).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntYearExists(id))
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

        // POST: api/EntYears
        [HttpPost]
        [Route("api/EntYears", Name = "PostEntYear")]
        public async Task<IActionResult> PostEntYear(EntYear entYear)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EntYears.Add(entYear);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostEntYear", new { id = entYear.Id }, entYear);
        }

        // DELETE: api/EntYears/5
        [HttpDelete]
        [Route("api/EntYears/{id:int}")]
        public async Task<IActionResult> DeleteEntYear(int id)
        {
            EntYear entYear = await db.EntYears.FindAsync(id);
            if (entYear == null)
            {
                return NotFound();
            }

            db.EntYears.Remove(entYear);
            await db.SaveChangesAsync();

            return Ok(entYear);
        }
        private bool EntYearExists(int id)
        {
            return db.EntYears.Any(e => e.Id == id);
        }
    }
}