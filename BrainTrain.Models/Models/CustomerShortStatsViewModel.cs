using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class CustomerShortStatsViewModel
    {
        public double ModulesCompletedPercent { get; set; }
        public int NumberOfModules { get; set; }
        public int ModuleNumberAt { get; set; }
        public double TasksCompletedPercent { get; set; }
        public int NumberOfTasks { get; set; }
        public int TaskNumberAt { get; set; }
        public double ThemesCompletedPercent { get; set; }
        public int NumberOfThemes { get; set; }
        public int ThemesNumberAt { get; set; }

        public double OverallReadinessPercent { get; set; }
    }
}