using BrainTrain.API.Models;
using BrainTrain.Core.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [RoutePrefix("api/Customer/User")]
    public class CustomerUserController : BaseApiController
    {
        [HttpGet]
        [Route("Coins")]
        public async Task<IHttpActionResult> GetCoins()
        {
            var userId = User.Identity.GetUserId();

            var coins = await db.UserCoins.FirstOrDefaultAsync(uc => uc.UserId == UserId);
            if (coins == null){
                db.UserCoins.Add(new UserCoins { UserId = userId, Balance = 0});
                return Ok(0);
            }
            else
            {
                return Ok(coins.Balance);
            }            
        }

        [HttpGet]
        [Route("Experience")]
        public async Task<IHttpActionResult> GetXP()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var levels = await db.Levels.OrderBy(l => l.Id).ToListAsync();
            var rating = await db.UserRatings.FirstOrDefaultAsync(uc => uc.UserId == UserId);

            var currentLevel = levels.FirstOrDefault(l => l.Id == user.LevelId);
            var nextLevel = levels.FirstOrDefault(l => l.Id == (currentLevel?.Id + 1));

            var model = new CustomerLevelAndExperienceViewModel
            {
                Experience = rating?.Rating,
                LevelId = user.LevelId,
                LevelTitle = currentLevel?.Text,
                NextLevelId = nextLevel?.Id,
                NextLevelTitle = nextLevel?.Text,
                BetweenCurrentAndNextLevel = currentLevel?.ToRating - currentLevel?.FromRating,
                TillNextLevel = currentLevel?.ToRating - rating?.Rating
            };

            return Ok(model);
        }

        [HttpGet]
        [Route("IsAdditionalInfoFilled")]
        public async Task<IHttpActionResult> IsAdditionalInfoFilled(int subjectId)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
                return BadRequest();

            if (!db.UsersToSubjects.Any(uts => uts.UserId == user.Id) || user.GradeId == null || user.DesiredResult == null) {
                //не заполненая доп информация
                return Ok(0);
            }
            if (!db.UsersToModules.Any(utm => utm.UserId == user.Id && utm.Module.SubjectId == subjectId))
            {
                //не заполнен входной тест
                return Ok(2);
            }

            //все заполнено
            return Ok(1);
        }

        [HttpGet]
        [Route("Grades")]
        public IEnumerable<Grade> GetGrades()
        {
            return db.Grades.ToList();
        }

        [HttpGet]
        [Route("Subjects")]
        public IEnumerable<Subject> GetSubjects()
        {
            return db.Subjects.ToList();
        }

        [HttpGet]
        [Route("KnowledgeBorder")]
        public async Task<double> GetKnowledgeBorder(int subjectId)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            var uts = await db.UsersToSubjects.FirstOrDefaultAsync(u => u.UserId == user.Id && u.SubjectId == subjectId);
            if (uts == null)
                BadRequest();

            var subj = await db.Subjects.FirstOrDefaultAsync(s => s.Id == subjectId);

            return ((double) (uts.DesiredScore ?? 0) / (double)(subj.MaximumScore ?? 25)) * 100;
        }

        [HttpGet]
        [Route("UserDesiredScore")]
        public async Task<double> GetUserDesiredScore()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            return user.DesiredResult ?? 0;
        }

        [HttpPost]
        [Route("ChangeDesiredScore")]
        public async Task<IHttpActionResult> ChangeDesiredScore(int desiredScore)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            user.DesiredResult = desiredScore;
            var entSubjectNumber = await db.Constants.FirstOrDefaultAsync(c => c.Id == 5);

            var usersToSubjects = await db.UsersToSubjects.ToListAsync();
            var desiredScoreBySubj = desiredScore / entSubjectNumber.NumericValue;

            foreach (var uts in usersToSubjects)
            {

                uts.DesiredScore = Convert.ToInt32(desiredScoreBySubj);
            }

            await UserManager.UpdateAsync(user);
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("AddAdditionalInfo")]
        public async Task<IHttpActionResult> AddAdditionalInfo(AdditionalInfoViewModel model)
        {            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest();
            
            if (model.GradeId != 0)
            {
                user.GradeId = model.GradeId;
            }

            if (!db.UsersToGoals.Any(utg => utg.GoalId == 1 && utg.UserId == user.Id))
            {
                db.UsersToGoals.Add(new UsersToGoals {UserId = user.Id, GoalId = 1, DateCreated = DateTime.Now });
            }

            user.DesiredResult = model.DesiredScore;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            if (user.LevelId == null)
            {
                user.LevelId = 1;
            }

            var entSubjectNumber = await db.Constants.FirstOrDefaultAsync(c => c.Id == 5);

            int desiredScore = (int)(user.DesiredResult == null ? 25 : (user.DesiredResult / entSubjectNumber.NumericValue));

            foreach (var sid in model.SubjectIds)
            {
                if (!db.UsersToSubjects.Any(uts => uts.SubjectId == sid.SubjectId && uts.UserId == user.Id))
                    db.UsersToSubjects.Add(new UsersToSubjects {
                        UserId = user.Id, SubjectId = sid.SubjectId, DesiredScore = desiredScore, DateCreated = DateTime.Now
                    });
            }

            await UserManager.UpdateAsync(user);
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("UserPersonalInfo")]
        public async Task<CustomerPersonalInfoViewModel> GetUserPersonalInfo()
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            var rating = await db.UserRatings.FirstOrDefaultAsync(ur => ur.UserId == userId);

            var level = rating != null ? await db.Levels.FirstOrDefaultAsync(l => rating.Rating >= l.FromRating && rating.Rating < l.ToRating) : null;

            var model = new CustomerPersonalInfoViewModel {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsMale = user.IsMale,
                PhoneNumber = user.PhoneNumber,
                RegionId = user.RegionId,
                SchoolId = user.SchoolId,
                Email = user.Email,
                GradeId = user.GradeId,
                BirthDate = user.BirthDate,
                AvatarUrl = user.AvatarUrl,
                Experience = rating?.Rating,
                RegionStr = user.RegionStr,
                SchoolStr = user.SchoolStr,
                Level = level
            };

            if (user.SchoolId != null)
            {
                var school = await db.Schools.FirstOrDefaultAsync(s => s.Id == user.SchoolId);
                model.School = new School { Id = school.Id, Title = school.Title };
            }
            if (user.RegionId != null)
            {
                var region = await db.Regions.FirstOrDefaultAsync(r => r.Id == user.RegionId);
                model.Region = new Region { Id = region.Id, Title = region.Title };
            }

            return model;
        }

        [HttpPost]
        [Route("UpdatePersonalInfo")]
        public async Task<IHttpActionResult> UpdatePersonalInfo(CustomerPersonalInfoViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);

            if (!string.IsNullOrEmpty( model.FirstName))
            {
                user.FirstName = model.FirstName;
            }
            if (!string.IsNullOrEmpty(model.UserName))
            {
                if (db.Users.Any(u => u.UserName.Equals(model.UserName) && u.Id != userId))
                {
                    return BadRequest("Ник уже используется другим пользователем");
                }
                user.UserName = model.UserName;
            }
            if (!string.IsNullOrEmpty(model.LastName))
            {
                user.LastName = model.LastName;
            }
            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                user.PhoneNumber = model.PhoneNumber;
            }
            if (model.SchoolId != null)
            {
                user.SchoolId = model.SchoolId.Value;
            }
            if (model.RegionId != null)
            {
                user.RegionId = model.RegionId.Value;
            }
            if (model.IsMale != null)
            {
                user.IsMale = model.IsMale.Value;
            }
            if (model.GradeId != null)
            {
                user.GradeId = model.GradeId.Value;
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                if (db.Users.Any(u => u.Email.Equals(model.Email) && u.Id != userId))
                {
                    return BadRequest("Email уже используется другим пользователем");
                }
                user.Email = model.Email;
            }
            if (model.BirthDate != null)
            {
                user.BirthDate = model.BirthDate.Value;
            }
            await UserManager.UpdateAsync(user);
            return Ok();
        }

        [HttpGet]
        [Route("RegionsList")]
        public async Task<IEnumerable<Region>> GetRegionsList()
        {
            return await db.Regions.ToListAsync();
        }

        [HttpGet]
        [Route("SchoolsList")]
        public async Task<IEnumerable<School>> GetSchoolsList(int regionId)
        {
            return await db.Schools.Where(s => s.RegionId == regionId).ToListAsync();
        }

        [HttpPost]
        [Route("AddUserSubject")]
        public async Task<IHttpActionResult> AddUserSubject([FromUri]List<int> subjectIds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);

            var usersToSubject = await db.UsersToSubjects.Where(uts => uts.UserId == userId).ToListAsync();
            var entDate = await db.Constants.FirstOrDefaultAsync(c => c.Id == 3);
            var daysBorder = await db.Constants.FirstOrDefaultAsync(c => c.Id == 2);
            var maxEntScore = await db.Constants.FirstOrDefaultAsync(c => c.Id == 4);
            var entSubjectNumber = await db.Constants.FirstOrDefaultAsync(c => c.Id == 5);
            
            int desiredScore = (int) ( user.DesiredResult == null ? 20 : (user.DesiredResult / entSubjectNumber.NumericValue) );

            foreach (var s in subjectIds)
            {
                var sDb = usersToSubject.FirstOrDefault(sts => sts.SubjectId == s);
                if (sDb == null)
                {
                    db.UsersToSubjects.Add(new UsersToSubjects { SubjectId = s, UserId = userId, DesiredScore = desiredScore, DateCreated = DateTime.Now });

                    var UserId = new SqlParameter("@UserId", userId);
                    var SubjectId = new SqlParameter("@SubjectId", s);
                    //укороченные или обычные модули
                    if ((entDate.DateValue.Value - DateTime.Now).Days <= daysBorder.NumericValue)
                    {
                        db.Database.ExecuteSqlCommand("dbo.InsertUsersToQuickModules @SubjectId, @UserId", SubjectId, UserId);

                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("dbo.InsertUsersToModules @SubjectId, @UserId", SubjectId, UserId);
                    }
                }
                else
                {
                    sDb.DesiredScore = desiredScore;
                }
            }

            foreach (var uts in usersToSubject)
            {
                if (!subjectIds.Any(s => s == uts.SubjectId))
                {
                    db.Entry<UsersToSubjects>(uts).State = EntityState.Deleted;
                }
            }
                        
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("AllUsers")]
        public async Task<List<UserViewModel>> GetAllUsers()
        {
            var regions = await db.Regions.ToListAsync();
            var schools = await db.Schools.ToListAsync();
            var dbUsers = await db.Users.Include(u => u.UserRatings).Where(u => u.Roles.Any(r => r.RoleId == "4" || r.RoleId == "2" || r.RoleId == "1")  == false).OrderByDescending(u => u.UserRatings.Sum(ur => ur.Rating)).ToListAsync();
            var users = new List<UserViewModel>();
            var levels = await db.Levels.ToListAsync();

            foreach (var u in dbUsers)
            {
                var model = new UserViewModel
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    AvatarUrl = u.AvatarUrl,
                    Experience = u.UserRatings.Sum(ur => ur.Rating),
                    SecondsOnTheSite = u.SecondsOnTheSite,
                    Status = StatsHandler.ConnectedIds.Any(c => c.UserName == u.UserName) ? "Онлайн" : "Оффлайн",
                    Level = levels.FirstOrDefault(l => u.UserRatings.Sum(ur => ur.Rating) >= l.FromRating && u.UserRatings.Sum(ur => ur.Rating) < l.ToRating)
                };
                if (u.RegionId != null)
                {
                    var reg = regions.FirstOrDefault(r => r.Id == u.RegionId.Value);
                    if (reg != null)
                        model.Region = reg.Title;
                }
                if (u.SchoolId != null)
                {
                    var sch = schools.FirstOrDefault(r => r.Id == u.SchoolId.Value);
                    if (sch != null)
                        model.School = sch.Title;
                }
                users.Add(model);
            }

            return users;
        }

        [HttpGet]
        [Route("SolvedQuestionsToAllQuestions")]
        public async Task<SolvedQuestionsToAllQuestionsViewModel> GetSolvedQuestionsToAllQuestions()
        {
            var sq = new SolvedQuestionsToAllQuestionsViewModel();
            var userId = User.Identity.GetUserId();

            sq.NumberOfQuestions = await db.QuestionAnswers.Where(qa => qa.UserId == userId).CountAsync();
            sq.NumberOfSolvedQuestions = await db.QuestionAnswers.Where(qa => qa.UserId == UserId && (qa.Value.Trim().ToLower() == qa.Question.CorrectAnswerValue.Trim().ToLower() || qa.QuestionAnswerVariants.Any(qav => qav.QuestionVariant.IsCorrect == true))).CountAsync();
            sq.Relation = (double)sq.NumberOfSolvedQuestions / (double)sq.NumberOfQuestions * 100;

            return sq;
        }

        [HttpGet]
        [Route("LearnedThemesToAllThemes")]
        public async Task<LearnedThemesToAllThemesViewModel> GetLearnedThemesToAllThemes(int subjectId)
        {
            var sq = new LearnedThemesToAllThemesViewModel();
            var userId = User.Identity.GetUserId();

            sq.NumberOfThemes = await db.Themes.Where(qa => qa.SubjectId == subjectId).CountAsync();
            sq.NumberOfLearnedThemes = await db.UsersToThemes.Where(utt => utt.IsThemeLearned == true && utt.UserId == userId && utt.Theme.SubjectId == subjectId).CountAsync();
            sq.Relation = (double)sq.NumberOfLearnedThemes / (double)sq.NumberOfThemes * 100;

            return sq;
        }

        [HttpGet]
        [Route("LearnedModulesToAllModules")]
        public async Task<LearnedModulesToAllModulesViewModel> GetLearnedModulesToAllModules()
        {
            var sq = new LearnedModulesToAllModulesViewModel();
            var userId = User.Identity.GetUserId();

            sq.NumberOfModules = await db.UsersToModules.Where(qa => qa.UserId == userId).CountAsync();
            sq.NumberOfLearnedModules = await db.UsersToModules.Where(utt => utt.FactLearnedDate != null).CountAsync();
            sq.Relation = (double)sq.NumberOfLearnedModules / (double)sq.NumberOfModules * 100;

            return sq;
        }

        [HttpGet]
        [Route("GetAvatar")]
        public async Task<string> GetAvatar()
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);

            return user.AvatarUrl ?? "";
        }

        [HttpPost]
        [Route("UploadAvatar")]
        public async Task<IHttpActionResult> UploadAvatar()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var uploadFiles = new List<UploadFile>();
            var httpRequest = HttpContext.Current.Request;

            foreach (string file in httpRequest.Files)
            {
                var postedFile = httpRequest.Files[file];
                var fileNameInFileSystem = Guid.NewGuid().ToString() + Path.GetExtension(postedFile.FileName);

                var filePath = HttpContext.Current.Server.MapPath("~/App_Data/uploads/" + fileNameInFileSystem);
                postedFile.SaveAs(filePath);

                var uploadFile = new UploadFile
                {
                    BlobUrl = "~/App_Data/uploads/" + fileNameInFileSystem,
                    DateCreated = DateTime.Now,
                    FileName = postedFile.FileName
                };
                uploadFiles.Add(uploadFile);
            }

            db.UploadFiles.AddRange(uploadFiles);
            await db.SaveChangesAsync();

            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);

            if (uploadFiles.Count > 0)
                user.AvatarUrl = "/api/UploadFiles/" + uploadFiles[0].Id;
           await UserManager.UpdateAsync(user);

           return Ok(user.AvatarUrl);
        }

        [HttpGet]
        [Route("ConnectedUsers")]
        public int GetConnectedUsers()
        {
            return StatsHandler.ConnectedIds.Count;
        }

        [HttpGet]
        [Route("Grade")]
        public string GetUserGrade()
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);

            if (user.GradeId == null)
                return "Не заполнен";

            var grade = db.Grades.FirstOrDefault(g => g.Id == user.GradeId);

            return grade.Title;
        }

        [HttpGet]
        [Route("ShortInfo")]
        public async Task<CustomerShortInfoViewModel> ShortInfo(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            var shortInfo = new CustomerShortInfoViewModel
            {
                UserId = user.Id,
                AvatarUrl = user.AvatarUrl,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RegionId = user.RegionId,
                SchoolId = user.SchoolId,
                Experience = 0,
                RegionStr = user.RegionStr,
                SchoolStr = user.SchoolStr,
                Email = user.Email,
                BirthDate = user.BirthDate
            };

            if (shortInfo.RegionId != null)
            {
                var region = await db.Regions.FirstOrDefaultAsync(r => r.Id == shortInfo.RegionId.Value);
                if (region != null)
                    shortInfo.Region = region.Title;
            }

            if (shortInfo.SchoolId != null)
            {
                var school = await db.Schools.FirstOrDefaultAsync(r => r.Id == shortInfo.SchoolId.Value);
                if (school != null)
                    shortInfo.School = school.Title;
            }
            var ratings = await db.UserRatings.FirstOrDefaultAsync(ur => ur.UserId == userId);
            if (ratings != null)
            {
                shortInfo.Experience = ratings.Rating;

                var level = await db.Levels.FirstOrDefaultAsync(l => l.Id == user.LevelId);
                shortInfo.Level = level;
            }



            return shortInfo;
        }

        [HttpGet]
        [Route("ShortStats")]
        public async Task<CustomerShortStatsViewModel> ShortStats(int subjectId)
        {
            var userId = User.Identity.GetUserId();

            var stats = new CustomerShortStatsViewModel();


            var modulesCount = db.Modules.Where(m => m.ThemesToModules.Any(ttm => ttm.Theme.SubjectId == subjectId)).Count();
            var modulesCompleted = db.UsersToModules.Where(utm => utm.Module.ThemesToModules.Any(ttm => ttm.Theme.SubjectId == subjectId) && utm.FactLearnedDate != null && utm.UserId == userId).Count();

            var themesCount = db.Themes.Where(t => t.SubjectId == subjectId && t.ThemesToModules.Count > 0 
                && t.UsersToThemes.Any(utt => utt.UserId == userId)).Select(t => t.Id).Distinct().Count();
            var themesCompleted = db.UsersToThemes.Where(utt => utt.UserId == userId 
                && utt.IsThemeLearned == true && utt.Theme.SubjectId == subjectId && utt.Theme.ThemesToModules.Count > 0).Select(utt => utt.ThemeId).Distinct().Count();

            var questionsAnswered = db.QuestionAnswers.Where(qa => 
                qa.Question.QuestionsToThemes.Any(qtt => qtt.Theme.SubjectId == subjectId) 
                && qa.UserId == userId).Select(qa=>qa.QuestionId).Distinct().Count();
            var correctlyAnswered = db.QuestionAnswers.
                Where(qa => qa.Question.QuestionsToThemes.Any(qtt => qtt.Theme.SubjectId == subjectId) 
                && qa.UserId == userId && qa.IsCorrect == true).Select(qa => qa.QuestionId).Distinct().Count();

            var subjectLearningRate = db.UsersToSubjects.FirstOrDefault(uts => uts.SubjectId == subjectId);

            stats.NumberOfModules = modulesCount;
            stats.ModuleNumberAt = modulesCompleted;
            stats.ModulesCompletedPercent = (double)modulesCompleted / (double)modulesCount * 100;

            stats.NumberOfThemes = themesCount;
            stats.ThemesNumberAt = themesCompleted;
            stats.ThemesCompletedPercent = (double)themesCompleted / (double)themesCount * 100;

            stats.TaskNumberAt = correctlyAnswered;
            stats.NumberOfTasks = questionsAnswered;
            stats.TasksCompletedPercent = (double)correctlyAnswered / (double) questionsAnswered * 100;

            stats.OverallReadinessPercent = subjectLearningRate.CurrentLearningRate ?? 0;

            return stats;
        }

        [HttpGet]
        [Route("ShortStatsByUserId")]
        public async Task<CustomerShortStatsViewModel> ShortStatsByUserId(int subjectId, string userId)
        {
            var stats = new CustomerShortStatsViewModel();


            var modulesCount = db.Modules.Where(m => m.ThemesToModules.Any(ttm => ttm.Theme.SubjectId == subjectId)).Count();
            var modulesCompleted = db.UsersToModules.Where(utm => utm.Module.ThemesToModules.Any(ttm => ttm.Theme.SubjectId == subjectId) && utm.FactLearnedDate != null && utm.UserId == userId).Count();

            var themesCount = db.Themes.Where(t => t.SubjectId == subjectId && t.ThemesToModules.Count > 0
                && t.UsersToThemes.Any(utt => utt.UserId == userId)).Select(t => t.Id).Distinct().Count();
            var themesCompleted = db.UsersToThemes.Where(utt => utt.UserId == userId
                && utt.IsThemeLearned == true && utt.Theme.SubjectId == subjectId && utt.Theme.ThemesToModules.Count > 0).Select(utt => utt.ThemeId).Distinct().Count();

            var questionsAnswered = db.QuestionAnswers.Where(qa =>
                qa.Question.QuestionsToThemes.Any(qtt => qtt.Theme.SubjectId == subjectId)
                && qa.UserId == userId
                && (!string.IsNullOrEmpty(qa.Value) || qa.QuestionAnswerVariants.Any())
            ).Select(qa => qa.QuestionId).Distinct().Count();
            var correctlyAnswered = db.QuestionAnswers.
                Where(qa => qa.Question.QuestionsToThemes.Any(qtt => qtt.Theme.SubjectId == subjectId)
                && qa.UserId == userId
                && (qa.QuestionAnswerVariants.Any(qav => qav.QuestionVariant.IsCorrect == true) || qa.Value.ToLower().Trim() == qa.Question.CorrectAnswerValue.ToLower().Trim())
                ).Select(qa => qa.QuestionId).Distinct().Count();

            var subjectLearningRate = db.UsersToSubjects.FirstOrDefault(uts => uts.SubjectId == subjectId);

            stats.NumberOfModules = modulesCount;
            stats.ModuleNumberAt = modulesCompleted;
            stats.ModulesCompletedPercent = (double)modulesCompleted / (double)modulesCount * 100;

            stats.NumberOfThemes = themesCount;
            stats.ThemesNumberAt = themesCompleted;
            stats.ThemesCompletedPercent = (double)themesCompleted / (double)themesCount * 100;

            stats.TaskNumberAt = correctlyAnswered;
            stats.NumberOfTasks = questionsAnswered;
            stats.TasksCompletedPercent = (double)correctlyAnswered / (double)questionsAnswered * 100;

            stats.OverallReadinessPercent = subjectLearningRate.CurrentLearningRate ?? 0;

            return stats;
        }
    }
}
