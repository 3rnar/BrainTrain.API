using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class UsersToMaterials
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        [ForeignKey("MaterialLearningStatus")]
        public int MaterialLearningStatusId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Material Material { get; set; }
        public virtual MaterialLearningStatus MaterialLearningStatus { get; set; } 
    }
}
