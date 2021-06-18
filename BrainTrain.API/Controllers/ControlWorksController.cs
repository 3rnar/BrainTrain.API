using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер")]
    public class ControlWorksController : BaseApiController
    {
        public ControlWorksController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/ControlWorks
        [HttpGet]
        [Route("api/ControlWorks")]
        public IEnumerable<ControlWork> GetControlWorks()
        {
            return db.ControlWorks.
                Select(cw => new {
                    id = cw.Id,
                    title = cw.Title,
                    typeId = cw.TypeId,
                    subjectId = cw.SubjectId,
                    subject = new { Id = cw.Subject.Id, title = cw.Subject.Title },
                    controlWorksToModules = cw.ControlWorksToModules.Select(cwt => new {
                        controlWorkId = cwt.ControlWorkId,
                        moduleId = cwt.ModuleId,
                        module = new { id = cwt.Module.Id, title = cwt.Module.Title }
                    }).ToList()
                }).
                AsEnumerable().
                Select(cw => new ControlWork {
                    Id = cw.id,
                    Title = cw.title,
                    TypeId = cw.typeId,
                    SubjectId = cw.subjectId,
                    Subject = new Subject { Id = cw.subject.Id, Title = cw.subject.title },
                    ControlWorksToModules = cw.controlWorksToModules.Select(cwt => new ControlWorksToModules {
                        ControlWorkId = cwt.controlWorkId,
                        ModuleId = cwt.moduleId,
                        Module = new Module {Id = cwt.module.id, Title = cwt.module.title }
                    }).ToList()
                }).OrderBy(cw => cw.SubjectId).ThenBy(cw => cw.Id).
                ToList();
        }

        // GET: api/ControlWorks/5
        [HttpGet]
        [Route("api/ControlWorks/{id:int}")]
        public async Task<IActionResult> GetControlWork(int id)
        {
            ControlWork controlWork = await db.ControlWorks.FindAsync(id);
            if (controlWork == null)
            {
                return NotFound();
            }

            return Ok(controlWork);
        }

        // PUT: api/ControlWorks/5
        [HttpPut]
        [Route("api/ControlWorks/{id:int}")]
        public async Task<IActionResult> PutControlWork(int id, ControlWork controlWork)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != controlWork.Id)
            {
                return BadRequest();
            }

            db.Entry(controlWork).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ControlWorkExists(id))
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

        // POST: api/ControlWorks
        [HttpPost]
        [Route("api/ControlWorks", Name = "PostControlWork")]
        public async Task<IActionResult> PostControlWork(ControlWork controlWork)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ControlWorks.Add(controlWork);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostControlWork", new { id = controlWork.Id }, controlWork);
        }

        // DELETE: api/ControlWorks/5
        [HttpDelete]
        [Route("api/ControlWorks/{id:int}")]
        public async Task<IActionResult> DeleteControlWork(int id)
        {
            ControlWork controlWork = await db.ControlWorks.FindAsync(id);
            if (controlWork == null)
            {
                return NotFound();
            }

            db.ControlWorks.Remove(controlWork);
            await db.SaveChangesAsync();

            return Ok(controlWork);
        }
        private bool ControlWorkExists(int id)
        {
            return db.ControlWorks.Any(e => e.Id == id);
        }
    }
}