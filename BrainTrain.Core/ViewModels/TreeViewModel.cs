using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class TreeViewModel
    {
        public string text { get; set; }
        public string value { get; set; }
        public bool collapsed { get; set; }

        public List<TreeViewModel> children { get; set; }
    }

    public class OrgTableTreeViewModel
    {
        public string label { get; set; }
        public bool expanded { get; set; }
        public List<OrgTableTreeViewModel> children { get; set; }

    }
}