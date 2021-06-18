using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class UsersToControlWorks
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, ForeignKey("User")]
        public string UserId { get; set; }
        [Required, ForeignKey("ControlWork")]
        public int ControlWorkId { get; set; }

        public double CurrentLearningRate { get; set; }
        public bool IsCompleted { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ControlWork ControlWork { get; set; }

        public virtual ICollection<UsersToControlWorksToQuestions> UsersToControlWorksToQuestions { get; set; }
    }
}
