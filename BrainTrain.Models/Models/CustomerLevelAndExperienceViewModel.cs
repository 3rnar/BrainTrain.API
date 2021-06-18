using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class CustomerLevelAndExperienceViewModel
    {
        public double? Experience { get; set; }
        public int? LevelId { get; set; }
        public string LevelTitle { get; set; }
        public int? NextLevelId { get; set; }
        public string NextLevelTitle { get; set; }
        public double? TillNextLevel { get; set; }
        public double? BetweenCurrentAndNextLevel { get; set; }
    }
}