using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class KeywordsToMaterials
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Keyword")]
        public int KeywordId { get; set; }
        [ForeignKey("Material")]
        public int MaterialId { get; set; }

        public virtual Keyword Keyword { get; set; }

        public virtual Material Material { get; set; }
    }
}
