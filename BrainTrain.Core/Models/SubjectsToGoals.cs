using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class SubjectsToGoals
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        [ForeignKey("Goal")]
        public int GoalId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Goal Goal { get; set; }
    }
}
