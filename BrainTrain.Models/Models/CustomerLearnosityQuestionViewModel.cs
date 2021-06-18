using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class CustomerLearnosityQuestionViewModel
    {
        public string LearnosityJson { get; set; }
        public int QuestionId { get; set; }
        public double QuestionExperience { get; set; }

        public QuestionDifficulty QuestionDifficulty { get; set; }
        public List<QuestionsToThemes> QuestionsToThemes { get; set; }
        public List<Solution> Solutions { get; set; }
        public int NumberOfSolved { get; set; }
        public double AverageSolvingSeconds { get; set; }
        public int QuestionDifficultyId { get; set; }
                
    }

    public class CustomerLearnosityModuleQuestionViewModel: CustomerLearnosityQuestionViewModel
    {
        public int AttemptId { get; set; }
        public int QuestionNumber { get; set; }
        public int NumberOfQuestions { get; set; }
        public double Score { get; set; }
        public List<CustomerQuestionWithDifficultyAndCorrectnessViewModel> QuestionIds { get; set; }
    }
}