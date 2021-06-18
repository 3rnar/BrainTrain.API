using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BrainTrain.Models.Models;

namespace BrainTrain.API.Controllers
{
    [Authorize (Roles = "Контент-менеджер,Заполнение вопросов")]
    //[RoutePrefix("api/AdminDashboard")]
    public class AdminDashboardController : BaseApiController
    {
        private readonly BrainTrainContext db;
        public AdminDashboardController(BrainTrainContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Route("api/AdminDashboard/MainPage")]
        public async Task<IActionResult> MainPage()
        {
            var dashboardModel = await db.Set<AdminDashboardViewModel>().FromSqlRaw("EXEC GetAdminDashboardMainData").ToListAsync();


            return Ok(dashboardModel);
        }

        [HttpGet]
        [Route("api/AdminDashboard/ManagersByQuestion")]
        public async Task<IEnumerable<ManagersByQuestionsViewModel>> ManagersByQuestions(DateTime fromDate, DateTime toDate)
        {
            var mq = db.ApplicationUsers.Select(u => new ManagersByQuestionsViewModel {
                UserId = u.Id, UserName = u.UserName,
                QuestionsAdded = db.Questions.Where(q => q.ContentManagerId == u.Id 
                && EF.Functions.DateDiffDay(fromDate, q.DateCreated) >= 0
                && EF.Functions.DateDiffDay(toDate, q.DateCreated) <= 0
                ).Count(),
                QuestionsChecked = db.Questions.Where(q => q.IsChecked == true && q.ContentManagerId == u.Id
                && EF.Functions.DateDiffDay(fromDate, q.DateCreated) >= 0
                && EF.Functions.DateDiffDay(toDate, q.DateCreated) <= 0
                ).Count()
            } ).ToList();

            return mq.Where(m => m.QuestionsAdded > 0).OrderByDescending(m => m.QuestionsAdded).ToList();
        }
    }
}
