using BrainTrain.API.Helpers.Learnosity.Request;
using BrainTrain.API.Helpers.Learnosity.Utilities;
using BrainTrain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Helpers.Learnosity
{
    public class LQuestions
    {
        public static string Simple(List<QuestionViewModel> questions, out string uuid, bool showCorrectAnswers = false)
        {
            uuid = Uuid.generate();
            string courseId = "mycourse";

            string service = "questions";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);
            security.set("user_id", "demo_student");

            string secret = Credentials.ConsumerSecret;

            JsonObject request = JsonObjectFactory.fromString(LQuestions.requestJson(uuid, courseId, questions, showCorrectAnswers));

            Init init = new Init(service, security, secret, request);
            return init.generate();
        }

        private static string requestJson(string uuid, string courseId, List<QuestionViewModel> questions, bool showCorrectAnswers)
        {
            var json = $@"{{
                ""type"": ""local_practice"",
                ""state"": ""initial"",
                ""id"": ""questionsapi-demo"",
                ""name"": ""Questions API Demo"",
                ""course_id"": ""{courseId}""," +
                (showCorrectAnswers == true ?
                @"""state"":""review"",
                ""showCorrectAnswers"":true," :
                "") +
                $@"""questions"": [
                    {Converter.ConvertoToLearnosityJson(uuid, questions)}
                ]
            }}";

            return json;
        }
    }
}
