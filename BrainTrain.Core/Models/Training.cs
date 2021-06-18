using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class Training
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required, ForeignKey("TrainingType")]
        public int TrainingTypeId { get; set; }

        public virtual TrainingType TrainingType { get; set; }
        public virtual ICollection<QuestionsToTrainings> QuestionsToTrainings { get; set; }
    }
}
