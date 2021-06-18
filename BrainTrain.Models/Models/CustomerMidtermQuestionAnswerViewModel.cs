using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class CustomerMidtermQuestionAnswerViewModel
    {
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }
        public string Answer { get; set; }
    }
}