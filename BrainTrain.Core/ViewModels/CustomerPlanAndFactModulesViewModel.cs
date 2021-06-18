using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class CustomerPlanAndFactModulesViewModel
    {
        public string UserId { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public DateTime PlannedDate { get; set; }
        public DateTime? FactDate { get; set; }
        public double LearningRate { get; set; }
    }
}