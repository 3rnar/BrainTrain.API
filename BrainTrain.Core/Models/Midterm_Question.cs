using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class Midterm_Question
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string LrnJson { get; set; }
        public int? Order { get; set; }
        [Required, ForeignKey("Midterm_Variant")]
        public int VariantId { get; set; }
        [Required]
        public double Weight { get; set; }
        [ForeignKey("Midterm_Language")]
        public int? LanguageId { get; set; }
        [ForeignKey("Midterm_Subject")]
        public int? SubjectId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Midterm_Variant Midterm_Variant { get; set; }
        public virtual Midterm_Language Midterm_Language { get; set; }
        public virtual Midterm_Subject Midterm_Subject { get; set; }
        
        public virtual ICollection<Midterm_UserEventQuestion> Midterm_UserEventQuestions { get; set; }
    }
}
