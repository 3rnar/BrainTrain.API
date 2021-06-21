using BrainTrain.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BrainTrain.Core.Models
{
    public class BrainTrainContext : DbContext
    {
        public BrainTrainContext(DbContextOptions<BrainTrainContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<AdminDashboardViewModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<MidtermEntrantViewModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<IntView>().HasNoKey().ToView(null);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<FilesToMaterials> FilesToMaterials { get; set; }
        public DbSet<VideosToMaterials> VideosToMaterials { get; set; }
        public DbSet<TextsToMaterials> TextsToMaterials { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<UsersToGoals> UsersToGoals { get; set; }
        public DbSet<Keyword> KeyWords { get; set; }
        public DbSet<KeywordsToMaterials> KeywordsToMaterials { get; set; }
        public DbSet<LearningPlace> LearningPlaces { get; set; }
        public DbSet<UsersToLearningPlaces> UsersToLearningPlaces { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialLearningStatus> MaterialLearningStatuses { get; set; }
        public DbSet<MaterialAuthor> MaterialAuthors { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<UsersToSubjects> UsersToSubjects { get; set; }
        public DbSet<MaterialType> MaterialTypes { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<MaterialsToThemes> MaterialsToThemes { get; set; }
        public DbSet<UploadFile> UploadFiles { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<Question> Questions{ get; set; }
        public DbSet<ControlWork> ControlWorks { get; set; }
        public DbSet<ControlWorksToModules> ControlWorksToModules { get; set; }
        public DbSet<UserRatings> UserRatings { get; set; }
        public DbSet<UserCoins> UserCoins { get; set; }
        public DbSet<QuestionsToThemes> QuestionsToThemes { get; set; }

        public DbSet<EntrantTestQuestionsToThemes> EntrantTestQuestionsToThemes { get; set; }
        public DbSet<EntrantTestQuestion> EntrantTestQuestions { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ThemesToModules> ThemesToModules { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<QuestionAnswerVariant> QuestionAnswerVariants { get; set; }
        public DbSet<UsersToModules> UsersToModules { get; set; }
        public DbSet<BattleQueue> BattleQueues { get; set; }
        public DbSet<BattleQueuesToThemes> BattleQueuesToThemes { get; set; }
        public DbSet<BattleQueuesToQuestions> BattleQueuesToQuestions { get; set; }
        public DbSet<AnswerSource> AnswerSources { get; set; }
        public DbSet<Constant> Constants { get; set; }
        public DbSet<ModuleThemePassAttempt> ModuleThemePassAttempts { get; set; }
        public DbSet<UsersToThemes> UsersToThemes { get; set; }
        public DbSet<ThemesTrees> ThemesTrees { get; set; }
        public DbSet<ModuleType> ModuleTypes { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<QuestionCompaint> QuestionCompaints { get; set; }
        public DbSet<ComplaintType> ComplaintTypes { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<ModuleThemePassAttemptsToQuestions> ModuleThemePassAttemptsToQuestions { get; set; }
        public DbSet<ControlWorkType> ControlWorkTypes { get; set; }
        public DbSet<EntYear> EntYears { get; set; }
        public DbSet<EntVariant> EntVariants { get; set; }
        public DbSet<EntVariantsToQuestions> EntVariantsToQuestions { get; set; }
        public DbSet<UsersToControlWorks> UsersToControlWorks { get; set; }
        public DbSet<UsersToControlWorksToQuestions> UsersToControlWorksToQuestions { get; set; }
        public DbSet<UsersToEntVariants> UsersToEntVariants { get; set; }
        public DbSet<RequestLog> RequestLogs { get; set; }
        public DbSet<LRNQuestion> LRNQuestions { get; set; }
        public DbSet<Midterm_UserEventQuestion> Midterm_UserEventQuestions { get; set; }
        public DbSet<Midterm_Question> Midterm_Questions { get; set; }
        public DbSet<Midterm_UserEvent> Midterm_UserEvents { get; set; }
        public DbSet<Midterm_Language> Midterm_Languages { get; set; }
        public DbSet<Midterm_Event> Midterm_Events { get; set; }
        public DbSet<Midterm_User> Midterm_Users { get; set; }
        public DbSet<Midterm_Variant> Midterm_Variants { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<QuestionAnswerLrnVariant> QuestionAnswerLrnVariants { get; set; }
        public DbSet<SourceUsefullness> SourceUsefullnesses { get; set; }
        public DbSet<Midterm_Teacher> Midterm_Teachers { get; set; }
        public DbSet<Midterm_UserStatus> Midterm_UserStatuses { get; set; }
        public DbSet<Midterm_Subject> Midterm_Subjects { get; set; }
        public DbSet<Midterm_QuestionStatus> Midterm_QuestionStatuses { get; set; }
        public DbSet<Midterm_UserComplaint> Midterm_UserComplaints { get; set; }
        public DbSet<KaznituUser> KaznituUsers { get; set; }

        public DbSet<SubjectsToGoals> SubjectsToGoals { get; set; }

        public DbSet<TrainingType> TrainingTypes { get; set; }

        public DbSet<TrainingStatus> TrainingStatus { get; set; }

        public DbSet<QuestionType> QuestionTypes { get; set; }

        public DbSet<QuestionDifficulty> QuestionDifficulties { get; set; }

        public DbSet<QuestionVariant> QuestionVariants { get; set; }

        public DbSet<OpenQuestionsToMaterialParts> OpenQuestionsToMaterialParts { get; set; }


        public DbSet<AdminDashboardViewModel> AdminDashboardViewModels { get; set; }
    }
}
