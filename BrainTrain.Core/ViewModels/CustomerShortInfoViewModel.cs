using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class CustomerShortInfoViewModel
    {
        public string UserId { get; set; }
        public string AvatarUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int? RegionId { get; set; }
        public string RegionStr { get; set; }
        public string Region { get; set; }
        public int? SchoolId { get; set; }
        public string SchoolStr { get; set; }
        public string School { get; set; }
        public double Experience { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int? BattleId { get; set; }
        public Level Level { get; set; }
    }
}