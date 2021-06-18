using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using BrainTrain.API.Models;
using BrainTrain.Core.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер,Заполнение вопросов")]
    public class UploadFilesController : BaseApiController
    {
        // GET: api/UploadFiles
        [HttpGet]
        [Route("api/UploadFiles")]
        public IEnumerable<UploadFile> GetUploadFiles()
        {
            var ufs = db.UploadFiles.OrderByDescending(uf => uf.DateCreated).ToList();

            foreach (var uploadF in ufs)
            {
                uploadF.BlobUrl = "/api/UploadFiles/" + uploadF.Id.ToString();
            }

            return ufs;
        }

        //// GET: api/UploadFiles/5   AZURE BLOB
        //[AllowAnonymous]
        //[ResponseType(typeof(UploadFile))]
        //[HttpGet]
        //[Route("api/UploadFiles/{id:Guid}")]
        //public async Task<IHttpActionResult> GetUploadFile(Guid id)
        //{
        //    UploadFile uploadFile = await db.UploadFiles.FindAsync(id);

        //    if (uploadFile == null)
        //    {
        //        return NotFound();
        //    }
        //    var blobName = Path.GetFileName(uploadFile.BlobUrl);

        //    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
        //        CloudConfigurationManager.GetSetting("StorageConnectionString"));
        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        //    CloudBlobContainer container = blobClient.GetContainerReference("uploadfiles");
        //    CloudBlockBlob blockBlob2 = container.GetBlockBlobReference(blobName);


        //    using (var memoryStream = new MemoryStream())
        //    {
        //        blockBlob2.DownloadToStream(memoryStream);

        //        var result = new HttpResponseMessage(HttpStatusCode.OK)
        //        {
        //            Content = new ByteArrayContent(memoryStream.GetBuffer())
        //        };
        //        result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //        {
        //            FileName = string.IsNullOrEmpty(uploadFile.FileName) ?  blobName : uploadFile.FileName
        //        };
        //        result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        //        var response = ResponseMessage(result);

        //        return response;
        //    }

        //}

        // GET: api/UploadFiles/5
        [AllowAnonymous]
        [ResponseType(typeof(UploadFile))]
        [HttpGet]
        [Route("api/UploadFiles/{id:Guid}")]
        public async Task<IHttpActionResult> GetUploadFile(Guid id)
        {
            UploadFile uploadFile = await db.UploadFiles.FindAsync(id);

            if (uploadFile == null)
            {
                return NotFound();
            }

            var stream = new FileStream(HttpContext.Current.Server.MapPath(uploadFile.BlobUrl), FileMode.Open, FileAccess.Read);
            
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream)
            };
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = uploadFile.FileName
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            var response = ResponseMessage(result);

            return response;
        }

        //// POST: api/UploadFiles   WITH AZURE BLOB
        //[ResponseType(typeof(List<UploadFile>))]
        //[HttpPost]
        //[Route("api/UploadFiles", Name = "PostUploadFile")]
        //public async Task<HttpResponseMessage> PostUploadFile()
        //{
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    //azure stirage settings
        //    var storageAccount = CloudStorageAccount.Parse(
        //        CloudConfigurationManager.GetSetting("StorageConnectionString"));
        //    var blobClient = storageAccount.CreateCloudBlobClient();
        //    var container = blobClient.GetContainerReference("uploadfiles");

        //    var uploadFiles = new List<UploadFile>();
        //    var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());
        //    NameValueCollection formData = provider.FormData;
        //    IList<HttpContent> files = provider.Files;

        //    foreach (var file in files)
        //    {
        //        var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
        //        var input = await file.ReadAsStreamAsync();

        //        var blockBlob = container.GetBlockBlobReference(Guid.NewGuid().ToString() + Path.GetExtension(fileName));
        //        blockBlob.UploadFromStream(input);

        //        var uploadFile = new UploadFile
        //        {
        //            BlobUrl = blockBlob.Uri.AbsoluteUri,
        //            DateCreated = DateTime.Now,
        //            FileName = fileName
        //        };
        //        uploadFiles.Add(uploadFile);
        //    }

        //    db.UploadFiles.AddRange(uploadFiles);
        //    await db.SaveChangesAsync();

        //    return Request.CreateResponse(HttpStatusCode.OK, uploadFiles);
        //}

        // POST: api/UploadFiles
        [ResponseType(typeof(List<UploadFile>))]
        [HttpPost]
        [Route("api/UploadFiles", Name = "PostUploadFile")]
        public async Task<HttpResponseMessage> PostUploadFile()
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

            return Request.CreateResponse(HttpStatusCode.OK, uploadFiles);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/UploadFiles/FroalaUpload")]
        public async Task<FroalaUploadFileViewModel> PostFroalaFile()
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

            return new FroalaUploadFileViewModel { link = HostName + "api/UploadFiles/" + uploadFiles[0].Id };
        }

        // DELETE: api/UploadFiles/5
        [ResponseType(typeof(UploadFile))]
        [HttpDelete]
        [Route("api/UploadFiles/{id:Guid}")]
        public async Task<IHttpActionResult> DeleteUploadFile(Guid id)
        {
            UploadFile uploadFile = await db.UploadFiles.FindAsync(id);
            if (uploadFile == null)
            {
                return NotFound();
            }

            db.UploadFiles.Remove(uploadFile);
            await db.SaveChangesAsync();

            return Ok(uploadFile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UploadFileExists(Guid id)
        {
            return db.UploadFiles.Count(e => e.Id == id) > 0;
        }
    }
}