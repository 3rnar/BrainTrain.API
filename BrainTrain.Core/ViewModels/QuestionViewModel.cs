using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class QuestionViewModel: Question
    {
        public int NumberOfSolved { get; set; }
        public double AverageSolvingSeconds { get; set; }
    }
}