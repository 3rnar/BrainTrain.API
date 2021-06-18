using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class QuestionsToGoals
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        [ForeignKey("Goal")]
        public int GoalId { get; set; }

        public virtual Question Question { get; set; }
        public virtual Goal Goal { get; set; }
    }
}
