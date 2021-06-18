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
    public class ModulesController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/Modules
        [HttpGet]
        [Route("api/Modules")]
        public IEnumerable<Module> GetModule()
        {
            var modules = db.Modules.Select(m => new {
                id = m.Id,
                title = m.Title,
                themesToModules = m.ThemesToModules.Select(ttm => new { themeId = ttm.ThemeId, moduleId = ttm.ModuleId, isDominant = ttm.IsDominant, theme = new { id = ttm.Theme.Id, title = ttm.Theme.Title, subjectId = ttm.Theme.SubjectId, gradeId = ttm.Theme.GradeId } }).ToList(),
                moduleType = new { id = m.ModuleType.Id, title = m.ModuleType.Title },
                moduleTypeId = m.ModuleTypeId, 
                subjectId = m.SubjectId
            }).AsEnumerable().Select(m => new Module {
                Id = m.id,
                Title = m.title,
                ThemesToModules = m.themesToModules.Select(ttm => new ThemesToModules { ThemeId = ttm.themeId, ModuleId = ttm.moduleId, IsDominant = ttm.isDominant, Theme = new Theme { Id = ttm.theme.id, Title = ttm.theme.title, SubjectId = ttm.theme.subjectId, GradeId = ttm.theme.gradeId } }).ToList(),
                ModuleType = new ModuleType { Id = m.moduleType.id, Title = m.moduleType.title },
                ModuleTypeId = m.moduleTypeId,
                SubjectId = m.subjectId
            }).ToList();

            return modules;
        }

        // GET: api/Modules/5
        [ResponseType(typeof(Module))]
        [HttpGet]
        [Route("api/Modules/{id:int}")]
        public async Task<IHttpActionResult> GetModule(int id)
        {
            Module module = await db.Modules.Include(m => m.ThemesToModules).FirstOrDefaultAsync(m => m.Id == id);
            if (module == null)
            {
                return NotFound();
            }

            return Ok(module);
        }

        // PUT: api/Modules/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/Modules/{id:int}")]
        public async Task<IHttpActionResult> PutModule(int id, Module module)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != module.Id)
            {
                return BadRequest();
            }

            var dbModule = await db.Modules.Include(m => m.ThemesToModules).FirstOrDefaultAsync(m => m.Id == id);

            if (dbModule.ThemesToModules.Count > 0)
            {
                var toDel =
                    dbModule.ThemesToModules.Where(
                        mtt => mtt.IsDominant == true &&
                            !module.ThemesToModules.Any(
                                qtt =>
                                    qtt.ThemeId == mtt.ThemeId)).ToList();

                if (toDel.Count > 0)
                {
                    //var treeTemes = db.ThemesTrees.ToList();
                    //var dependentToDel = new List<ThemesToModules>(); ;

                    //foreach (var td in toDel)
                    //{
                    //    var dependent = DependentThemesRecursive(treeTemes, td.ThemeId);

                    //    dependentToDel.AddRange(
                    //        dbModule.ThemesToModules.Where(
                    //            mtt => mtt.IsDominant == false &&
                    //                dependent.Any(
                    //                    qtt =>
                    //                        qtt.ThemeId == mtt.ThemeId)).ToList()
                    //        );
                    //}

                    //db.ThemesToModules.RemoveRange(dependentToDel);
                    db.ThemesToModules.RemoveRange(toDel);
                }
                
            }
            foreach (var ttm in module.ThemesToModules)
            {
                if (
                    !dbModule.ThemesToModules.Any(
                        mtt => mtt.ThemeId == ttm.ThemeId && mtt.ModuleId == ttm.ModuleId))
                {
                    db.ThemesToModules.Add(ttm);
                }
            }

            await db.SaveChangesAsync();

            db.Entry(dbModule).State = EntityState.Detached;

            module.ThemesToModules = null;
                                    

            try
            {
                db.Entry(module).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(id))
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

        // POST: api/Modules
        [ResponseType(typeof(Module))]
        [HttpPost]
        [Route("api/Modules", Name = "PostModule")]
        public async Task<IHttpActionResult> PostModule(Module module)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (module.ThemesToModules.Count > 0)
            {
                var treeTemes = db.ThemesTrees.ToList();
                var newThemesToModules = new List<ThemesToModules>();

                foreach (var ttm in module.ThemesToModules)
                {
                    ttm.IsDominant = true;
                    newThemesToModules.AddRange(DependentThemesRecursive(treeTemes, ttm.ThemeId));            
                }

                foreach (var ttm in newThemesToModules)
                {
                    if (module.ThemesToModules.Any(t => t.ThemeId == ttm.ThemeId) == false)
                        module.ThemesToModules.Add(ttm);
                }
            }

            db.Modules.Add(module);
            await db.SaveChangesAsync();

            return CreatedAtRoute("PostModule", new { id = module.Id }, module);
        }

        private List<ThemesToModules> DependentThemesRecursive(List<ThemesTrees> trees, int firstThemeId)
        {
            var list = new List<ThemesToModules>();
            foreach (var item in trees.Where(t => t.FirstThemeId == firstThemeId).ToList())
            {
                list.Add(new ThemesToModules { IsDominant = false, ThemeId = item.SecondThemeId });
                list.AddRange(DependentThemesRecursive(trees, item.SecondThemeId));
            }

            return list;
        }

        // DELETE: api/Modules/5
        [ResponseType(typeof(Module))]
        [HttpDelete]
        [Route("api/Modules/{id:int}")]
        public async Task<IHttpActionResult> DeleteModule(int id)
        {
            Module module = await db.Modules.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            db.Modules.Remove(module);
            await db.SaveChangesAsync();

            return Ok(module);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModuleExists(int id)
        {
            return db.Modules.Count(e => e.Id == id) > 0;
        }
    }
}