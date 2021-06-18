using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class TextsToMaterials
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Text")]
        public int TextId { get; set; }
        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public string PreText { get; set; }

        public virtual Text Text { get; set; }

        public virtual Material Material { get; set; }
    }
}
