using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class Midterm_UserEventQuestion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Midterm_UserEvent")]
        public int UserEventId { get; set; }
        [ForeignKey("Midterm_Question")]
        public int QuestionId { get; set; }

        public bool? IsCorrect { get; set; }
        public string Answer { get; set; }
        [ForeignKey("Midterm_QuestionStatus")]
        public int? StatusId { get; set; }

        public virtual Midterm_UserEvent Midterm_UserEvent { get; set; }
        public virtual Midterm_Question Midterm_Question { get; set; }
        public virtual Midterm_QuestionStatus Midterm_QuestionStatus { get; set; }

    }
}
