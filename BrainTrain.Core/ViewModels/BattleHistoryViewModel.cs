using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class BattleHistoryViewModel
    {
        public int BattleId { get; set; }
        public string OpponentName { get; set; }
        public string BattleResult { get; set; }
        public DateTime DateCreated { get; set; }
        public string WinnerName { get; set; }
        public string OpponentAvatarUrl { get; set; }
        public string SubjectTitle { get; set; }
        public string OpponentFirstName { get; set; }
        public string OpponentLastName { get; set; }
    }
}