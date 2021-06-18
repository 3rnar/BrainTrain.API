using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class CheckingTestStatsViewModel
    {
        public int CorrectAnswersNumber { get; set; }
        public int QuestionsNumber { get; set; }
        public double TotalExperience { get; set; }
        public double ThemeKnowledgePercent { get; set; }
    }
}