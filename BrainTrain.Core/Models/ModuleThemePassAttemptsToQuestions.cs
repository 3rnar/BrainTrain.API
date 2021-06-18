using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class ModuleThemePassAttemptsToQuestions
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        [ForeignKey("ModuleThemePassAttempt")]
        public int AttepmtId { get; set; }

        public virtual Question Question { get; set; }
        public virtual ModuleThemePassAttempt ModuleThemePassAttempt { get; set; }
    }
}
