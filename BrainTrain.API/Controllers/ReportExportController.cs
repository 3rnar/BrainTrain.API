using Microsoft.AspNetCore.Mvc;

namespace BrainTrain.API.Controllers
{
    public class ReportExportController : ControllerBase
    {
        private const string API_ROUTE_BASE = "api/Files/DownloadForm/";

        /// <summary>
        /// Для скачивания форм
        /// </summary>
        /// <param name="formName">Название формы</param>
        /// <returns>HTTP ответ с файлом указанной формы или архивом содержащий запрошенные отчеты</returns>
        /// private readonly IWebHostEnvironment environment;
        //private readonly IWebHostEnvironment environment;
        //public ReportExportController(IWebHostEnvironment environment) 
        //{
        //    this.environment = environment;
        //}
        //[HttpPost]
        //[Route(API_ROUTE_BASE + "{formName}")]
        //public async Task<HttpResponseMessage> DownloadFormPost(string formName)
        //{
        //    //var tempDir = environment.Ma .MapPath("\\App_Data\\Temp");
        //    var tempDir = Path.Combine(environment.WebRootPath, "\\App_Data\\Temp");
        //    var queryParams = Request.QueryString;

        //    // reflect to readonly property
        //    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);

        //    // make collection editable
        //    isreadonly.SetValue(queryParams, false, null);
        //    string rawMessage;
        //    using (var reader = new StreamReader(Request.Body))
        //    {
        //        rawMessage = await reader.ReadToEndAsync();
        //    }
        //    queryParams.Add("postBody", rawMessage);

        //    // make collection readonly again
        //    isreadonly.SetValue(queryParams, true, null);

        //    return Utils.GetResult(formName, queryParams, tempDir);
        //}

        //[HttpGet]
        //[Route(API_ROUTE_BASE + "{formName}")]
        //public async Task<HttpResponseMessage> DownloadForm(string formName)
        //{
        //    var tempDir = System.Web.Hosting.HostingEnvironment.MapPath("\\App_Data\\Temp");
        //    var queryParams = HttpContext.Current.Request.QueryString;
        //    return Utils.GetResult(formName, queryParams, tempDir);
        //}
    }
}