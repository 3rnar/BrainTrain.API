using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class QuestionsToThemes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        [ForeignKey("Theme")]
        public int ThemeId { get; set; }

        public virtual Question Question { get; set; }
        public virtual Theme Theme { get; set; }
    }
}
