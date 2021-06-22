using BrainTrain.API.Dapper;
using BrainTrain.Core.Models;
using BrainTrain.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [Route("api/Customer/Themes")]
    public class CustomerThemesController : BaseApiController
    {
        public CustomerThemesController(BrainTrainContext _db) : base(_db)
        {
        }

        [HttpGet]
        [Route("UserParentThemes")]
        public async Task<IEnumerable<CustomerParentThemesViewModel>> GetThemes(int subjectId, int? gradeId)
        {
            var themes = new StoredProcedure<SqlServer, CustomerParentThemesViewModel>("GetUserParentThemes").ExecResult(
                new FunctionParameter("@UserId", UserId),
                new FunctionParameter("@GradeId", gradeId),
                new FunctionParameter("@SubjectId", subjectId)
                );
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
            var userId = UserId;
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

            var themes = new StoredProcedure<SqlServer, CustomerModuleThemeViewModel>("GetUserParentThemes").ExecResult(
                new FunctionParameter("@UserId", UserId),
                new FunctionParameter("@ModuleId", moduleId),
                new FunctionParameter("@SubjectId", subjectId)
                );

            return themes;
        }
    }
}
