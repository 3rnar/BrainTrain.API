using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BrainTrain.Core.Models
{
    public class BattleQueuesToQuestions
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("BattleQueue")]
        public int BattleQueueId { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        public virtual BattleQueue BattleQueue { get; set; }
        public virtual Question Question { get; set; }
    }
}
