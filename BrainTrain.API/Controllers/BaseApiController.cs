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
        // TODO: Implement memory or Asp.Net cache
        protected string UserId
        {
            get
            {
                return User.Claims.FirstOrDefault(x=>x.Type == "sub").Value;
            }
        }

    }
}