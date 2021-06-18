using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class CustomerModulesViewModel
    {
        public int ModuleId { get; set; }
        public string ModuleTitle { get; set; }
        public DateTime ModuleDeadline { get; set; }

        public double? ModuleKnowingPercentage { get; set; }
        public bool? IsModuleKnown { get; set; }


        public int? ThemeId { get; set; }
        public string ThemeTitle { get; set; }
        public string ThemeImageUrl { get; set; }

        public int? NumberOfAttempts { get; set; }
        public bool? IsAttemptFinished { get; set; }
        public double? PassingGoal { get; set; }
        public double? ThemeKnowingPercentage { get; set; }
        public bool? IsThemeKnown { get; set; }
        public int? DependentThemeId { get; set; }

        public int? TextId { get; set; }
        public string TextTitle { get; set; }
        public int? VideoId { get; set; }
        public string VideoTitle { get; set; }
        public int? FileId { get; set; }
        public string FileTitle { get; set; }

        public int? TextCount { get; set; }
        public int? VidCount { get; set; }
        public int? FileCount { get; set; }

        public int NumberOfDaysPerModule { get; set; }

        public DateTime? ModuleFinishDate { get; set; }
    }
}