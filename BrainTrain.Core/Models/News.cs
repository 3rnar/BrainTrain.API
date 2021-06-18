using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class News
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string ShortText { get; set; }
        [Required, ForeignKey("ContentManager")]
        public string ContentManagerId { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public string ImageUrl { get; set; }
        

        public virtual ApplicationUser ContentManager { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
