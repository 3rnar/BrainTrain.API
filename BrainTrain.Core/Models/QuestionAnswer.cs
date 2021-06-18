using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class QuestionAnswer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, ForeignKey("Question")]
        public int QuestionId { get; set; }
        [Required, ForeignKey("User")]
        public string UserId { get; set; }
        public string Value { get; set; }
        [ForeignKey("AnswerSource")]
        public int AnswerSourceId { get; set; }
        public DateTime DateCreated { get; set; }
        public int? SolvingSeconds { get; set; }
        public int? NumberOfAttempts { get; set; }
        [ForeignKey("ModuleThemePassAttempt")]
        public int? ModuleThemePassAttemptId { get; set; }
        public bool? IsViewed { get; set; }
        public bool? IsCorrect { get; set; }

        public virtual Question Question { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<QuestionAnswerVariant> QuestionAnswerVariants { get; set; }
        public virtual ICollection<QuestionAnswerLrnVariant> QuestionAnswerLrnVariants { get; set; }
        public virtual AnswerSource AnswerSource { get; set; }
        public virtual ModuleThemePassAttempt ModuleThemePassAttempt { get; set; }
    }
}
