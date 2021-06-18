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
using Microsoft.AspNet.Identity;
using BrainTrain.API.Models;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер")]
    public class MaterialsController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        //[HttpGet]
        //[Route("api/Materials/Count")]
        //public int GetMaterialsCount(string searchStr)
        //{
        //    return ;
        //}

        // GET: api/Materials
        [HttpGet]
        [Route("api/Materials")]
        public MaterialsPagingViewModel GetMaterials(int pageNum, int perPage, int? themeId, string searchStr)
        {
            var mats = db.Materials.Where(m => 
            (string.IsNullOrEmpty(searchStr) || m.Title.ToLower().Contains(searchStr.ToLower().Trim())) && 
            (themeId == null || m.MaterialsToThemes.Any(mtt => mtt.ThemeId == themeId))).
            OrderByDescending(m => m.Id).Skip((pageNum - 1) * perPage).Take(perPage).Select(m => new
            {
                id = m.Id,
                title = m.Title,
                contentManagerId = m.ContentManagerId,
                materialTypeId = m.MaterialTypeId,
                materialAuthorId = m.MaterialAuthorId,
                materialType = new { id = m.MaterialType.Id, title = m.MaterialType.Title },
                materialAuthor = new { id = m.MaterialAuthor.Id, title = m.MaterialAuthor.Title },
                keywordsToMaterials = m.KeywordsToMaterials.Select(ktm => new
                {
                    materialId = ktm.MaterialId,
                    keywordId = ktm.KeywordId,
                    keyword = new { id = ktm.Keyword.Id, text = ktm.Keyword.Text }
                }),
                videosToMaterials = m.VideosToMaterials.Select(vtm => new
                {
                    materialId = vtm.MaterialId,
                    videoId = vtm.VideoId,
                    video = new
                    {
                        id = vtm.Video.Id,
                        title = vtm.Video.Title,
                        preText = vtm.Video.PreText,
                        //url = vtm.Video.Url,
                        DateCreated = vtm.Video.DateCreated,
                        openQuestion = vtm.Video.OpenQuestionsToMaterialParts.Select(oq => new { id = oq.Id, videoId = oq.VideoId, questionId = oq.QuestionId })
                    }
                }),
                filesToMaterials = m.FilesToMaterials.Select(ftm => new
                {
                    materialId = ftm.MaterialId,
                    fileId = ftm.FileId,
                    file = new
                    {
                        id = ftm.File.Id,
                        title = ftm.File.Title,
                        preText = ftm.File.PreText,
                        //url = ftm.File.Url,
                        DateCreated = ftm.File.DateCreated,
                        openQuestion = ftm.File.OpenQuestionsToMaterialParts.Select(oq => new { id = oq.Id, fileId = oq.FileId, questionId = oq.QuestionId })
                    }
                }),
                textsToMaterials = m.TextsToMaterials.Select(ttm => new
                {
                    materialId = ttm.MaterialId,
                    textId = ttm.TextId,
                    text = new
                    {
                        id = ttm.Text.Id,
                        title = ttm.Text.Title,
                        preText = ttm.Text.PreText,
                        //fullText = ttm.Text.FullText,
                        DateCreated = ttm.Text.DateCreated,
                        openQuestion = ttm.Text.OpenQuestionsToMaterialParts.Select(oq => new { id = oq.Id, textId = oq.TextId, questionId = oq.QuestionId })
                    }
                }),
                materialsToThemes = m.MaterialsToThemes.Select(mtt => new
                {
                    themeId = mtt.ThemeId,
                    materialId = mtt.MaterialId,
                    theme = new { id = mtt.Theme.Id, title = mtt.Theme.Title, gradeId = mtt.Theme.GradeId }
                })
            }).AsEnumerable().Select(m => new Material
            {
                Id = m.id,
                Title = m.title,
                ContentManagerId = m.contentManagerId,
                MaterialTypeId = m.materialTypeId,
                MaterialAuthorId = m.materialAuthorId,
                MaterialAuthor = new MaterialAuthor { Id = m.materialAuthor.id, Title = m.materialAuthor.title },
                MaterialType = new MaterialType { Id = m.materialType.id, Title = m.materialType.title },
                KeywordsToMaterials = m.keywordsToMaterials.Select(ktw => new KeywordsToMaterials { KeywordId = ktw.keywordId, MaterialId = ktw.materialId, Keyword = new Keyword { Id = ktw.keyword.id, Text = ktw.keyword.text } }).ToList(),
                VideosToMaterials = m.videosToMaterials.Select(vtm => new VideosToMaterials { MaterialId =  vtm.materialId, VideoId = vtm.videoId, Video = new Video { Id = vtm.video.id, Title = vtm.video.title, PreText = vtm.video.preText, /*Url = vtm.video.url,*/ DateCreated = vtm.video.DateCreated, OpenQuestionsToMaterialParts = vtm.video.openQuestion.Select(oq => new OpenQuestionsToMaterialParts { Id = oq.id, VideoId = oq.videoId, QuestionId = oq.questionId }).ToList() } }).ToList(),
                FilesToMaterials = m.filesToMaterials.Select(ftm => new FilesToMaterials { MaterialId = ftm.materialId, FileId = ftm.fileId, File = new File { Id = ftm.file.id, Title = ftm.file.title, PreText = ftm.file.preText, /*Url = ftm.file.url, */ DateCreated = ftm.file.DateCreated, OpenQuestionsToMaterialParts = ftm.file.openQuestion.Select(oq => new OpenQuestionsToMaterialParts { Id = oq.id, FileId = oq.fileId, QuestionId = oq.questionId }).ToList() } }).ToList(),
                TextsToMaterials = m.textsToMaterials.Select(ttm => new TextsToMaterials { MaterialId = ttm.materialId, TextId = ttm.textId, Text = new Text { Id = ttm.text.id, Title = ttm.text.title, PreText = ttm.text.preText, /*FullText = ttm.text.fullText,*/ DateCreated = ttm.text.DateCreated, OpenQuestionsToMaterialParts = ttm.text.openQuestion.Select(oq => new OpenQuestionsToMaterialParts { Id = oq.id, VideoId = oq.textId, QuestionId = oq.questionId }).ToList() } }).ToList(),
                MaterialsToThemes = m.materialsToThemes.Select(mtt => new MaterialsToThemes { ThemeId = mtt.themeId, MaterialId = mtt.materialId, Theme = new Theme { Id = mtt.theme.id, Title = mtt.theme.title, GradeId = mtt.theme.gradeId } }).ToList()
            }).ToList();

            var matModel = new MaterialsPagingViewModel
            {
                Materials = mats,
                MaterialsCount = db.Materials.Where(m => (string.IsNullOrEmpty(searchStr) || m.Title.ToLower().Contains(searchStr.ToLower().Trim())) && (themeId == null || m.MaterialsToThemes.Any(mtt => mtt.ThemeId == themeId))).Count()
            };

            return matModel;
        }

        // GET: api/Materials/5
        [HttpGet]
        [Route("api/Materials/{id:int}")]
        [ResponseType(typeof(Material))]
        public async Task<IHttpActionResult> GetMaterial(int id)
        {
            Material material = await db.Materials.Include(m => m.MaterialsToThemes).Include(m => m.KeywordsToMaterials).FirstOrDefaultAsync(m => m.Id == id);

            if (material == null)
            {
                return NotFound();
            }

            return Ok(material);
        }

        // PUT: api/Materials/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/Materials/{id:int}")]
        public async Task<IHttpActionResult> PutMaterial(int id, Material material)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (id != material.Id)
            {
                return BadRequest();
            }

            var mat = await db.Materials.Include(m => m.MaterialsToThemes).Include(m => m.KeywordsToMaterials).FirstOrDefaultAsync(m => m.Id == id);
            if (mat == null)
                return NotFound();

            if (mat.MaterialsToThemes.Count > 0)
            {
                var toDel =
                    mat.MaterialsToThemes.Where(
                        mtt =>
                            !material.MaterialsToThemes.Any(
                                materialsToThemes =>
                                    materialsToThemes.ThemeId == mtt.ThemeId &&
                                    materialsToThemes.MaterialId == mtt.MaterialId)).ToList();
                db.MaterialsToThemes.RemoveRange(toDel);
            }
            foreach (var materialsToThemes in material.MaterialsToThemes)
            {
                if (
                    !mat.MaterialsToThemes.Any(
                        mtt => mtt.ThemeId == materialsToThemes.ThemeId && mtt.MaterialId == materialsToThemes.MaterialId))
                {
                    db.MaterialsToThemes.Add(materialsToThemes);
                }
            }

            if (mat.KeywordsToMaterials.Count > 0)
            {
                var toDel =
                    mat.KeywordsToMaterials.Where(
                        ktm =>
                            !material.KeywordsToMaterials.Any(
                                keywordsToMaterials =>
                                    keywordsToMaterials.KeywordId == ktm.KeywordId &&
                                    keywordsToMaterials.MaterialId == ktm.MaterialId)).ToList();
                db.KeywordsToMaterials.RemoveRange(toDel);
            }
            foreach (var keywordsToMaterials in material.KeywordsToMaterials)
            {
                if (
                    !mat.KeywordsToMaterials.Any(
                        ktm => ktm.KeywordId == keywordsToMaterials.KeywordId && ktm.MaterialId == keywordsToMaterials.MaterialId))
                {
                    db.KeywordsToMaterials.Add(keywordsToMaterials);
                }
            }

            await db.SaveChangesAsync();

            db.Entry(mat).State = EntityState.Detached;

            material.MaterialsToThemes = null;
            material.KeywordsToMaterials = null;

            db.Entry(material).State = EntityState.Modified;
          
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
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

        // POST: api/Materials
        [ResponseType(typeof(Material))]
        [HttpPost]
        [Route("api/Materials", Name = "PostMaterial")]
        public async Task<IHttpActionResult> PostMaterial(Material material)
        {
            material.ContentManagerId = User.Identity.GetUserId();

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            

            db.Materials.Add(material);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostMaterial", new { id = material.Id }, material);
        }

        // DELETE: api/Materials/5
        [ResponseType(typeof(Material))]
        [HttpDelete]
        [Route("api/Materials/{id:int}")]
        public async Task<IHttpActionResult> DeleteMaterial(int id)
        {
            Material material = await db.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }



            db.Materials.Remove(material);
            await db.SaveChangesAsync();

            return Ok(material);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MaterialExists(int id)
        {
            return db.Materials.Count(e => e.Id == id) > 0;
        }
    }
}