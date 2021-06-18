using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class Module
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [ForeignKey("ModuleType")]
        public int? ModuleTypeId { get; set; }
        public int? DaysToLearn { get; set; }
        [ForeignKey("Subject")]
        public int? SubjectId { get; set; }

        public virtual ICollection<ThemesToModules> ThemesToModules { get; set; }
        public virtual ModuleType ModuleType { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
