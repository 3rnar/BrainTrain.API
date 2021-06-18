using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [Route("api/Customer/Events")]
    public class CustomerEventController : BaseApiController
    {
        public CustomerEventController(BrainTrainContext _db) : base(_db)
        {
        }

        [HttpGet]
        [Route("Delivered")]
        public async Task<IActionResult> Delivered(int eventId)
        {
            var e = await db.Events.FirstOrDefaultAsync(ev => ev.Id == eventId);
            e.DateSent = DateTime.Now;
            e.IsSent = true;
            e.IsSeen = true;
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
