using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class EntVariantsToQuestions
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("EntVariant")]
        public int EntVariantId { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        public virtual EntVariant EntVariant { get; set; }

        public virtual Question Question { get; set; }
    }
}
