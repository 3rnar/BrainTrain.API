using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BrainTrain.Core.Models
{
    public class QuestionAnswerLrnVariant
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, ForeignKey("QuestionAnswer")]
        public int QuestionAnswerId { get; set; }
        [Required]
        public int VariantValue { get; set; }

        public virtual QuestionAnswer QuestionAnswer { get; set; }
    }
}
