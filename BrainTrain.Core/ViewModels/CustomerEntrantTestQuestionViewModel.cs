using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class CustomerEntrantTestQuestionViewModel
    {
        public int QuestionId { get; set; }
        public bool IsAnswered { get; set; }
        public bool? IsCorrect { get; set; }
    }
}