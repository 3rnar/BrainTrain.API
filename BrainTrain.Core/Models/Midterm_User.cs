using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class Midterm_User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string SystemId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required, ForeignKey("Midterm_Language")]
        public int LanguageId { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [ForeignKey("Midterm_Teacher")]
        public int? TeacherId { get; set; }

        public virtual ICollection<Midterm_UserEvent> Midterm_UserEvents { get; set; }
        public virtual Midterm_Language Midterm_Language { get; set; }
        public virtual Midterm_Teacher Midterm_Teacher { get; set; }
    }
}
