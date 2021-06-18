using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required, ForeignKey("User")]
        public string UserId { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [ForeignKey("ReplyingComment")]
        public int? ReplyingCommentId { get; set; }
        [ForeignKey("News")]
        public int? NewsId { get; set; }
        public string ImageUrl { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Comment ReplyingComment { get; set; }
        public virtual News News { get; set; }
    }
}
