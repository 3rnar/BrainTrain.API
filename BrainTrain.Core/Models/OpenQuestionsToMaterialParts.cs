using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class OpenQuestionsToMaterialParts
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        [ForeignKey("Text")]
        public int?  TextId { get; set; }
        [ForeignKey("Video")]
        public int? VideoId { get; set; }
        [ForeignKey("File")]
        public int? FileId { get; set; }

        public virtual Question Question { get; set; }
        public virtual Text Text { get; set; }
        public virtual Video Video { get; set; }
        public virtual File File { get; set; }
    }
}
