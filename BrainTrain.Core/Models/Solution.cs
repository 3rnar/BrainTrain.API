using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class Solution
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, ForeignKey("ContentManager")]
        public string ContentManagerId { get; set; }
        [Required]
        public string Text { get; set; }
        [Required, ForeignKey("Question")]
        public int QuestionId { get; set; }

        public virtual ApplicationUser ContentManager { get; set; }
        public virtual Question Question { get; set; }
    }
}
