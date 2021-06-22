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
    [Authorize(Roles = "Контент-менеджер")]
    public class VideosController : BaseApiController
    {
        public VideosController(BrainTrainContext _db) : base(_db)
        {
        }

        // GET: api/Videos
        [HttpGet]
        [Route("api/Videos")]
        public IQueryable<Video> GetVideos()
        {
            return db.Videos;
        }

        // GET: api/Videos/5
        [HttpGet]
        [Route("api/Videos/{id:int}")]
        public async Task<IActionResult> GetVideo(int id)
        {
            Video video = await db.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            return Ok(video);
        }

        // PUT: api/Videos/5
        [HttpPut]
        [Route("api/Videos/{id:int}")]
        public async Task<IActionResult> PutVideo(int id, Video video)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != video.Id)
            {
                return BadRequest();
            }

            video.VideosToMaterials = null;
            db.Entry(video).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
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

        // POST: api/Videos
        [HttpPost]
        [Route("api/Videos", Name = "PostVideo")]
        public async Task<IActionResult> PostVideo(Video video, int materialId)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            video.VideosToMaterials = new List<VideosToMaterials>();
            video.VideosToMaterials.Add(new VideosToMaterials { MaterialId = materialId });
            video.DateCreated = DateTime.Now;

            db.Videos.Add(video);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostVideo", new { id = video.Id }, video);
        }

        // DELETE: api/Videos/5
        [HttpDelete]
        [Route("api/Videos/{id:int}")]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            Video video = await db.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            db.Videos.Remove(video);
            await db.SaveChangesAsync();

            return Ok(video);
        }


        private bool VideoExists(int id)
        {
            return db.Videos.Count(e => e.Id == id) > 0;
        }
    }
}