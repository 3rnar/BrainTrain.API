using BrainTrain.API.Helpers.Learnosity.Request;
using BrainTrain.API.Helpers.Learnosity.Utilities;
using BrainTrain.Core.Models;
using BrainTrain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Helpers.Learnosity
{
    public class LRNQuestionsHelper
    {
        public static string Simple(List<Question> questions, out string uuid)
        {
            uuid = Uuid.generate();
            string courseId = "mycourse";

            string service = "questions";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);
            security.set("user_id", "demo_student");

            string secret = Credentials.ConsumerSecret;

            JsonObject request = JsonObjectFactory.fromString(LRNQuestionsHelper.requestJson(uuid, courseId, questions));

            Init init = new Init(service, security, secret, request);
            return init.generate();
        }

        public static string Simple(List<LRNQuestion> questions, out string uuid)
        {
            uuid = Uuid.generate();
            string courseId = "mycourse";

            string service = "questions";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);
            security.set("user_id", "demo_student");

            string secret = Credentials.ConsumerSecret;

            JsonObject request = JsonObjectFactory.fromString(LRNQuestionsHelper.requestJson(uuid, courseId, questions));

            Init init = new Init(service, security, secret, request);
            return init.generate();
        }

        public static string Simple(List<Midterm_Question> questions, out string uuid)
        {
            uuid = Uuid.generate();
            string courseId = "mycourse";

            string service = "questions";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);
            security.set("user_id", "demo_student");

            string secret = Credentials.ConsumerSecret;

            JsonObject request = JsonObjectFactory.fromString(LRNQuestionsHelper.requestJson(uuid, courseId, questions));

            Init init = new Init(service, security, secret, request);
            return init.generate();
        }

        public static string Simple(List<CustomerMidtermReviewQuestionViewModel> questions, out string uuid, bool showCorrectAnswers = false)
        {
            uuid = Uuid.generate();
            string courseId = "mycourse";

            string service = "questions";

            JsonObject security = new JsonObject();
            security.set("consumer_key", Credentials.ConsumerKey);
            security.set("domain", Credentials.Domain);
            security.set("user_id", "demo_student");

            string secret = Credentials.ConsumerSecret;

            JsonObject request = JsonObjectFactory.fromString(LRNQuestionsHelper.requestJson(uuid, courseId, questions, showCorrectAnswers));

            Init init = new Init(service, security, secret, request);
            return init.generate();
        }

        private static string requestJson(string uuid, string courseId, List<Question> questions)
        {
            var json = $@"{{
                ""type"": ""local_practice"",
                ""state"": ""initial"",
                ""id"": ""questionsapi-demo"",
                ""name"": ""Questions API Demo"",
                ""course_id"": ""{courseId}"",
                ""questions"": [
                    {
                    CombineQuestion(questions, uuid)
                }
                ]
            }}";

            return json;
        }

        private static string requestJson(string uuid, string courseId, List<LRNQuestion> questions)
        {
            var json = $@"{{
                ""type"": ""local_practice"",
                ""state"": ""initial"",
                ""id"": ""questionsapi-demo"",
                ""name"": ""Questions API Demo"",
                ""course_id"": ""{courseId}"",
                ""questions"": [
                    {
                    CombineQuestion(questions, uuid)
                }
                ]
            }}";

            return json;
        }

        private static string requestJson(string uuid, string courseId, List<Midterm_Question> questions)
        {
            var json = $@"{{
                ""type"": ""local_practice"",
                ""state"": ""initial"",
                ""id"": ""questionsapi-demo"",
                ""name"": ""Questions API Demo"",
                ""course_id"": ""{courseId}"",
                ""questions"": [
                    {
                    CombineQuestion(questions, uuid)
                }
                ]
            }}";

            return json;
        }

        private static string requestJson(string uuid, string courseId, List<CustomerMidtermReviewQuestionViewModel> questions, bool showCorrectAnswers)
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
                    {CombineQuestion(questions, uuid)}
                ]
            }}";

            return json;
        }

        private static string CombineQuestion(List<Question> questions, string uuid)
        {
            var json = "";

            foreach (var question in questions)
            {
                json += $@"
                {{
                    ""response_id"": ""demoformula{questions.IndexOf(question)}_{uuid}"",
                    ""questionId"": ""{question.Id}"",
                    {question.JsonData}
                }}";

                if (questions.IndexOf(question) != questions.Count - 1)
                {
                    json += ",";
                }
            }

            return json;
        }

        private static string CombineQuestion(List<LRNQuestion> questions, string uuid)
        {
            var json = "";

            foreach (var question in questions)
            {
                json += $@"
                {{
                    ""response_id"": ""demoformula{questions.IndexOf(question)}_{uuid}"",
                    ""questionID"": ""{question.Id}"",
                    {question.JsonData}
                }}";

                if (questions.IndexOf(question) != questions.Count - 1)
                {
                    json += ",";
                }
            }

            return json;
        }

        private static string CombineQuestion(List<Midterm_Question> questions, string uuid)
        {
            var json = "";

            foreach (var question in questions)
            {
                json += $@"
                {{
                    ""response_id"": ""demoformula{questions.IndexOf(question)}_{uuid}"",
                    ""questionID"": ""{question.Id}"",
                    ""order"": ""{question.Order}"",
                    ""variantId"": ""{question.VariantId}"",
                    ""weight"": ""{question.Weight}"",
                    ""languageId"": ""{question.LanguageId}"",
                    ""subjectId"": ""{question.SubjectId}"",
                    {question.LrnJson}
                }}";

                if (questions.IndexOf(question) != questions.Count - 1)
                {
                    json += ",";
                }
            }

            return json;
        }

        private static string CombineQuestion(List<CustomerMidtermReviewQuestionViewModel> questions, string uuid)
        {
            var json = "";

            foreach (var question in questions)
            {
                json += $@"
                {{
                    ""response_id"": ""demoformula{questions.IndexOf(question)}_{uuid}"",
                    ""questionID"": ""{question.Id}"",
                    ""order"": ""{question.Order}"",
                    ""variantId"": ""{question.VariantId}"",
                    ""weight"": ""{question.Weight}"",
                    ""languageId"": ""{question.LanguageId}"",
                    ""subjectId"": ""{question.SubjectId}"",";

                if (!string.IsNullOrEmpty(question.UserAnswer))
                {
                    json += $@"""userAnswer"": ""{Converter.ReplaceLatex(question.UserAnswer)}"",";
                    json += $@"""isUserAnswerCorrect"": ""{question.IsUserAnswerCorrect}"",";
                }

                json += $@"""statusId"": ""{question.StatusId}"",";

                json += $@"
                    {question.LrnJson}
                }}";

                if (questions.IndexOf(question) != questions.Count - 1)
                {
                    json += ",";
                }
            }

            return json;
        }
    }
}
