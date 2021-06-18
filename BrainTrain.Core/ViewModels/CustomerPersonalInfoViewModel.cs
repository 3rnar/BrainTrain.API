using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class CustomerPersonalInfoViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsMale { get; set; }
        public string PhoneNumber { get; set; }
        public int? RegionId { get; set; }
        public int? SchoolId { get; set; }
        public string Email { get; set; }
        public int? GradeId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string AvatarUrl { get; set; }
        public double? Experience { get; set; }

        public string SchoolStr { get; set; }
        public string RegionStr { get; set; }

        public School School { get; set; }
        public Region Region { get; set; }

        public Level Level { get; set; }
    }
}