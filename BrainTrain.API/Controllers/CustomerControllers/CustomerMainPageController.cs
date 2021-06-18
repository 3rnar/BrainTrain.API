using BrainTrain.Core.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [RoutePrefix("api/Customer/MainPage")]
    public class CustomerMainPageController : BaseApiController
    {
        [HttpGet]
        [Route("CustomerSubjects")]
        public async Task<IEnumerable<Subject>> CustomerSubjects()
        {
            var userId = User.Identity.GetUserId();
            var subjects = await db.Subjects.Where(s => s.UsersToSubjects.Any(uts => uts.UserId == userId)).ToListAsync();
            return subjects;
        }
    }
}
