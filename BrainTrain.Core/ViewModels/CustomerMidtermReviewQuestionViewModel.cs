using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class CustomerMidtermReviewQuestionViewModel: Midterm_Question
    {
        public string UserAnswer { get; set; }
        public bool? IsUserAnswerCorrect { get; set; }
        public int? StatusId { get; set; }
    }
}