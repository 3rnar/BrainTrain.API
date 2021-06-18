using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class ControlWork
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required, ForeignKey("ControlWorkType")]
        public int TypeId { get; set; }
        [Required, ForeignKey("Subject")]
        public int SubjectId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ControlWorkType ControlWorkType { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<ControlWorksToModules> ControlWorksToModules { get; set; }
        public virtual ICollection<UsersToControlWorks> UsersToControlWorks { get; set; }

    }
}
