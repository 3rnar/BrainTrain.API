using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class EntrantTestQuestion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, ForeignKey("Subject")]
        public int SubjectId { get; set; }

        [Required, ForeignKey("Question")]
        public int QuestionId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Question Question { get; set; }

        public virtual ICollection<EntrantTestQuestionsToThemes> EntrantTestQuestionsToThemes { get; set; }
    }
}
