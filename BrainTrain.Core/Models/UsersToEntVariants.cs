using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class UsersToEntVariants
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("EntVariant")]
        public int EntVariantId { get; set; }
        [Required]
        public double CurrentLearningRate { get; set; }
        [Required]
        public int NumberOfCorrectAnswers { get; set; }
        [Required]
        public bool IsCompleted { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual EntVariant EntVariant { get; set; }
    }
}
