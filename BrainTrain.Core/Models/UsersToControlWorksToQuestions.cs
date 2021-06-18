using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class UsersToControlWorksToQuestions
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("UsersToControlWorks")]
        public int UsersToControlWorksId { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public bool IsAnswered { get; set; }

        public virtual UsersToControlWorks UsersToControlWorks { get; set; }
        public virtual Question Question { get; set; }
    }
}
