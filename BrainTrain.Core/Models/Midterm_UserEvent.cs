using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class Midterm_UserEvent
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, ForeignKey("Midterm_Event")]
        public int EventId { get; set; }
        [Required, ForeignKey("Midterm_User")]
        public int UserId { get; set; }
        [ForeignKey("Midterm_UserStatus")]
        public int? StatusId { get; set; }
        public double? FinalResult { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public double? SecondsSpent { get; set; }
        public bool? IsPrinted { get; set; }

        public virtual Midterm_Event Midterm_Event { get; set; }
        public virtual Midterm_User Midterm_User { get; set; }
        public virtual Midterm_UserStatus Midterm_UserStatus { get; set; }

        public virtual ICollection<Midterm_UserEventQuestion> Midterm_UserEventQuestions { get; set; }
    }
}
