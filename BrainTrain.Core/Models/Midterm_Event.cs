using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class Midterm_Event
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime DateHold { get; set; }

        public virtual ICollection<Midterm_UserEvent> Midterm_UserEvents { get; set; }
        public virtual ICollection<Midterm_Variant> Midterm_Variants { get; set; }
    }
}
