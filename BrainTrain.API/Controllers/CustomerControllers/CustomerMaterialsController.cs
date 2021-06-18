using BrainTrain.Core.Models;
using BrainTrain.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [Route("api/Customer/Materials")]
    //jhh
    public class CustomerMaterialsController : BaseApiController
    {
        public CustomerMaterialsController(BrainTrainContext _db) : base(_db)
        {
        }

        [HttpGet]
        [Route("ThemeMaterials")]
        public IEnumerable<ThemeMaterialViewModel> ThemeMaterials(int themeId)
        {
            var mats = db.Materials.
                Where(m => m.MaterialsToThemes.Any(mtt => mtt.ThemeId == themeId) && m.MaterialTypeId == 1).
                Select(m => new 
            {
                Id = m.Id,
                Title = m.Title,
                Texts = m.TextsToMaterials.Select(ttm => new 
                {
                    Id = ttm.Text.Id,
                    Title = ttm.Text.Title,
                    PreText = ttm.Text.PreText,
                    FullText = ttm.Text.FullText,
                    OpenQuestions = ttm.Text.OpenQuestionsToMaterialParts.Select(oq => new
                    {
                        Id = oq.Question.Id,
                        QuestionTypeId = oq.Question.QuestionTypeId,
                        Text = oq.Question.Text,
                        CorrectAnswerValue = oq.Question.CorrectAnswerValue,
                        Variants = oq.Question.QuestionVariants.Select(qv => new { id = qv.Id, questionId = qv.QuestionId, text = qv.Text, isCorrect = qv.IsCorrect }).ToList(),
                        Solutions = oq.Question.Solutions.Select(s => new { id = s.Id, text = s.Text, questionId = s.QuestionId }).ToList()
                    }).ToList()
                }).ToList()
            }).AsEnumerable().Select(m => new ThemeMaterialViewModel {
                Id = m.Id,
                Title = m.Title,
                Texts = m.Texts.Select(t => new ThemeMatText {
                    Id = t.Id,
                    Title = t.Title,
                    PreText = t.PreText,
                    FullText = t.FullText,
                    OpenQuestions = t.OpenQuestions.Select(oq => new Question { Id = oq.Id, QuestionTypeId = oq.QuestionTypeId, Text = oq.Text, CorrectAnswerValue = oq.CorrectAnswerValue, QuestionVariants = oq.Variants.Select(v=>new QuestionVariant { Id = v.id, QuestionId = v.questionId, Text = v.text, IsCorrect = v.isCorrect }).ToList(), Solutions = oq.Solutions.Select(s => new Solution { Id = s.id, Text = s.text, QuestionId = s.questionId }).ToList() }).ToList()
                }).ToList()
            }).ToList();

            return mats;
        }

        [HttpGet]
        [Route("ThemeVideoMaterials")]
        public IEnumerable<ThemeMaterialViewModel> ThemeVideoMaterials(int themeId)
        {
            var mats = db.Materials.
                Where(m => m.MaterialsToThemes.Any(mtt => mtt.ThemeId == themeId) && m.MaterialTypeId == 2).
                Select(m => new
                {
                    Id = m.Id,
                    Title = m.Title,
                    Videos = m.VideosToMaterials.Select(vtm => new
                    {
                        Id = vtm.Video.Id,
                        Title = vtm.Video.Title,
                        PreText = vtm.Video.PreText,
                        Url = vtm.Video.Url,
                        OpenQuestions = vtm.Video.OpenQuestionsToMaterialParts.Select(oq => new
                        {
                            Id = oq.Question.Id,
                            QuestionTypeId = oq.Question.QuestionTypeId,
                            Text = oq.Question.Text,
                            CorrectAnswerValue = oq.Question.CorrectAnswerValue,
                            Variants = oq.Question.QuestionVariants.Select(qv => new { id = qv.Id, questionId = qv.QuestionId, text = qv.Text, isCorrect = qv.IsCorrect }).ToList(),
                            Solutions = oq.Question.Solutions.Select(s => new { id = s.Id, text = s.Text, questionId = s.QuestionId }).ToList()
                        }).ToList()
                    }).ToList()
                }).AsEnumerable().Select(m => new ThemeMaterialViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Videos = m.Videos.Select(t => new ThemeMatVideo
                    {
                        Id = t.Id,
                        Title = t.Title,
                        PreText = t.PreText,
                        Url = t.Url,
                        OpenQuestions = t.OpenQuestions.Select(oq => new Question { Id = oq.Id, QuestionTypeId = oq.QuestionTypeId, Text = oq.Text, CorrectAnswerValue = oq.CorrectAnswerValue, QuestionVariants = oq.Variants.Select(v => new QuestionVariant { Id = v.id, QuestionId = v.questionId, Text = v.text, IsCorrect = v.isCorrect }).ToList(), Solutions = oq.Solutions.Select(s => new Solution { Id = s.id, Text = s.text, QuestionId = s.questionId }).ToList() }).ToList()
                    }).ToList()
                }).ToList();

            return mats;
        }

        [HttpGet]
        [Route("ThemeFileMaterials")]
        public IEnumerable<ThemeMaterialViewModel> ThemeFileMaterials(int themeId)
        {
            var mats = db.Materials.
                Where(m => m.MaterialsToThemes.Any(mtt => mtt.ThemeId == themeId) && m.MaterialTypeId == 3).
                Select(m => new
                {
                    Id = m.Id,
                    Title = m.Title,
                    Files = m.FilesToMaterials.Select(ftm => new
                    {
                        Id = ftm.File.Id,
                        Title = ftm.File.Title,
                        PreText = ftm.File.PreText,
                        Url = ftm.File.Url,
                        OpenQuestions = ftm.File.OpenQuestionsToMaterialParts.Select(oq => new
                        {
                            Id = oq.Question.Id,
                            QuestionTypeId = oq.Question.QuestionTypeId,
                            Text = oq.Question.Text,
                            CorrectAnswerValue = oq.Question.CorrectAnswerValue,
                            Variants = oq.Question.QuestionVariants.Select(qv => new { id = qv.Id, questionId = qv.QuestionId, text = qv.Text, isCorrect = qv.IsCorrect }).ToList(),
                            Solutions = oq.Question.Solutions.Select(s => new { id = s.Id, text = s.Text, questionId = s.QuestionId }).ToList()
                        }).ToList()
                    }).ToList()
                }).AsEnumerable().Select(m => new ThemeMaterialViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Files = m.Files.Select(t => new ThemeMatFile
                    {
                        Id = t.Id,
                        Title = t.Title,
                        PreText = t.PreText,
                        Url = t.Url,
                        OpenQuestions = t.OpenQuestions.Select(oq => new Question { Id = oq.Id, QuestionTypeId = oq.QuestionTypeId, Text = oq.Text, CorrectAnswerValue = oq.CorrectAnswerValue, QuestionVariants = oq.Variants.Select(v => new QuestionVariant { Id = v.id, QuestionId = v.questionId, Text = v.text, IsCorrect = v.isCorrect }).ToList(), Solutions = oq.Solutions.Select(s => new Solution { Id = s.id, Text = s.text, QuestionId = s.questionId }).ToList() }).ToList()
                    }).ToList()
                }).ToList();

            return mats;
        }

        [HttpGet]
        [Route("TextMaterial")]
        public Text TextMaterial(int textId)
        {
            var text = db.Texts.FirstOrDefault(t => t.Id == textId);

            if (text == null)
                NotFound();

            return text;
        }

        [HttpGet]
        [Route("VideoMaterial")]
        public Video VideoMaterial(int videoId)
        {
            var video = db.Videos.FirstOrDefault(v => v.Id == videoId);

            if (video == null)
                NotFound();

            return video;
        }

        [HttpGet]
        [Route("ThemeMeaterials")]
        public async Task<IEnumerable<Material>> GetMaterialsByThemeId(int themeId)
        {
            var mats = db.Materials.ToList();

            return mats;
        }

        [HttpGet]
        [Route("FileMaterial")]
        public File FileMaterial(int fileId)
        {
            var file = db.Files.FirstOrDefault(f => f.Id == fileId);

            if (file == null)
                NotFound();

            return file;
        }

        //[HttpGet]
        //[Route("TextsByMaterial")]
        //public IEnumerable<Text> GetTexts(int materialId)
        //{
        //    var texts = db.Texts.Where(t => t.TextsToMaterials.Any(ttm => ttm.MaterialId == materialId)).Select(t => new {
        //        id = t.Id,
        //        title = t.Title,
        //        pretext = t.PreText,
        //        fullText = t.FullText,
        //        oq = t.OpenQuestionsToMaterialParts.Select(oqtm => new { id = oqtm.Id, textId = oqtm.TextId, questionId = oqtm.QuestionId, question = new { id = oqtm.Question.Id, text = oqtm.Question.Text, correctAnswer = oqtm.Question.CorrectAnswerValue } })
        //    }).AsEnumerable().Select(t => new Text {
        //        Id = t.id,
        //        Title = t.title,
        //        PreText = t.pretext,
        //        FullText = t.fullText,
        //        OpenQuestionsToMaterialParts = t.oq.Select(oqtm => new OpenQuestionsToMaterialParts { Id = oqtm.id, TextId = oqtm.textId, QuestionId = oqtm.questionId, Question = new Question { Id = oqtm.question.id, Text = oqtm.question.text, CorrectAnswerValue = oqtm.question.correctAnswer } }).ToList()
        //    });

        //    return texts;
        //}

        //[HttpGet]
        //[Route("VideosByMaterial")]
        //public IEnumerable<Video> GetVids(int materialId)
        //{
        //    var vids = db.Videos.Where(t => t.VideosToMaterials.Any(ttm => ttm.MaterialId == materialId)).Select(t => new {
        //        id = t.Id,
        //        title = t.Title,
        //        pretext = t.PreText,
        //        url = t.Url,
        //        oq = t.OpenQuestionsToMaterialParts.Select(oqtm => new { id = oqtm.Id, videoId = oqtm.VideoId, questionId = oqtm.QuestionId, question = new { id = oqtm.Question.Id, text = oqtm.Question.Text, correctAnswer = oqtm.Question.CorrectAnswerValue } })
        //    }).AsEnumerable().Select(t => new Video
        //    {
        //        Id = t.id,
        //        Title = t.title,
        //        PreText = t.pretext,
        //        Url = t.url,
        //        OpenQuestionsToMaterialParts = t.oq.Select(oqtm => new OpenQuestionsToMaterialParts { Id = oqtm.id, VideoId = oqtm.videoId, QuestionId = oqtm.questionId, Question = new Question { Id = oqtm.question.id, Text = oqtm.question.text, CorrectAnswerValue = oqtm.question.correctAnswer } }).ToList()
        //    });

        //    return vids;
        //}

        //[HttpGet]
        //[Route("FilesByMaterial")]
        //public IEnumerable<File> GetFiles(int materialId)
        //{
        //    var files = db.Files.Where(t => t.FilesToMaterials.Any(ttm => ttm.MaterialId == materialId)).Select(t => new {
        //        id = t.Id,
        //        title = t.Title,
        //        pretext = t.PreText,
        //        url = t.Url,
        //        oq = t.OpenQuestionsToMaterialParts.Select(oqtm => new { id = oqtm.Id, fileId = oqtm.FileId, questionId = oqtm.QuestionId, question = new { id = oqtm.Question.Id, text = oqtm.Question.Text, correctAnswer = oqtm.Question.CorrectAnswerValue } })
        //    }).AsEnumerable().Select(t => new File
        //    {
        //        Id = t.id,
        //        Title = t.title,
        //        PreText = t.pretext,
        //        Url = t.url,
        //        OpenQuestionsToMaterialParts = t.oq.Select(oqtm => new OpenQuestionsToMaterialParts { Id = oqtm.id, FileId = oqtm.fileId, QuestionId = oqtm.questionId, Question = new Question { Id = oqtm.question.id, Text = oqtm.question.text, CorrectAnswerValue = oqtm.question.correctAnswer } }).ToList()
        //    });

        //    return files;
        //}

    }
}
