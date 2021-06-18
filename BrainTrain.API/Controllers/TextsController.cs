using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер")]
    public class TextsController : BaseApiController
    {
        public TextsController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/Texts
        [HttpGet]
        [Route("api/Texts")]
        public IQueryable<Text> GetTexts()
        {
            return db.Texts;
        }

        // GET: api/Texts/5
        [HttpGet]
        [Route("api/Texts/{id:int}")]
        public async Task<IActionResult> GetText(int id)
        {
            Text text = await db.Texts.FindAsync(id);
            if (text == null)
            {
                return NotFound();
            }

            return Ok(text);
        }

        // PUT: api/Texts/5
        [HttpPut]
        [Route("api/Texts/{id:int}")]
        public async Task<IActionResult> PutText(int id, Text text)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != text.Id)
            {
                return BadRequest();
            }

            text.TextsToMaterials = null;

            db.Entry(text).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TextExists(id))
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

        // POST: api/Texts
        [HttpPost]
        [Route("api/Texts", Name = "PostText")]
        public async Task<IActionResult> PostText(Text text, int materialId)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            text.TextsToMaterials = new List<TextsToMaterials>();
            text.TextsToMaterials.Add(new TextsToMaterials { MaterialId = materialId });
            text.DateCreated = DateTime.Now;

            db.Texts.Add(text);

            await db.SaveChangesAsync();


            return CreatedAtRoute("PostText", new { id = text.Id }, text);
        }

        // DELETE: api/Texts/5
        [HttpDelete]
        [Route("api/Texts/{id:int}")]
        public async Task<IActionResult> DeleteText(int id)
        {
            Text text = await db.Texts.FindAsync(id);
            if (text == null)
            {
                return NotFound();
            }

            db.Texts.Remove(text);
            await db.SaveChangesAsync();

            return Ok(text);
        }


        private bool TextExists(int id)
        {
            return db.Texts.Count(e => e.Id == id) > 0;
        }
    }
}