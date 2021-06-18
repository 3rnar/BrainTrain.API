using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class Goal
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public virtual ICollection<UsersToGoals> UsersToGoals { get; set; }
        public virtual ICollection<QuestionsToGoals> QuestionsToGoals { get; set; }
        public virtual ICollection<SubjectsToGoals> SubjectsToGoals { get; set; }
    }
}
