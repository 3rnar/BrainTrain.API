using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class Question
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, ForeignKey("ContentManager")]
        public string ContentManagerId { get; set; }
        [Required]
        public string Text { get; set; }
        [Required, ForeignKey("QuestionType")]
        public int QuestionTypeId { get; set; }
        [Required, ForeignKey("QuestionDifficulty")]
        public int QuestionDifficultyId { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public string CorrectAnswerValue { get; set; }
        public bool? IsEnt { get; set; }
        public bool? IsChecked { get; set; }
        public bool? IsModuleQuestion { get; set; }
        public string JsonData { get; set; }

        public virtual ApplicationUser ContentManager { get; set; }
        public virtual QuestionDifficulty QuestionDifficulty { get; set; }
        public virtual QuestionType QuestionType { get; set; }
        public virtual ICollection<QuestionsToTrainings> QuestionsToTainings { get; set; }
        public virtual ICollection<QuestionVariant> QuestionVariants { get; set; }
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual ICollection<QuestionsToGoals> QuestionsToGoals { get; set; }
        public virtual ICollection<QuestionsToSubjects> QuestionsToSubjects { get; set; }
        public virtual ICollection<QuestionsToThemes> QuestionsToThemes { get; set; }
        public virtual ICollection<QuestionsToMaterials> QuestionsToMaterials { get; set; }
        public virtual ICollection<Solution> Solutions { get; set; }
        public virtual ICollection<BattleQueuesToQuestions> BattleQueuesToQuestions { get; set; }
        public virtual ICollection<EntVariantsToQuestions> EntVariantsToQuestions { get; set; }
    }
}
