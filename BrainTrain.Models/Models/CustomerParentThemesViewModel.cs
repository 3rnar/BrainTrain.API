using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class CustomerParentThemesViewModel
    {
        public int ParentThemeId { get; set; }
        public string ParentThemeTitle { get; set; }
        public int? ThemeId { get; set; }
        public string ThemeTitle { get; set; }
        public string ThemeImageUrl { get; set; }
        public int? TextId { get; set; }
        public string TextTitle { get; set; }
        public int? VideoId { get; set; }
        public string VideoTitle { get; set; }
        public int? FileId { get; set; }
        public string FileTitle { get; set; }
        public double? ThemeKnowingPercentage { get; set; }
        public bool? IsThemeKnown { get; set; }
        public double? ParentThemeKnowingPercentage { get; set; }

        public int? TextCount { get; set; }
        public int? VidCount { get; set; }
        public int? FileCount { get; set; }
    }
}