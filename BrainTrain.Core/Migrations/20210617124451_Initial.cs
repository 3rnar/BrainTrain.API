using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainTrain.Core.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnswerSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BattleResultStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleResultStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Constants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumericValue = table.Column<double>(type: "float", nullable: true),
                    TextValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateValue = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Constants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ControlWorkTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlWorkTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntYears",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KaznituUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    MyProperty = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IIN = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KaznituUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeyWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyWords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LearningPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningPlaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromRating = table.Column<double>(type: "float", nullable: false),
                    ToRating = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LRNQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JsonData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrecalculus = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LRNQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialAuthors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialAuthors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialLearningStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialLearningStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Midterm_Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateHold = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Midterm_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Midterm_Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Midterm_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Midterm_QuestionStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Midterm_QuestionStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Midterm_Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Midterm_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Midterm_Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Midterm_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Midterm_UserComplaints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Midterm_UserComplaints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Midterm_UserStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Midterm_UserStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModuleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionDifficulties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionDifficulties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestHeaders = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaximumScore = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Texts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Texts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploadFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlobUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Midterm_Variants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Midterm_Variants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Midterm_Variants_Midterm_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Midterm_Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Midterm_Variants_Midterm_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Midterm_Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Midterm_Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Midterm_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Midterm_Users_Midterm_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Midterm_Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Midterm_Users_Midterm_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Midterm_Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schools_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntYearId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntVariants_EntYears_EntYearId",
                        column: x => x.EntYearId,
                        principalTable: "EntYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntVariants_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModuleTypeId = table.Column<int>(type: "int", nullable: true),
                    DaysToLearn = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_ModuleTypes_ModuleTypeId",
                        column: x => x.ModuleTypeId,
                        principalTable: "ModuleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Modules_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectsToGoals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    GoalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectsToGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectsToGoals_Goals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectsToGoals_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    ParentThemeId = table.Column<int>(type: "int", nullable: true),
                    GradeId = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    DaysToLearn = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Themes_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Themes_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Themes_Themes_ParentThemeId",
                        column: x => x.ParentThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Training",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainingTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Training", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Training_TrainingTypes_TrainingTypeId",
                        column: x => x.TrainingTypeId,
                        principalTable: "TrainingTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Midterm_Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LrnJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true),
                    VariantId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Midterm_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Midterm_Questions_Midterm_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Midterm_Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Midterm_Questions_Midterm_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Midterm_Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Midterm_Questions_Midterm_Variants_VariantId",
                        column: x => x.VariantId,
                        principalTable: "Midterm_Variants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Midterm_UserEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    FinalResult = table.Column<double>(type: "float", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateFinish = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SecondsSpent = table.Column<double>(type: "float", nullable: true),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Midterm_UserEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Midterm_UserEvents_Midterm_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Midterm_Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Midterm_UserEvents_Midterm_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Midterm_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Midterm_UserEvents_Midterm_UserStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Midterm_UserStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastVisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GradeId = table.Column<int>(type: "int", nullable: true),
                    DesiredResult = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMale = table.Column<bool>(type: "bit", nullable: true),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    RegionStr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolId = table.Column<int>(type: "int", nullable: true),
                    SchoolStr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondsOnTheSite = table.Column<double>(type: "float", nullable: true),
                    KaznituPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LevelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUser_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUser_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUser_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUser_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThemesToModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThemeId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    IsDominant = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemesToModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThemesToModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThemesToModules_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThemesTrees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstThemeId = table.Column<int>(type: "int", nullable: false),
                    SecondThemeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemesTrees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThemesTrees_Themes_FirstThemeId",
                        column: x => x.FirstThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThemesTrees_Themes_SecondThemeId",
                        column: x => x.SecondThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Midterm_UserEventQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEventId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: true),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Midterm_UserEventQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Midterm_UserEventQuestions_Midterm_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Midterm_Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Midterm_UserEventQuestions_Midterm_QuestionStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Midterm_QuestionStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Midterm_UserEventQuestions_Midterm_UserEvents_UserEventId",
                        column: x => x.UserEventId,
                        principalTable: "Midterm_UserEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BattleQueues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SecondUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BattleResultStatusId = table.Column<int>(type: "int", nullable: true),
                    WinnerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattleQueues_ApplicationUser_FirstUserId",
                        column: x => x.FirstUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BattleQueues_ApplicationUser_SecondUserId",
                        column: x => x.SecondUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BattleQueues_ApplicationUser_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BattleQueues_BattleResultStatus_BattleResultStatusId",
                        column: x => x.BattleResultStatusId,
                        principalTable: "BattleResultStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BattleQueues_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ControlWorks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControlWorks_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ControlWorks_ControlWorkTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ControlWorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ControlWorks_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSent = table.Column<bool>(type: "bit", nullable: true),
                    IsSeen = table.Column<bool>(type: "bit", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_EventTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "EventTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentManagerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaterialTypeId = table.Column<int>(type: "int", nullable: false),
                    MaterialAuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_ApplicationUser_ContentManagerId",
                        column: x => x.ContentManagerId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_MaterialAuthors_MaterialAuthorId",
                        column: x => x.MaterialAuthorId,
                        principalTable: "MaterialAuthors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_MaterialTypes_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalTable: "MaterialTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModuleThemePassAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ThemeId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: true),
                    CurrentScore = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleThemePassAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleThemePassAttempts_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleThemePassAttempts_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentManagerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_ApplicationUser_ContentManagerId",
                        column: x => x.ContentManagerId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentManagerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionTypeId = table.Column<int>(type: "int", nullable: false),
                    QuestionDifficultyId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CorrectAnswerValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnt = table.Column<bool>(type: "bit", nullable: true),
                    IsChecked = table.Column<bool>(type: "bit", nullable: true),
                    IsModuleQuestion = table.Column<bool>(type: "bit", nullable: true),
                    JsonData = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_ApplicationUser_ContentManagerId",
                        column: x => x.ContentManagerId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionDifficulties_QuestionDifficultyId",
                        column: x => x.QuestionDifficultyId,
                        principalTable: "QuestionDifficulties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionTypes_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCoins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCoins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCoins_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRatings_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersToEntVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntVariantId = table.Column<int>(type: "int", nullable: false),
                    CurrentLearningRate = table.Column<double>(type: "float", nullable: false),
                    NumberOfCorrectAnswers = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToEntVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersToEntVariants_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersToEntVariants_EntVariants_EntVariantId",
                        column: x => x.EntVariantId,
                        principalTable: "EntVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersToGoals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GoalId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersToGoals_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersToGoals_Goals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersToLearningPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LearningPlaceId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToLearningPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersToLearningPlaces_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersToLearningPlaces_LearningPlaces_LearningPlaceId",
                        column: x => x.LearningPlaceId,
                        principalTable: "LearningPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersToModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    PredictedDeadLine = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentLearningRate = table.Column<double>(type: "float", nullable: false),
                    FactLearnedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersToModules_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersToModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersToSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    IsEntrantTestPassed = table.Column<bool>(type: "bit", nullable: false),
                    DesiredScore = table.Column<int>(type: "int", nullable: true),
                    CurrentLearningRate = table.Column<double>(type: "float", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersToSubjects_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersToSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersToThemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ThemeId = table.Column<int>(type: "int", nullable: false),
                    CurrentLearningRate = table.Column<double>(type: "float", nullable: false),
                    PredictedDeadLine = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PredictedSubjectLearningRate = table.Column<double>(type: "float", nullable: true),
                    IsThemeLearned = table.Column<bool>(type: "bit", nullable: true),
                    ThemeLearnedFactSubjectLearningRate = table.Column<double>(type: "float", nullable: true),
                    ThemeLearnedFactSubjectLearningDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsHidden = table.Column<bool>(type: "bit", nullable: true),
                    CurrentOvarallLearningRate = table.Column<double>(type: "float", nullable: true),
                    ModuleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToThemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersToThemes_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersToThemes_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersToThemes_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersToTrainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    TrainingStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToTrainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersToTrainings_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersToTrainings_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersToTrainings_TrainingStatus_TrainingStatusId",
                        column: x => x.TrainingStatusId,
                        principalTable: "TrainingStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BattleQueuesToThemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BattleQueueId = table.Column<int>(type: "int", nullable: false),
                    ThemeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleQueuesToThemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattleQueuesToThemes_BattleQueues_BattleQueueId",
                        column: x => x.BattleQueueId,
                        principalTable: "BattleQueues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BattleQueuesToThemes_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ControlWorksToModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControlWorkId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlWorksToModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControlWorksToModules_ControlWorks_ControlWorkId",
                        column: x => x.ControlWorkId,
                        principalTable: "ControlWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ControlWorksToModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersToControlWorks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ControlWorkId = table.Column<int>(type: "int", nullable: false),
                    CurrentLearningRate = table.Column<double>(type: "float", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToControlWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersToControlWorks_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersToControlWorks_ControlWorks_ControlWorkId",
                        column: x => x.ControlWorkId,
                        principalTable: "ControlWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FilesToMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    PreText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesToMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilesToMaterials_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FilesToMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KeywordsToMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KeywordId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordsToMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeywordsToMaterials_KeyWords_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "KeyWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeywordsToMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialsToThemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThemeId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialsToThemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialsToThemes_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialsToThemes_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TextsToMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    PreText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextsToMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextsToMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TextsToMaterials_Texts_TextId",
                        column: x => x.TextId,
                        principalTable: "Texts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersToMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    MaterialLearningStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersToMaterials_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersToMaterials_MaterialLearningStatuses_MaterialLearningStatusId",
                        column: x => x.MaterialLearningStatusId,
                        principalTable: "MaterialLearningStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersToMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VideosToMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    PreText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideosToMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideosToMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideosToMaterials_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReplyingCommentId = table.Column<int>(type: "int", nullable: true),
                    NewsId = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ReplyingCommentId",
                        column: x => x.ReplyingCommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BattleQueuesToQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BattleQueueId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleQueuesToQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattleQueuesToQuestions_BattleQueues_BattleQueueId",
                        column: x => x.BattleQueueId,
                        principalTable: "BattleQueues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BattleQueuesToQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntrantTestQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrantTestQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntrantTestQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntrantTestQuestions_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntVariantsToQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntVariantId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntVariantsToQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntVariantsToQuestions_EntVariants_EntVariantId",
                        column: x => x.EntVariantId,
                        principalTable: "EntVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntVariantsToQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModuleThemePassAttemptsToQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AttepmtId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleThemePassAttemptsToQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleThemePassAttemptsToQuestions_ModuleThemePassAttempts_AttepmtId",
                        column: x => x.AttepmtId,
                        principalTable: "ModuleThemePassAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleThemePassAttemptsToQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpenQuestionsToMaterialParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    TextId = table.Column<int>(type: "int", nullable: true),
                    VideoId = table.Column<int>(type: "int", nullable: true),
                    FileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenQuestionsToMaterialParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenQuestionsToMaterialParts_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OpenQuestionsToMaterialParts_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OpenQuestionsToMaterialParts_Texts_TextId",
                        column: x => x.TextId,
                        principalTable: "Texts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OpenQuestionsToMaterialParts_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerSourceId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolvingSeconds = table.Column<int>(type: "int", nullable: true),
                    NumberOfAttempts = table.Column<int>(type: "int", nullable: true),
                    ModuleThemePassAttemptId = table.Column<int>(type: "int", nullable: true),
                    IsViewed = table.Column<bool>(type: "bit", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswers_AnswerSources_AnswerSourceId",
                        column: x => x.AnswerSourceId,
                        principalTable: "AnswerSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionAnswers_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionAnswers_ModuleThemePassAttempts_ModuleThemePassAttemptId",
                        column: x => x.ModuleThemePassAttemptId,
                        principalTable: "ModuleThemePassAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionCompaints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ComplaintTypeId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionCompaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionCompaints_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionCompaints_ComplaintTypes_ComplaintTypeId",
                        column: x => x.ComplaintTypeId,
                        principalTable: "ComplaintTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionCompaints_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionsToGoals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    GoalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsToGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionsToGoals_Goals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionsToGoals_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionsToMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsToMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionsToMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionsToMaterials_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionsToSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsToSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionsToSubjects_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionsToSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionsToThemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    ThemeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsToThemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionsToThemes_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionsToThemes_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionsToTrainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    TrainingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsToTrainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionsToTrainings_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionsToTrainings_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionVariants_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Solutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentManagerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solutions_ApplicationUser_ContentManagerId",
                        column: x => x.ContentManagerId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solutions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SourceUsefullnesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsLike = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceUsefullnesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceUsefullnesses_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SourceUsefullnesses_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SourceUsefullnesses_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersToControlWorksToQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsersToControlWorksId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    IsAnswered = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToControlWorksToQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersToControlWorksToQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersToControlWorksToQuestions_UsersToControlWorks_UsersToControlWorksId",
                        column: x => x.UsersToControlWorksId,
                        principalTable: "UsersToControlWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntrantTestQuestionsToThemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThemeId = table.Column<int>(type: "int", nullable: false),
                    EntrantTestQuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrantTestQuestionsToThemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntrantTestQuestionsToThemes_EntrantTestQuestions_EntrantTestQuestionId",
                        column: x => x.EntrantTestQuestionId,
                        principalTable: "EntrantTestQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntrantTestQuestionsToThemes_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswerLrnVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionAnswerId = table.Column<int>(type: "int", nullable: false),
                    VariantValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswerLrnVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswerLrnVariants_QuestionAnswers_QuestionAnswerId",
                        column: x => x.QuestionAnswerId,
                        principalTable: "QuestionAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswerVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionAnswerId = table.Column<int>(type: "int", nullable: false),
                    QuestionVariantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswerVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswerVariants_QuestionAnswers_QuestionAnswerId",
                        column: x => x.QuestionAnswerId,
                        principalTable: "QuestionAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionAnswerVariants_QuestionVariants_QuestionVariantId",
                        column: x => x.QuestionVariantId,
                        principalTable: "QuestionVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_GradeId",
                table: "ApplicationUser",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_LevelId",
                table: "ApplicationUser",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_RegionId",
                table: "ApplicationUser",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_SchoolId",
                table: "ApplicationUser",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleQueues_BattleResultStatusId",
                table: "BattleQueues",
                column: "BattleResultStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleQueues_FirstUserId",
                table: "BattleQueues",
                column: "FirstUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleQueues_SecondUserId",
                table: "BattleQueues",
                column: "SecondUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleQueues_SubjectId",
                table: "BattleQueues",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleQueues_WinnerId",
                table: "BattleQueues",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleQueuesToQuestions_BattleQueueId",
                table: "BattleQueuesToQuestions",
                column: "BattleQueueId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleQueuesToQuestions_QuestionId",
                table: "BattleQueuesToQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleQueuesToThemes_BattleQueueId",
                table: "BattleQueuesToThemes",
                column: "BattleQueueId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleQueuesToThemes_ThemeId",
                table: "BattleQueuesToThemes",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_NewsId",
                table: "Comments",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ReplyingCommentId",
                table: "Comments",
                column: "ReplyingCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlWorks_SubjectId",
                table: "ControlWorks",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlWorks_TypeId",
                table: "ControlWorks",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlWorks_UserId",
                table: "ControlWorks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlWorksToModules_ControlWorkId",
                table: "ControlWorksToModules",
                column: "ControlWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlWorksToModules_ModuleId",
                table: "ControlWorksToModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_EntrantTestQuestions_QuestionId",
                table: "EntrantTestQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_EntrantTestQuestions_SubjectId",
                table: "EntrantTestQuestions",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EntrantTestQuestionsToThemes_EntrantTestQuestionId",
                table: "EntrantTestQuestionsToThemes",
                column: "EntrantTestQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_EntrantTestQuestionsToThemes_ThemeId",
                table: "EntrantTestQuestionsToThemes",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_EntVariants_EntYearId",
                table: "EntVariants",
                column: "EntYearId");

            migrationBuilder.CreateIndex(
                name: "IX_EntVariants_SubjectId",
                table: "EntVariants",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EntVariantsToQuestions_EntVariantId",
                table: "EntVariantsToQuestions",
                column: "EntVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_EntVariantsToQuestions_QuestionId",
                table: "EntVariantsToQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_TypeId",
                table: "Events",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FilesToMaterials_FileId",
                table: "FilesToMaterials",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_FilesToMaterials_MaterialId",
                table: "FilesToMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordsToMaterials_KeywordId",
                table: "KeywordsToMaterials",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordsToMaterials_MaterialId",
                table: "KeywordsToMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ContentManagerId",
                table: "Materials",
                column: "ContentManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialAuthorId",
                table: "Materials",
                column: "MaterialAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialTypeId",
                table: "Materials",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsToThemes_MaterialId",
                table: "MaterialsToThemes",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsToThemes_ThemeId",
                table: "MaterialsToThemes",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Midterm_Questions_LanguageId",
                table: "Midterm_Questions",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Midterm_Questions_SubjectId",
                table: "Midterm_Questions",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Midterm_Questions_VariantId",
                table: "Midterm_Questions",
                column: "VariantId");

            migrationBuilder.CreateIndex(
                name: "IX_Midterm_UserEventQuestions_QuestionId",
                table: "Midterm_UserEventQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Midterm_UserEventQuestions_StatusId",
                table: "Midterm_UserEventQuestions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Midterm_UserEventQuestions_UserEventId",
                table: "Midterm_UserEventQuestions",
                column: "UserEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Midterm_UserEvents_EventId",
                table: "Midterm_UserEvents",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Midterm_UserEvents_StatusId",
                table: "Midterm_UserEvents",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Midterm_UserEvents_UserId",
                table: "Midterm_UserEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Midterm_Users_LanguageId",
                table: "Midterm_Users",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Midterm_Users_TeacherId",
                table: "Midterm_Users",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Midterm_Variants_EventId",
                table: "Midterm_Variants",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Midterm_Variants_SubjectId",
                table: "Midterm_Variants",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_ModuleTypeId",
                table: "Modules",
                column: "ModuleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_SubjectId",
                table: "Modules",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleThemePassAttempts_ThemeId",
                table: "ModuleThemePassAttempts",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleThemePassAttempts_UserId",
                table: "ModuleThemePassAttempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleThemePassAttemptsToQuestions_AttepmtId",
                table: "ModuleThemePassAttemptsToQuestions",
                column: "AttepmtId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleThemePassAttemptsToQuestions_QuestionId",
                table: "ModuleThemePassAttemptsToQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_News_ContentManagerId",
                table: "News",
                column: "ContentManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenQuestionsToMaterialParts_FileId",
                table: "OpenQuestionsToMaterialParts",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenQuestionsToMaterialParts_QuestionId",
                table: "OpenQuestionsToMaterialParts",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenQuestionsToMaterialParts_TextId",
                table: "OpenQuestionsToMaterialParts",
                column: "TextId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenQuestionsToMaterialParts_VideoId",
                table: "OpenQuestionsToMaterialParts",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswerLrnVariants_QuestionAnswerId",
                table: "QuestionAnswerLrnVariants",
                column: "QuestionAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_AnswerSourceId",
                table: "QuestionAnswers",
                column: "AnswerSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_ModuleThemePassAttemptId",
                table: "QuestionAnswers",
                column: "ModuleThemePassAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_QuestionId",
                table: "QuestionAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_UserId",
                table: "QuestionAnswers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswerVariants_QuestionAnswerId",
                table: "QuestionAnswerVariants",
                column: "QuestionAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswerVariants_QuestionVariantId",
                table: "QuestionAnswerVariants",
                column: "QuestionVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCompaints_ComplaintTypeId",
                table: "QuestionCompaints",
                column: "ComplaintTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCompaints_QuestionId",
                table: "QuestionCompaints",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCompaints_UserId",
                table: "QuestionCompaints",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ContentManagerId",
                table: "Questions",
                column: "ContentManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionDifficultyId",
                table: "Questions",
                column: "QuestionDifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsToGoals_GoalId",
                table: "QuestionsToGoals",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsToGoals_QuestionId",
                table: "QuestionsToGoals",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsToMaterials_MaterialId",
                table: "QuestionsToMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsToMaterials_QuestionId",
                table: "QuestionsToMaterials",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsToSubjects_QuestionId",
                table: "QuestionsToSubjects",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsToSubjects_SubjectId",
                table: "QuestionsToSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsToThemes_QuestionId",
                table: "QuestionsToThemes",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsToThemes_ThemeId",
                table: "QuestionsToThemes",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsToTrainings_QuestionId",
                table: "QuestionsToTrainings",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsToTrainings_TrainingId",
                table: "QuestionsToTrainings",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionVariants_QuestionId",
                table: "QuestionVariants",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_RegionId",
                table: "Schools",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_ContentManagerId",
                table: "Solutions",
                column: "ContentManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_QuestionId",
                table: "Solutions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceUsefullnesses_MaterialId",
                table: "SourceUsefullnesses",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceUsefullnesses_QuestionId",
                table: "SourceUsefullnesses",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceUsefullnesses_UserId",
                table: "SourceUsefullnesses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectsToGoals_GoalId",
                table: "SubjectsToGoals",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectsToGoals_SubjectId",
                table: "SubjectsToGoals",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TextsToMaterials_MaterialId",
                table: "TextsToMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_TextsToMaterials_TextId",
                table: "TextsToMaterials",
                column: "TextId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes_GradeId",
                table: "Themes",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes_ParentThemeId",
                table: "Themes",
                column: "ParentThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Themes_SubjectId",
                table: "Themes",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ThemesToModules_ModuleId",
                table: "ThemesToModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ThemesToModules_ThemeId",
                table: "ThemesToModules",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_ThemesTrees_FirstThemeId",
                table: "ThemesTrees",
                column: "FirstThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_ThemesTrees_SecondThemeId",
                table: "ThemesTrees",
                column: "SecondThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Training_TrainingTypeId",
                table: "Training",
                column: "TrainingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCoins_UserId",
                table: "UserCoins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_UserId",
                table: "UserRatings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToControlWorks_ControlWorkId",
                table: "UsersToControlWorks",
                column: "ControlWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToControlWorks_UserId",
                table: "UsersToControlWorks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToControlWorksToQuestions_QuestionId",
                table: "UsersToControlWorksToQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToControlWorksToQuestions_UsersToControlWorksId",
                table: "UsersToControlWorksToQuestions",
                column: "UsersToControlWorksId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToEntVariants_EntVariantId",
                table: "UsersToEntVariants",
                column: "EntVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToEntVariants_UserId",
                table: "UsersToEntVariants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToGoals_GoalId",
                table: "UsersToGoals",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToGoals_UserId",
                table: "UsersToGoals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToLearningPlaces_LearningPlaceId",
                table: "UsersToLearningPlaces",
                column: "LearningPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToLearningPlaces_UserId",
                table: "UsersToLearningPlaces",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToMaterials_MaterialId",
                table: "UsersToMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToMaterials_MaterialLearningStatusId",
                table: "UsersToMaterials",
                column: "MaterialLearningStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToMaterials_UserId",
                table: "UsersToMaterials",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToModules_ModuleId",
                table: "UsersToModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToModules_UserId",
                table: "UsersToModules",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToSubjects_SubjectId",
                table: "UsersToSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToSubjects_UserId",
                table: "UsersToSubjects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToThemes_ModuleId",
                table: "UsersToThemes",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToThemes_ThemeId",
                table: "UsersToThemes",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToThemes_UserId",
                table: "UsersToThemes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToTrainings_TrainingId",
                table: "UsersToTrainings",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToTrainings_TrainingStatusId",
                table: "UsersToTrainings",
                column: "TrainingStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToTrainings_UserId",
                table: "UsersToTrainings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VideosToMaterials_MaterialId",
                table: "VideosToMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_VideosToMaterials_VideoId",
                table: "VideosToMaterials",
                column: "VideoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattleQueuesToQuestions");

            migrationBuilder.DropTable(
                name: "BattleQueuesToThemes");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Constants");

            migrationBuilder.DropTable(
                name: "ControlWorksToModules");

            migrationBuilder.DropTable(
                name: "EntrantTestQuestionsToThemes");

            migrationBuilder.DropTable(
                name: "EntVariantsToQuestions");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "FilesToMaterials");

            migrationBuilder.DropTable(
                name: "KaznituUsers");

            migrationBuilder.DropTable(
                name: "KeywordsToMaterials");

            migrationBuilder.DropTable(
                name: "LRNQuestions");

            migrationBuilder.DropTable(
                name: "MaterialsToThemes");

            migrationBuilder.DropTable(
                name: "Midterm_UserComplaints");

            migrationBuilder.DropTable(
                name: "Midterm_UserEventQuestions");

            migrationBuilder.DropTable(
                name: "ModuleThemePassAttemptsToQuestions");

            migrationBuilder.DropTable(
                name: "OpenQuestionsToMaterialParts");

            migrationBuilder.DropTable(
                name: "QuestionAnswerLrnVariants");

            migrationBuilder.DropTable(
                name: "QuestionAnswerVariants");

            migrationBuilder.DropTable(
                name: "QuestionCompaints");

            migrationBuilder.DropTable(
                name: "QuestionsToGoals");

            migrationBuilder.DropTable(
                name: "QuestionsToMaterials");

            migrationBuilder.DropTable(
                name: "QuestionsToSubjects");

            migrationBuilder.DropTable(
                name: "QuestionsToThemes");

            migrationBuilder.DropTable(
                name: "QuestionsToTrainings");

            migrationBuilder.DropTable(
                name: "RequestLogs");

            migrationBuilder.DropTable(
                name: "Solutions");

            migrationBuilder.DropTable(
                name: "SourceUsefullnesses");

            migrationBuilder.DropTable(
                name: "SubjectsToGoals");

            migrationBuilder.DropTable(
                name: "TextsToMaterials");

            migrationBuilder.DropTable(
                name: "ThemesToModules");

            migrationBuilder.DropTable(
                name: "ThemesTrees");

            migrationBuilder.DropTable(
                name: "UploadFiles");

            migrationBuilder.DropTable(
                name: "UserCoins");

            migrationBuilder.DropTable(
                name: "UserRatings");

            migrationBuilder.DropTable(
                name: "UsersToControlWorksToQuestions");

            migrationBuilder.DropTable(
                name: "UsersToEntVariants");

            migrationBuilder.DropTable(
                name: "UsersToGoals");

            migrationBuilder.DropTable(
                name: "UsersToLearningPlaces");

            migrationBuilder.DropTable(
                name: "UsersToMaterials");

            migrationBuilder.DropTable(
                name: "UsersToModules");

            migrationBuilder.DropTable(
                name: "UsersToSubjects");

            migrationBuilder.DropTable(
                name: "UsersToThemes");

            migrationBuilder.DropTable(
                name: "UsersToTrainings");

            migrationBuilder.DropTable(
                name: "VideosToMaterials");

            migrationBuilder.DropTable(
                name: "BattleQueues");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "EntrantTestQuestions");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropTable(
                name: "KeyWords");

            migrationBuilder.DropTable(
                name: "Midterm_Questions");

            migrationBuilder.DropTable(
                name: "Midterm_QuestionStatuses");

            migrationBuilder.DropTable(
                name: "Midterm_UserEvents");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "QuestionAnswers");

            migrationBuilder.DropTable(
                name: "QuestionVariants");

            migrationBuilder.DropTable(
                name: "ComplaintTypes");

            migrationBuilder.DropTable(
                name: "Texts");

            migrationBuilder.DropTable(
                name: "UsersToControlWorks");

            migrationBuilder.DropTable(
                name: "EntVariants");

            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropTable(
                name: "LearningPlaces");

            migrationBuilder.DropTable(
                name: "MaterialLearningStatuses");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Training");

            migrationBuilder.DropTable(
                name: "TrainingStatus");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "BattleResultStatus");

            migrationBuilder.DropTable(
                name: "Midterm_Variants");

            migrationBuilder.DropTable(
                name: "Midterm_Users");

            migrationBuilder.DropTable(
                name: "Midterm_UserStatuses");

            migrationBuilder.DropTable(
                name: "AnswerSources");

            migrationBuilder.DropTable(
                name: "ModuleThemePassAttempts");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "ControlWorks");

            migrationBuilder.DropTable(
                name: "EntYears");

            migrationBuilder.DropTable(
                name: "ModuleTypes");

            migrationBuilder.DropTable(
                name: "TrainingTypes");

            migrationBuilder.DropTable(
                name: "MaterialAuthors");

            migrationBuilder.DropTable(
                name: "MaterialTypes");

            migrationBuilder.DropTable(
                name: "Midterm_Events");

            migrationBuilder.DropTable(
                name: "Midterm_Subjects");

            migrationBuilder.DropTable(
                name: "Midterm_Languages");

            migrationBuilder.DropTable(
                name: "Midterm_Teachers");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.DropTable(
                name: "QuestionDifficulties");

            migrationBuilder.DropTable(
                name: "QuestionTypes");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "ControlWorkTypes");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
