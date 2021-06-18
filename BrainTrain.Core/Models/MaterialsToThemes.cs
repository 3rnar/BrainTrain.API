using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class MaterialsToThemes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Theme")]
        public int ThemeId { get; set; }
        [ForeignKey("Material")]
        public int MaterialId { get; set; }

        public virtual Theme Theme { get; set; }

        public virtual Material Material { get; set; }
    }
}
