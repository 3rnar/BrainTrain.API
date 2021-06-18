using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarUrl { get; set; }
        public double? Experience { get; set; }
        public string Region { get; set; }
        public string School { get; set; }
        public double? SecondsOnTheSite { get; set; }
        public string Status { get; set; }
        public Level Level { get; set; }
    }
}