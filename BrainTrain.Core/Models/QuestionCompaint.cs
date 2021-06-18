using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class QuestionCompaint
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, ForeignKey("User")]
        public string UserId { get; set; }

        [Required, ForeignKey("ComplaintType")]
        public int ComplaintTypeId { get; set; }

        [Required, ForeignKey("Question")]
        public int QuestionId { get; set; }

        public string Comment { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ComplaintType ComplaintType { get; set; }
        public virtual Question Question { get; set; }
    }
}
