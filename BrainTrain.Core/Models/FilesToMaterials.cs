using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class FilesToMaterials
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("File")]
        public int FileId { get; set; }
        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public string PreText { get; set; }

        public virtual File File { get; set; }

        public virtual Material Material { get; set; }
    }
}
