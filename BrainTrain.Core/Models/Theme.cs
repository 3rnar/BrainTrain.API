using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class Theme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required, ForeignKey("Subject")]
        public int SubjectId { get; set; }

        [ForeignKey("ParentTheme")]
        public int? ParentThemeId { get; set; }

        [ForeignKey("Grade")]
        public int? GradeId { get; set; }
        public string ImageUrl { get; set; }
        public int? Order { get; set; }
        public int? DaysToLearn { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Theme ParentTheme { get; set; }
        public virtual Grade Grade { get; set; }

        public virtual ICollection<QuestionsToThemes> QuestionsToThemes { get; set; }
        public virtual ICollection<MaterialsToThemes> MaterialsToThemes { get; set; }
        public virtual ICollection<UsersToThemes> UsersToThemes { get; set; }

        public virtual ICollection<Theme> ChildThemes { get; set; }
        public virtual ICollection<ThemesToModules> ThemesToModules { get; set; }
        //public virtual ICollection<ThemesTrees> ThemesTrees { get; set; }
    }
}
