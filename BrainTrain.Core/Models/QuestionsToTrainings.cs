using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class QuestionsToTrainings
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        [ForeignKey("Training")]
        public int TrainingId { get; set; }


        public virtual Question Question { get; set; }
        public virtual Training Training { get; set; }
    }
}
