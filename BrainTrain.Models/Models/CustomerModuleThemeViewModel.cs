using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class CustomerModuleThemeViewModel
    {
        public int? ThemeId { get; set; }
        public string ThemeTitle { get; set; }
        public string ThemeImageUrl { get; set; }

        public int? NumberOfAttempts { get; set; }
        public bool? IsAttemptFinished { get; set; }
        public double? PassingGoal { get; set; }
        public double? ThemeKnowingPercentage { get; set; }
        public bool? IsThemeKnown { get; set; }
        public int? DependentThemeId { get; set; }

        public int? TextCount { get; set; }
        public int? VidCount { get; set; }
        public int? FileCount { get; set; }
    }
}