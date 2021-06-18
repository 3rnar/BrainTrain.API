using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    [NotMapped]
    public class AdminDashboardViewModel
    {
        public int TodayMaterials { get; set; }
        public int TodayQuestions { get; set; }
        public int TodayRegistrations { get; set; }
        public int TodayLogins { get; set; }
    }
}