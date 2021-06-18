using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class LrnMidtermQuestionsEditViewModel
    {
        public int Id { get; set; }
        public string LrnJson { get; set; }
    }

    public class LrnQuestionsEditViewModel : LrnMidtermQuestionsEditViewModel
    {
        public string Solution { get; set; }
        public int QuestionTypeId { get; set; }
        public int QuestionDifficultyId { get; set; }

        public bool? IsEnt { get; set; }
        public bool? IsModuleQuestion { get; set; }
        public List<int> ThemeIds { get; set; }
    }
}