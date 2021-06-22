using BrainTrain.Core.Models;
using BrainTrain.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер,Заполнение вопросов")]
    public class UploadFilesController : BaseApiController
    {
        private readonly IWebHostEnvironment _environment;

        public UploadFilesController(BrainTrainContext _db, IWebHostEnvironment environment) : base(_db)
        {
            _environment = environment;
        }

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
        //public async Task<IActionResult> GetUploadFile(Guid id)
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
        [HttpGet]
        [Route("api/UploadFiles/{id:Guid}")]
        public async Task<IActionResult> GetUploadFile(Guid id)
        {
            UploadFile uploadFile = await db.UploadFiles.FindAsync(id);

            if (uploadFile == null)
            {
                return NotFound();
            }

            var stream = new FileStream(Path.Combine(_environment.WebRootPath, uploadFile.BlobUrl), FileMode.Open, FileAccess.Read);
            
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream)
            };
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = uploadFile.FileName
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");


            return Ok(result);
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
        [HttpPost]
        [Route("api/UploadFiles", Name = "PostUploadFile")]
        public async Task<IActionResult> PostUploadFile()
        {
            //if (!Request.Content.IsMimeMultipartContent())
            //{
            //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            //}
            
            var uploadFiles = new List<UploadFile>();
            var httpRequest = Request;

            foreach (var file in httpRequest.Form.Files)
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

            return  Ok(uploadFiles);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/UploadFiles/FroalaUpload")]
        public async Task<FroalaUploadFileViewModel> PostFroalaFile()
        {
            //if (!Request.Content.IsMimeMultipartContent())
            //{
            //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            //}

            var uploadFiles = new List<UploadFile>();
            var httpRequest = Request;

            foreach (var file in httpRequest.Form.Files)
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

            return new FroalaUploadFileViewModel { link = _environment.ApplicationName  + "api/UploadFiles/" + uploadFiles[0].Id };
        }

        // DELETE: api/UploadFiles/5
        [HttpDelete]
        [Route("api/UploadFiles/{id:Guid}")]
        public async Task<IActionResult> DeleteUploadFile(Guid id)
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

        private bool UploadFileExists(Guid id)
        {
            return db.UploadFiles.Count(e => e.Id == id) > 0;
        }
    }
}