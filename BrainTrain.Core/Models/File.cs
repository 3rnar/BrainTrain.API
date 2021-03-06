using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class File
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string PreText { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }

        public virtual ICollection<FilesToMaterials> FilesToMaterials { get; set; }
        public virtual ICollection<OpenQuestionsToMaterialParts> OpenQuestionsToMaterialParts { get; set; }

    }
}
