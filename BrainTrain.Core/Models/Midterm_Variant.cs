using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class Midterm_Variant
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required, ForeignKey("Midterm_Event")]
        public int EventId { get; set; }
        [ForeignKey("Midterm_Subject")]
        public int? SubjectId { get; set; }

        public virtual Midterm_Event Midterm_Event { get; set; }
        public virtual Midterm_Subject Midterm_Subject { get; set; }
        public virtual ICollection<Midterm_Question> Midterm_Questions { get; set; }
    }
}
