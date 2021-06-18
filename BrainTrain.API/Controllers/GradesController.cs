using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер")]
    public class GradesController : BaseApiController
    {
        private readonly BrainTrainContext db;
        public GradesController(BrainTrainContext _db)
        {
            db = _db;
        }

        // GET: api/Grades
        [HttpGet]
        [Route("api/Grades")]
        public IEnumerable<Grade> GetGrades()
        {
            return db.Grades.ToList();
        }

        // GET: api/Grades/5
        [HttpGet]
        [Route("api/Grades/{id:int}")]
        public async Task<IActionResult> GetGrade(int id)
        {
            Grade grade = await db.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }

            return Ok(grade);
        }

        // PUT: api/Grades/5
        [HttpPut]
        [Route("api/Grades/{id:int}")]
        public async Task<IActionResult> PutGrade(int id, Grade grade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != grade.Id)
            {
                return BadRequest();
            }

            db.Entry(grade).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradeExists(id))
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
        [Route("api/Grades", Name = "PostGrade")]
        public async Task<IActionResult> PostGrade(Grade grade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Grades.Add(grade);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostGrade", new { id = grade.Id }, grade);
        }

        // DELETE: api/Grades/5
        [HttpDelete]
        [Route("api/Grades/{id:int}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            Grade grade = await db.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }

            db.Grades.Remove(grade);
            await db.SaveChangesAsync();

            return Ok(grade);
        }

        private bool GradeExists(int id)
        {
            return db.Grades.Any(e => e.Id == id);
        }
    }
}