using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class UsersToLearningPlaces
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("LearningPlace")]
        public int LearningPlaceId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual LearningPlace LearningPlace { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
    }
}
