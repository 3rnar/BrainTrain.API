using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class Event
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public string Url { get; set; }
        [Required, ForeignKey("EventType")]
        public int TypeId { get; set; }
        [Required, ForeignKey("User")]
        public string UserId { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public DateTime? DateSent { get; set; }
        public bool? IsSent { get; set; }
        public bool? IsSeen { get; set; }
        public int? EntityId { get; set; }

        public virtual EventType EventType { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
