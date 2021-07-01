using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class ApplicationUser 
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime RegistrationDate { get; set; }
        [ForeignKey("Grade")]
        public int? GradeId { get; set; }
        public int? DesiredResult { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsMale { get; set; }
        [ForeignKey("Region")]
        public int? RegionId { get; set; }
        public string RegionStr { get; set; }
        [ForeignKey("School")]
        public int? SchoolId { get; set; }
        public string SchoolStr { get; set; }
        public double? SecondsOnTheSite { get; set; }
        public string KaznituPassword { get; set; }
        [ForeignKey("Level")]
        public int? LevelId { get; set; }
        public virtual ICollection<UsersToGoals> UsersToGoals { get; set; }
        public virtual ICollection<UsersToLearningPlaces> UsersToLearningPlaces { get; set; }
        public virtual ICollection<UsersToMaterials> UsersToMaterials { get; set; }
        public virtual ICollection<UsersToSubjects> UsersToSubjects { get; set; }
        public virtual ICollection<UsersToTrainings> UsersToTrainings { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual ICollection<UsersToThemes> UsersToThemes { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual Region Region { get; set; }
        public virtual School School { get; set; }
        public virtual Level Level { get; set; }
        public virtual ICollection<UsersToModules> UsersToModules { get; set; }
        public virtual ICollection<UserRatings> UserRatings { get; set; }
        public virtual ICollection<UsersToControlWorks> UsersToControlWorks { get; set; }
        
    }
}
