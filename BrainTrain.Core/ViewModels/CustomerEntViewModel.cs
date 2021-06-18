using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class CustomerLearnosityEntQuestionViewModel
    {
        public double KnowingPercentage { get; set; }
        public string LearnosityJson { get; set; }

        public List<CustomerEntQuestionAnswerViewModel> QuestionAnswers { get; set; }        
    }

    public class CustomerEntQuestionAnswerViewModel
    {
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }
        public string AnsweredValue { get; set; }

        public string Solution { get; set; }
        public string CorrectAnswerValue { get; set; }
        public List<Theme> Themes { get; set; }
    }

    public class CustomerEntYearViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<CustomerEntVariantViewModel> EntVariants { get; set; }
    }

    public class CustomerEntVariantViewModel: EntVariant
    {
        public int NumberOfQuestions { get; set; }
        public int NumberOfCorrectAnswers { get; set; }
        public double KnowingPercentage { get; set; }
    }
}