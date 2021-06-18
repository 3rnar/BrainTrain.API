using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class CustomerThemeKnowledgeLevelViewModel
    {
        public int ThemeId { get; set; }
        public string ThemeTitle { get; set; }
        public int ParentThemeId { get; set; }
        public double? ThemeKnowingPercentage { get; set; }
        public int? IsThemeKnown { get; set; }
    }
}