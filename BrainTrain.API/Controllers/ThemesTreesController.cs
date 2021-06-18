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
using BrainTrain.API.Models;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер")]
    public class ThemesTreesController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        // GET: api/ThemesTrees
        [HttpGet]
        [Route("api/ThemesTrees")]
        public IEnumerable<TreeViewModel> GetThemesTrees(int subjectId)
        {
            var themeTrees = db.ThemesTrees.Where(tt => tt.FirstTheme.SubjectId == subjectId && tt.SecondTheme.SubjectId == subjectId).Select(tt => new
            {
                firstThemeId = tt.FirstThemeId,
                secondThemeId = tt.SecondThemeId,
                firstTheme = new { id = tt.FirstTheme.Id, title = tt.FirstTheme.Title},
                secondTheme =  new { id = tt.SecondTheme.Id, title = tt.SecondTheme.Title }
            }).AsEnumerable().Select(tt => new ThemesTrees {
                FirstThemeId = tt.firstThemeId,
                SecondThemeId = tt.secondThemeId,
                FirstTheme = new Theme { Id = tt.firstTheme.id, Title = tt.firstTheme.title },
                SecondTheme = new Theme { Id = tt.secondTheme.id, Title = tt.secondTheme.title }
            }).ToList();

            
            var firstLineItems = themeTrees.Where(tt => !themeTrees.Any(tt2 => tt2.SecondThemeId == tt.FirstThemeId)).Select(tt => tt.FirstThemeId).Distinct().ToList();
            var trees = new List<TreeViewModel>();

            foreach (var fli in firstLineItems)
            {
                var theme = db.Themes.FirstOrDefault(t => t.Id == fli);
                var fliTree = new TreeViewModel { text = theme.Title, value = $"{fli}", collapsed = true, children = ChildTreeViewRecursive(themeTrees, fli)};
                trees.Add(fliTree);
            }

            return trees;
        }

        private List<TreeViewModel> ChildTreeViewRecursive(IEnumerable<ThemesTrees> themeTrees, int firstThemeId)
        {
            var trees = new List<TreeViewModel>();

            foreach (var themeTree in themeTrees.Where(tt => tt.FirstThemeId == firstThemeId).ToList())
            {
                var parentTree = new TreeViewModel { text = themeTree.SecondTheme.Title, value = $"{firstThemeId}-{themeTree.SecondThemeId}", collapsed = true, children = ChildTreeViewRecursive(themeTrees, themeTree.SecondThemeId)};
                trees.Add(parentTree);
            }

            return trees;
        }

        [HttpGet]
        [Route("api/ThemesTrees/OrgTable")]
        public IEnumerable<OrgTableTreeViewModel> GetThemesTreesOrgTable(int subjectId)
        {
            var themeTrees = db.ThemesTrees.Where(tt => tt.FirstTheme.SubjectId == subjectId && tt.SecondTheme.SubjectId == subjectId).Select(tt => new
            {
                firstThemeId = tt.FirstThemeId,
                secondThemeId = tt.SecondThemeId,
                firstTheme = new { id = tt.FirstTheme.Id, title = tt.FirstTheme.Title },
                secondTheme = new { id = tt.SecondTheme.Id, title = tt.SecondTheme.Title }
            }).AsEnumerable().Select(tt => new ThemesTrees
            {
                FirstThemeId = tt.firstThemeId,
                SecondThemeId = tt.secondThemeId,
                FirstTheme = new Theme { Id = tt.firstTheme.id, Title = tt.firstTheme.title },
                SecondTheme = new Theme { Id = tt.secondTheme.id, Title = tt.secondTheme.title }
            }).ToList();


            var firstLineItems = themeTrees.Where(tt => !themeTrees.Any(tt2 => tt2.SecondThemeId == tt.FirstThemeId)).Select(tt => tt.FirstThemeId).Distinct().ToList();
            var trees = new List<OrgTableTreeViewModel>();
            trees.Add(new OrgTableTreeViewModel { expanded = true, label= "Корень", children = new List<OrgTableTreeViewModel>() });

            foreach (var fli in firstLineItems)
            {
                var theme = db.Themes.FirstOrDefault(t => t.Id == fli);
                var fliTree = new OrgTableTreeViewModel { expanded = false, label = theme.Title, children = ChildOrgTableTreeViewRecursive(themeTrees, fli) };
                trees[0].children.Add(fliTree);
            }

            return trees;
        }

        private List<OrgTableTreeViewModel> ChildOrgTableTreeViewRecursive(IEnumerable<ThemesTrees> themeTrees, int firstThemeId)
        {
            var trees = new List<OrgTableTreeViewModel>();

            foreach (var themeTree in themeTrees.Where(tt => tt.FirstThemeId == firstThemeId).ToList())
            {
                var parentTree = new OrgTableTreeViewModel { expanded = false, label = themeTree.SecondTheme.Title, children = ChildOrgTableTreeViewRecursive(themeTrees, themeTree.SecondThemeId) };
                trees.Add(parentTree);
            }

            return trees;
        }

        // POST: api/ThemesTrees
        [HttpPost]
        [Route("api/ThemesTrees", Name = "PostThemesTrees")]
        public async Task<IHttpActionResult> PostThemesTrees(ThemesTrees[] themesTrees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var themesTree in themesTrees)
            {
                if (!db.ThemesTrees.Any(tt => tt.FirstThemeId == themesTree.FirstThemeId && tt.SecondThemeId == themesTree.SecondThemeId))
                    db.ThemesTrees.Add(themesTree);
            }            

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE: api/ThemesTrees/5
        [ResponseType(typeof(ThemesTrees))]
        [HttpDelete]
        [Route("api/ThemesTrees")]
        public async Task<IHttpActionResult> DeleteThemesTrees(int firstThemeId, int secondThemeId)
        {
            ThemesTrees themesTrees = await db.ThemesTrees.FirstOrDefaultAsync(tt => tt.FirstThemeId == firstThemeId && tt.SecondThemeId == secondThemeId);
            if (themesTrees == null)
            {
                return NotFound();
            }

            db.ThemesTrees.Remove(themesTrees);
            await db.SaveChangesAsync();

            return Ok(themesTrees);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ThemesTreesExists(int firstThemeId, int secondThemeId)
        {
            return db.ThemesTrees.Count(e => e.FirstThemeId == firstThemeId && e.SecondThemeId == secondThemeId) > 0;
        }
    }
}