using BrainTrain.Core.Models;
using BrainTrain.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [Route("api/Customer/Stuff")]
    public class CustomerStuffController : BaseApiController
    {
        private readonly IWebHostEnvironment _environment;
        public CustomerStuffController(BrainTrainContext _db, IWebHostEnvironment environment) : base(_db)
        {
            _environment = environment;
        }

        [HttpGet]
        [Route("News")]
        public IEnumerable<News> GetNews()
        {
            return db.News.Select(n => new {
                id = n.Id,
                title = n.Title,
                text = n.Text,
                dateCreated = n.DateCreated,
                imageUrl = n.ImageUrl,
                contentManager = new { firstName = n.ContentManager.FirstName, lastName = n.ContentManager.LastName },
                comments = n.Comments.Select(c => new {
                    id = c.Id,
                    text = c.Text,
                    imageUrl = c.ImageUrl,
                    replyingCommentId = c.ReplyingCommentId,
                    dateCreated = c.DateCreated,
                    user = new { firstName = c.User.FirstName, lastName = c.User.LastName }
                }).ToList()
            }).AsEnumerable().Select(n => new News {
                Id = n.id,
                Title = n.title,
                Text = n.text,
                DateCreated = n.dateCreated,
                ImageUrl = n.imageUrl,
                ContentManager = new ApplicationUser { FirstName = n.contentManager.firstName, LastName = n.contentManager.lastName },
                Comments = n.comments.Select(c => new Comment {
                    Id = c.id,
                    Text = c.text,
                    ImageUrl = c.imageUrl,
                    ReplyingCommentId = c.replyingCommentId,
                    DateCreated = c.dateCreated,
                    User = new ApplicationUser { FirstName = c.user.firstName, LastName = c.user.lastName }
                }).ToList()
            }).ToList()
                ;
        }

        [HttpGet]
        [Route("NewsComments")]
        public IEnumerable<Comment> GetNewsComments(int newsId)
        {
            return db.Comments.Select(c => new {
                id = c.Id,
                text = c.Text,
                imageUrl = c.ImageUrl,
                replyingCommentId = c.ReplyingCommentId,
                dateCreated = c.DateCreated,
                user = new { firstName = c.User.FirstName, lastName = c.User.LastName }
            }).AsEnumerable().Select(c => new Comment {
                Id = c.id,
                Text = c.text,
                ImageUrl = c.imageUrl,
                ReplyingCommentId = c.replyingCommentId,
                DateCreated = c.dateCreated,
                User = new ApplicationUser { FirstName = c.user.firstName, LastName = c.user.lastName }
            }).ToList();
        }

        [HttpPost]
        [Route("AddComment")]
        public async Task<IActionResult> PostNewComment(CommentViewModel model)
        {
            var userId = UserId;
            var comment = new Comment { Text = model.Text, NewsId = model.NewsId, UserId = userId, DateCreated = DateTime.Now };
            if (model.ReplyingCommentId != null)
            {
                comment.ReplyingCommentId = model.ReplyingCommentId;
            }

            //if (Request.Content.IsMimeMultipartContent())
            //{
            var uploadFiles = new List<UploadFile>();

            foreach (var file in Request.Form.Files)
            {
                var fileNameInFileSystem = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                var filePath = Path.Combine(_environment.WebRootPath, "~/App_Data/uploads/") + fileNameInFileSystem;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var uploadFile = new UploadFile
                {
                    BlobUrl = "~/App_Data/uploads/" + fileNameInFileSystem,
                    DateCreated = DateTime.Now,
                    FileName = file.FileName
                };
                uploadFiles.Add(uploadFile);
            }

            db.UploadFiles.AddRange(uploadFiles);
            await db.SaveChangesAsync();

            if (uploadFiles.Count > 0)
                comment.ImageUrl = "/api/UploadFiles/" + uploadFiles[0].Id;
            //}

            db.Comments.Add(comment);
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("GetComplaintTypes")]
        public IEnumerable<ComplaintType> GetComplaintTypes()
        {
            return db.ComplaintTypes.ToList();
        }

        [HttpPost]
        [Route("PostCompliant")]
        public IActionResult PostCompliant(QuestionCompaint model)
        {
            var userId = UserId;

            if (model != null)
            {
                db.QuestionCompaints.Add(new QuestionCompaint { ComplaintTypeId = model.ComplaintTypeId, UserId = userId, QuestionId = model.QuestionId, Comment = model.Comment });
                db.SaveChanges();
                return Ok();
            }

            return NotFound();
        }

        [HttpPost]
        [Route("LikeSource")]
        public IActionResult LikeSource(SourceUsefullnessViewModel model)
        {
            var userId = UserId;
            SourceUsefullness sourceUsefullness = new SourceUsefullness {
                UserId = userId,
                IsLike = model.IsLike,
                MaterialId = model.MaterialId,
                QuestionId = model.QuestionId
            };

            db.SourceUsefullnesses.Add(sourceUsefullness);

            db.SaveChanges();

            return Ok();
        }

    }
}
