using BrainTrain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Helpers.Learnosity
{
    public static class Converter
    {
        public static string ConvertFromJson(List<QuestionViewModel> questions)
        {
            var json = "";

            foreach (var question in questions)
            {
                json += $@"{{
                    {question.JsonData}
                    ";
                if (questions.IndexOf(question) == questions.Count - 1)
                {
                    json += "}";
                }
                else
                    json += "},";
            }

            return json;
        }

        public static string ConvertoToLearnosityJson(string uuid, List<QuestionViewModel> questions)
        {
            var json = "";

            foreach (var question in questions)
            {
                json += $@"{{
                    ""response_id"": ""emtihunter{questions.IndexOf(question)}_{uuid}"",
                    {/*""instant_feedback"": false,*/ ""}
                    {/*""is_math"": true,*/ ""}
                    {/*""math_renderer"": ""mathquill"",*/ "" }
                    { /*""stimulus"": ""{Converter.ReplaceLatex(question.Text)}"", */ ""}
                    { /*""type"": ""{ (question.QuestionTypeId == 1 ? "mcq" : question.QuestionTypeId == 4 ? "formulaV2" : "mcq")}"", */ ""}
                    ""questionID"":""{question.Id}"",
                    {/*""isAnswered"":false,*/ ""}
                    {question.JsonData}
                ";

                //if (question.QuestionTypeId == 1)
                //{
                //    json += $@"""multiple_responses"": false,
                //    ""ui_style"": {{
                //        ""type"": ""block"",
                //        ""choice_label"": ""upper-alpha""
                //    }},
                //    ""options"": [";

                //    foreach (var variant in question.QuestionVariants)
                //    {
                //        json += $@"{{
                //        ""value"": ""{variant.Id}"",
                //        ""label"": ""{Converter.ReplaceLatex(variant.Text)}""";

                //        if (question.QuestionVariants.ToList().IndexOf(variant) == question.QuestionVariants.Count-1)
                //            json+="}";
                //        else
                //            json += "},";
                //    }

                //    json += "],";

                //    json += $@"
                //        ""validation"": {{
                //            ""scoring_type"": ""exactMatch"",
                //            ""valid_response"": {{
                //                ""value"": [""{question.QuestionVariants.FirstOrDefault(qv => qv.IsCorrect).Id}""]
                //            }}
                //        }}";                    
                //}
                //else if (question.QuestionTypeId == 4)
                //{
                //    json += @"""case_sensitive"": true,";

                //    json += $@"
                //        ""validation"": {{
                //                ""scoring_type"": ""exactMatch"",
                //                ""valid_response"": {{
                //                        ""value"": [{{""method"":""equivSymbolic"",""value"": ""{question.CorrectAnswerValue.Replace("$", "").Replace("\\", "\\\\")}""}}]
                //                }}
                //        }}";
                //}

                ////убрал isexpanded
                if (questions.IndexOf(question) == questions.Count - 1)
                {
                    json += "}";
                }
                else
                    json += "},";
            }

            return json;
        }

        public static string ReplaceLatex(string text)
        {
            var str = "";
            var flag = false;
            text = text.Replace("\\", "\\\\");
            foreach (var symb in text)
            {
                if (symb == '$')
                {
                    if (flag == true)
                    {
                        str += "\\\\)";
                        flag = false;
                    }
                    else if (flag == false)
                    {
                        str += "\\\\(";
                        flag = true;
                    }
                }
                else
                {
                    str += symb;
                }
            }

            str = str.Replace("\"", "\\\"");

            return str;
        }
    }
}
