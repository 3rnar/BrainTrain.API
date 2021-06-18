using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class UsersToGoals
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Goal")]
        public int GoalId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Goal Goal { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
    }
}
