using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер,Заполнение вопросов")]
    public class FilesController : BaseApiController
    {
        public FilesController(BrainTrainContext _db) : base(_db)
        {
        }


        // GET: api/Files
        [HttpGet]
        [Route("api/Files")]
        public IQueryable<File> GetFiles()
        {
            return db.Files;
        }

        // GET: api/Files/5
        [HttpGet]
        [Route("api/Files/{id:int}")]
        public async Task<IActionResult> GetFile(int id)
        {
            File file = await db.Files.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }

            return Ok(file);
        }

        // PUT: api/Files/5
        [HttpPut]
        [Route("api/Files/{id:int}")]
        public async Task<IActionResult> PutFile(int id, File file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != file.Id)
            {
                return BadRequest();
            }

            file.FilesToMaterials = null;
            db.Entry(file).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileExists(id))
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

        // POST: api/Files
        [HttpPost]
        [Route("api/Files", Name = "PostFile")]
        public async Task<IActionResult> PostFile(File file, int materialId)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            file.FilesToMaterials = new List<FilesToMaterials>();
            file.FilesToMaterials.Add(new FilesToMaterials { MaterialId = materialId });
            file.DateCreated = DateTime.Now;

            db.Files.Add(file);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostFile", new { id = file.Id }, file);
        }

        // DELETE: api/Files/5
        [HttpDelete]
        [Route("api/Files/{id:int}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            File file = await db.Files.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }

            db.Files.Remove(file);
            await db.SaveChangesAsync();

            return Ok(file);
        }

        private bool FileExists(int id)
        {
            return db.Files.Any(e => e.Id == id);
        }
    }
}