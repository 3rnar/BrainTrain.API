using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class CustomerQuestionWithDifficultyViewModel
    {
        public int QuestionId { get; set; }
        public int DifficultyId { get; set; }
        public int ScoreValue { get; set; }
    }

    public class CustomerQuestionWithDifficultyAndCorrectnessViewModel: CustomerQuestionWithDifficultyViewModel
    {
        public bool? IsCorrect { get; set; }
    }

    public class CustomerControlWorkQuestionViewModel: CustomerQuestionWithDifficultyAndCorrectnessViewModel
    {
        public bool IsAnswered { get; set; }
    }
}