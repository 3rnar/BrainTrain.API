using IEIT.Reports.WebFramework.Api.Resolvers;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BrainTrain.API.Controllers
{
    public class ReportExportController : ApiController
    {
        private const string API_ROUTE_BASE = "api/Files/DownloadForm/";

        /// <summary>
        /// Для скачивания форм
        /// </summary>
        /// <param name="formName">Название формы</param>
        /// <returns>HTTP ответ с файлом указанной формы или архивом содержащий запрошенные отчеты</returns>
        [HttpPost]
        [Route(API_ROUTE_BASE + "{formName}")]
        public async Task<HttpResponseMessage> DownloadFormPost(string formName)
        {
            var tempDir = System.Web.Hosting.HostingEnvironment.MapPath("\\App_Data\\Temp");
            var queryParams = HttpContext.Current.Request.QueryString;

            // reflect to readonly property
            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);

            // make collection editable
            isreadonly.SetValue(queryParams, false, null);

            var rawMessage = await Request.Content.ReadAsStringAsync();
            queryParams.Add("postBody", rawMessage);

            // make collection readonly again
            isreadonly.SetValue(queryParams, true, null);

            return Utils.GetResult(formName, queryParams, tempDir);
        }

        [HttpGet]
        [Route(API_ROUTE_BASE + "{formName}")]
        public async Task<HttpResponseMessage> DownloadForm(string formName)
        {
            var tempDir = System.Web.Hosting.HostingEnvironment.MapPath("\\App_Data\\Temp");
            var queryParams = HttpContext.Current.Request.QueryString;
            return Utils.GetResult(formName, queryParams, tempDir);
        }
    }
}