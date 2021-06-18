using BrainTrain.API.Helpers;
using BrainTrain.API.Hubs;
using BrainTrain.API.Models;
using BrainTrain.Core.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [RoutePrefix("api/Customer/Events")]
    public class CustomerEventController : BaseApiController
    {
        [HttpGet]
        [Route("Delivered")]
        public async Task<IHttpActionResult> Delivered(int eventId)
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
