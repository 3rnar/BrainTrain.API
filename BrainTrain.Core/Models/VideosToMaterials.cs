using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class VideosToMaterials
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Video")]
        public int VideoId { get; set; }
        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public string PreText { get; set; }

        public virtual Video Video { get; set; }

        public virtual Material Material { get; set; }
    }
}
