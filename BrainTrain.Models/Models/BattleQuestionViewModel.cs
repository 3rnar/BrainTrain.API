using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class BattleQuestionViewModel
    {

        public int Id { get; set; }
        public string Text { get; set; }

        public Solution Solution { get; set; }

        public List<QuestionVariant> Variants { get; set; }


        public QuestionAnswer FirstUserAnswer { get; set; }
        public double FirstUserMark { get; set; }
        public QuestionAnswer SecondUserAnswer { get; set; }
        public double SecondUserMark { get; set; }
        public string ThemeTitle { get; set; }
        public int ThemeId { get; set; }
    }
}