using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class CustomerPlanAndFactThemesViewModel
    {
        public int ThemeId { get; set; }
        public string ThemeTitle { get; set; }
        public DateTime? PlannedDate { get; set; }
        public DateTime? FactDate { get; set; }
        public double? PlannedSubjectLearningPercent { get; set; }
        public double? FactSubjectLearningPercent { get; set; }
    }
}