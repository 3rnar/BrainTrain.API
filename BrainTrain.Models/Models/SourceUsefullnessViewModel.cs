using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class SourceUsefullnessViewModel
    {
        public int? QuestionId { get; set; }
        public int? MaterialId { get; set; }
        public bool IsLike { get; set; }
    }
}