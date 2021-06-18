using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class UsersToTrainings
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Training")]
        public int TrainingId { get; set; }
        [ForeignKey("TrainingStatus")]
        public int TrainingStatusId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Training Training { get; set; }
        public virtual TrainingStatus TrainingStatus { get; set; }
    }
}
