using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class QuestionAnswerVariant
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("QuestionAnswer")]
        public int QuestionAnswerId { get; set; }
        [ForeignKey("QuestionVariant")]
        public int QuestionVariantId { get; set; }

        public virtual QuestionAnswer QuestionAnswer { get; set; }
        public virtual QuestionVariant QuestionVariant { get; set; }
    }
}
