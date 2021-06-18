using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class EntVariant
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required, ForeignKey("EntYear")]
        public int EntYearId { get; set; }
        [Required, ForeignKey("Subject")]
        public int SubjectId { get; set; }

        public virtual EntYear EntYear { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<EntVariantsToQuestions> EntVariantsToQuestions { get; set; }
    }
}
