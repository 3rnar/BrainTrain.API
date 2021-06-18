using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class UsersToSubjects
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public bool IsEntrantTestPassed { get; set; }
        public int? DesiredScore { get; set; }
        public double? CurrentLearningRate { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Subject Subject { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
    }
}
