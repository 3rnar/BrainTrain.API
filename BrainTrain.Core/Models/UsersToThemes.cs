using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class UsersToThemes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Theme")]
        public int ThemeId { get; set; }
        [Required]
        public double CurrentLearningRate { get; set; }

        public DateTime? PredictedDeadLine { get; set; }
        public double? PredictedSubjectLearningRate { get; set; }
        public bool? IsThemeLearned { get; set; }
        public double? ThemeLearnedFactSubjectLearningRate { get; set; }
        public DateTime? ThemeLearnedFactSubjectLearningDate { get; set; }
        public bool? IsHidden { get; set; }
        public double? CurrentOvarallLearningRate { get; set; }

        [ForeignKey("Module")]
        public int? ModuleId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Theme Theme { get; set; }
        public virtual Module Module { get; set; }
    }
}
