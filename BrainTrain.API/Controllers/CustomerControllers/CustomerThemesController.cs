using BrainTrain.API.Models;
using BrainTrain.Core.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [RoutePrefix("api/Customer/Themes")]
    public class CustomerThemesController : BaseApiController
    {
        [HttpGet]
        [Route("UserParentThemes")]
        public async Task<IEnumerable<CustomerParentThemesViewModel>> GetThemes(int subjectId, int? gradeId)
        {
            var userId = User.Identity.GetUserId();

            var UserId = new SqlParameter("@UserId", userId);
            var SubjectId = new SqlParameter("@SubjectId", subjectId);
            var GradeId = new SqlParameter("@GradeId", (object)gradeId ?? DBNull.Value);

            var themes = db.Database.SqlQuery<CustomerParentThemesViewModel>("GetUserParentThemes @UserId, @GradeId, @SubjectId", UserId, GradeId, SubjectId).ToList();

            return themes;
        }

        [HttpGet]
        [Route("SubjectThemes")]
        public async Task<IEnumerable<Theme>> GetSubjectThemes(int subjectId)
        {
            var result = db.Themes.Where(t => t.SubjectId == subjectId && t.ParentThemeId != null).Select(t => new
            {
                id = t.Id,
                title = t.Title,
                subjectId = t.SubjectId,
                ParentThemeId = t.ParentThemeId,
                gradeId = t.GradeId,
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
                Subject = new Subject { Id = t.subject.id, Title = t.subject.title },
                ParentTheme = t.parentTheme == null ? null : new Theme { Id = t.parentTheme.id, Title = t.parentTheme.title },
                Grade = new Grade { Id = t.grade.id, Title = t.grade.title }
            }).ToList();

            return result;
        }

        [HttpGet]
        [Route("UserFactAndPlan")]
        public async Task<IEnumerable<CustomerPlanAndFactThemesViewModel>> GetFactAndPlanThemes(int subjectId)
        {
            var userId = User.Identity.GetUserId();
            return db.UsersToThemes.Where(utt => utt.UserId == userId && utt.Theme.SubjectId == subjectId && utt.PredictedDeadLine != null)
                .Select(utt => new CustomerPlanAndFactThemesViewModel
            {
                ThemeId = utt.ThemeId,
                ThemeTitle = utt.Theme.Title,
                PlannedDate = utt.PredictedDeadLine,
                PlannedSubjectLearningPercent = utt.PredictedSubjectLearningRate,
                FactSubjectLearningPercent = utt.ThemeLearnedFactSubjectLearningRate,
                FactDate = utt.ThemeLearnedFactSubjectLearningDate
            }).OrderBy(t => t.PlannedDate).ToList();
        }

        [HttpGet]
        [Route("UserFactAndPlanByUserId")]
        public async Task<IEnumerable<CustomerPlanAndFactThemesViewModel>> GetFactAndPlanThemesByUserId(int subjectId, string userId)
        {
            return db.UsersToThemes.Where(utt => utt.UserId == userId && utt.Theme.SubjectId == subjectId && utt.PredictedDeadLine != null)
                .Select(utt => new CustomerPlanAndFactThemesViewModel
                {
                    ThemeId = utt.ThemeId,
                    ThemeTitle = utt.Theme.Title,
                    PlannedDate = utt.PredictedDeadLine,
                    PlannedSubjectLearningPercent = utt.PredictedSubjectLearningRate,
                    FactSubjectLearningPercent = utt.ThemeLearnedFactSubjectLearningRate,
                    FactDate = utt.ThemeLearnedFactSubjectLearningDate
                }).OrderBy(t => t.PlannedDate).ToList();
        }

        [HttpGet]
        [Route("UserModuleThemes")]
        public async Task<IEnumerable<CustomerModuleThemeViewModel>> GetModuleThemes(int moduleId, int subjectId)
        {
            var userId = User.Identity.GetUserId();

            var UserId = new SqlParameter("@UserId", userId);
            var ModuleId = new SqlParameter("@ModuleId", moduleId);
            var SubjectId = new SqlParameter("@SubjectId", subjectId);

            var themes = db.Database.SqlQuery<CustomerModuleThemeViewModel>("GetUserModuleThemes @UserId, @ModuleId, @SubjectId", UserId, ModuleId, SubjectId).ToList();

            return themes;
        }
    }
}
