using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class Subject
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int? MaximumScore { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<UsersToSubjects> UsersToSubjects { get; set; }
        public virtual ICollection<Theme> Themes { get; set; }
        public virtual ICollection<QuestionsToSubjects> QuestionsToSubjects { get; set; }
        public virtual ICollection<SubjectsToGoals> SubjectsToGoals { get; set; }
        public virtual ICollection<EntrantTestQuestion> EntrantQuestions { get; set; }
    }
}
