using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class ManagersByQuestionsViewModel
    {
        public string UserId  { get; set; }
        public string UserName { get; set; }
        public int QuestionsAdded { get; set; }
        public int QuestionsChecked { get; set; }
    }
}