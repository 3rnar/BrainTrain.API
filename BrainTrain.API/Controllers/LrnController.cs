using BrainTrain.API.Models.LrnViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BrainTrain.API.Controllers
{
    [AllowAnonymous]
    public class LrnController : ApiController
    {
        [HttpPost]
        [Route("api/consumersettings")]
        public IHttpActionResult GetConsumerSettings(ConsumerSettingsViewModel model)
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
        public IHttpActionResult ListName()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~/App_Data/listName.js"));

            dynamic parsedJson = JsonConvert.DeserializeObject(json.Replace("\r\n", ""));

            return Ok(parsedJson);
        }

        [HttpGet]
        [Route("latest/questions/templates/editorV3")]
        public IHttpActionResult TemplatesEditor()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~/App_Data/templatesEditor.txt"));

            dynamic parsedJson = JsonConvert.DeserializeObject(json.Replace("\r\n", ""));

            return Ok(parsedJson);
        }

        [HttpGet]
        [Route("latest/questions/responses/editorV3")]
        public IHttpActionResult ResponsesEditor()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~/App_Data/responsesEditor.txt"));

            dynamic parsedJson = JsonConvert.DeserializeObject(json.Replace("\r\n", ""));

            return Ok(parsedJson);
        }

        [HttpGet]
        [Route("latest/questions/features/editorV3")]
        public IHttpActionResult FeaturesEditorV3()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~/App_Data/featuresEditor.txt"));

            dynamic parsedJson = JsonConvert.DeserializeObject(json.Replace("\r\n", ""));

            return Ok(parsedJson);
        }
    }
}
