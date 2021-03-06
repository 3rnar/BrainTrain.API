using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.API.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected readonly BrainTrainContext db;
        public BaseApiController(BrainTrainContext _db)
        {
            db = _db;
        }
        // TODO: Implement memory or Asp.Net cache
        protected string UserId
        {
            get
            {
                return User.Claims.FirstOrDefault(x=>x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            }
        }

    }
}