using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class SolvedQuestionsToAllQuestionsViewModel
    {
        public int NumberOfSolvedQuestions { get; set; }
        public int NumberOfQuestions { get; set; }
        public double Relation { get; set; }
    }
}