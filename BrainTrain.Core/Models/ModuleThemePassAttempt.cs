using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class ModuleThemePassAttempt
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
  
        [Required, ForeignKey("User")]
        public string UserId { get; set; }
        [Required, ForeignKey("Theme")]
        public int ThemeId { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public bool? IsFinished { get; set; }
        public double CurrentScore { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Theme Theme { get; set; }
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual ICollection<ModuleThemePassAttemptsToQuestions> AttemptsToQuestions { get; set; }
    }
}
