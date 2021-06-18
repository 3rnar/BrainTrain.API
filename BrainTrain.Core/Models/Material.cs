using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class Material
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required, ForeignKey("ContentManager")]
        public string ContentManagerId { get; set; }
        [Required, ForeignKey("MaterialType")]
        public int MaterialTypeId { get; set; }
        [ForeignKey("MaterialAuthor")]
        public int MaterialAuthorId { get; set; }


        public virtual ApplicationUser ContentManager { get; set; }
        public virtual ICollection<UsersToMaterials> UsersToMaterials { get; set; }
        public virtual MaterialType MaterialType { get; set; }
        public virtual MaterialAuthor MaterialAuthor { get; set; }
        public virtual ICollection<KeywordsToMaterials> KeywordsToMaterials { get; set; }
        public virtual ICollection<VideosToMaterials> VideosToMaterials { get; set; }
        public virtual ICollection<FilesToMaterials> FilesToMaterials { get; set; }
        public virtual ICollection<TextsToMaterials> TextsToMaterials { get; set; }
        public virtual ICollection<QuestionsToMaterials> QuestionsToMaterials { get; set; }
        public virtual ICollection<MaterialsToThemes> MaterialsToThemes { get; set; }
    }
}
