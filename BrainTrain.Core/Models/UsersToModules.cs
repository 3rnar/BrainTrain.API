using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class UsersToModules
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Module")]
        public int ModuleId { get; set; }
        [Required]
        public DateTime PredictedDeadLine { get; set; }
        [Required]
        public double CurrentLearningRate { get; set; }
        public DateTime? FactLearnedDate { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Module Module { get; set; }
    }
}
