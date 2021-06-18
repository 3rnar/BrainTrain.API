using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainTrain.Core.Models
{
    public class Level
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public double FromRating { get; set; }
        [Required]
        public double ToRating { get; set; }
    }
}
