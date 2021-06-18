using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class ControlWorksToModules
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("ControlWork")]
        public int ControlWorkId { get; set; }
        [ForeignKey("Module")]
        public int ModuleId { get; set; }

        public virtual ControlWork ControlWork { get; set; }
        public virtual Module Module { get; set; }
    }
}
