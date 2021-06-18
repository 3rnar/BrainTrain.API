using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class BattleQueuesToThemes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("BattleQueue")]
        public int BattleQueueId { get; set; }
        [ForeignKey("Theme")]
        public int ThemeId { get; set; }

        public virtual BattleQueue BattleQueue { get; set; }
        public virtual Theme Theme { get; set; }
    }
}
