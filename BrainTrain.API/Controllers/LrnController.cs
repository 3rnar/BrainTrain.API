using BrainTrain.Core.Models;
using BrainTrain.Core.ViewModels.LrnViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BrainTrain.API.Controllers
{
    [AllowAnonymous]
    public class LrnController : BaseApiController
    {
        public LrnController(BrainTrainContext _db) : base(_db)
        {
        }

        [HttpPost]
        [Route("api/consumersettings")]
        public IActionResult GetConsumerSettings(ConsumerSettingsViewModel model)
        {
            var str = @"{
                ""meta"": {
                ""status"": true,
                ""timestamp"": 1543203050,
                ""versions"": {
                            ""requested"": ""v1"",
                    ""mapped"": ""v1"",
                    ""concrete"": ""v1.41.0""
                },
                ""records"": 2
                },
                ""data"": {
                ""default_organisation_id"": 6,
                ""feature_flags"": {
                    ""question_editor_flags"": {
                    ""question_types"": [""video""],
                    ""attributes"": {
                        ""fileupload"": [""allow_video""]
                },
                    ""record_video"": true
                    }
                }
                }
            }";

            dynamic parsedJson = JsonConvert.DeserializeObject(str.Replace("\r\n", ""));

            return Ok(parsedJson);

        }

        [HttpGet]
        [Route("latest/questions/list/name")]
        public IActionResult ListName()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~/App_Data/listName.js"));

            dynamic parsedJson = JsonConvert.DeserializeObject(json.Replace("\r\n", ""));

            return Ok(parsedJson);
        }

        [HttpGet]
        [Route("latest/questions/templates/editorV3")]
        public IActionResult TemplatesEditor()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~/App_Data/templatesEditor.txt"));

            dynamic parsedJson = JsonConvert.DeserializeObject(json.Replace("\r\n", ""));

            return Ok(parsedJson);
        }

        [HttpGet]
        [Route("latest/questions/responses/editorV3")]
        public IActionResult ResponsesEditor()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~/App_Data/responsesEditor.txt"));

            dynamic parsedJson = JsonConvert.DeserializeObject(json.Replace("\r\n", ""));

            return Ok(parsedJson);
        }

        [HttpGet]
        [Route("latest/questions/features/editorV3")]
        public IActionResult FeaturesEditorV3()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~/App_Data/featuresEditor.txt"));

            dynamic parsedJson = JsonConvert.DeserializeObject(json.Replace("\r\n", ""));

            return Ok(parsedJson);
        }
    }
}
