using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class BattleQueue
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, ForeignKey("FirstUser")]
        public string FirstUserId { get; set; }
        [ForeignKey("SecondUser")]
        public string SecondUserId { get; set; }
        [Required, ForeignKey("Subject")]
        public int SubjectId { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }

        [ForeignKey("BattleResultStatus")]
        public int? BattleResultStatusId { get; set; }
        [ForeignKey("Winner")]
        public string WinnerId { get; set; }


        public virtual ApplicationUser FirstUser { get; set; }
        public virtual ApplicationUser SecondUser { get; set; }
        public virtual ApplicationUser Winner { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual BattleResultStatus BattleResultStatus { get; set; }
        public virtual ICollection<BattleQueuesToQuestions> BattleQueuesToQuestions { get; set; }
        public virtual ICollection<BattleQueuesToThemes> BattleQueuesToThemes { get; set; }

    }
}
