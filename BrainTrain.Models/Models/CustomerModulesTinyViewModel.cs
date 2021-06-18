using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class CustomerModulesAndControlWorksViewModel
    {
        public List<CustomerControlWorkViewModel> ControlWorks { get; set; }
        public List<CustomerModulesTinyViewModel> Modules { get; set; }
    }
    public class CustomerModulesTinyViewModel
    {
        public int ModuleId { get; set; }
        public string ModuleTitle { get; set; }
        public DateTime ModuleDeadline { get; set; }
        public double? ModuleKnowingPercentage { get; set; }
        public bool? IsModuleKnown { get; set; }
        public int NumberOfDaysPerModule { get; set; }
    }
    public class CustomerControlWorkViewModel
    {
        public int ControlWorkId { get; set; }
        public int ControlWorkTypeId { get; set; }
        public string ControlWorkTitle { get; set; }
        public List<int> ModuleIds { get; set; }
        public double? KnowingPercentage { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsKnown { get; set; }

        public bool IsAttemptCompleted { get; set; }
        public double GoalResult { get; set; }
        public double BestResult { get; set; }
        public int NumberOfAttempts { get; set; }
    }
}