using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [Route("api/Customer/MainPage")]
    public class CustomerMainPageController : BaseApiController
    {
        public CustomerMainPageController(BrainTrainContext _db) : base(_db)
        {
        }

        [HttpGet]
        [Route("CustomerSubjects")]
        public async Task<IEnumerable<Subject>> CustomerSubjects()
        {
            var userId = UserId;
            var subjects = await db.Subjects.Where(s => s.UsersToSubjects.Any(uts => uts.UserId == userId)).ToListAsync();
            return subjects;
        }
    }
}
