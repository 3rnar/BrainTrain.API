using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class QuestionsPagingViewModel
    {
        public IEnumerable<Question> Questions { get; set; }
        public int QuestionsCount { get; set; }
    }
}