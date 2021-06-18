using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class CommentViewModel
    {
        public string Text { get; set; }
        public int NewsId { get; set; }
        public int? ReplyingCommentId { get; set; }
    }
}