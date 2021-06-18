using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BrainTrain.API.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [RoutePrefix("api/Customer/Stuff")]
    public class CustomerStuffController : BaseApiController
    {
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
        public async Task<IHttpActionResult> PostNewComment(CommentViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var comment = new Comment { Text = model.Text, NewsId = model.NewsId, UserId = userId, DateCreated = DateTime.Now };
            if (model.ReplyingCommentId != null)
            {
                comment.ReplyingCommentId = model.ReplyingCommentId;
            }

            if (Request.Content.IsMimeMultipartContent())
            {
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

                if (uploadFiles.Count > 0)
                    comment.ImageUrl = "/api/UploadFiles/" + uploadFiles[0].Id;
            }

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
        public IHttpActionResult PostCompliant(QuestionCompaint model)
        {
            var userId = User.Identity.GetUserId();

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
        public IHttpActionResult LikeSource(SourceUsefullnessViewModel model)
        {
            var userId = User.Identity.GetUserId();
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
