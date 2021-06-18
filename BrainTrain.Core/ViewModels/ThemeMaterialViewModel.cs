using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class ThemeMaterialViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<ThemeMatText> Texts { get; set; }
        public List<ThemeMatVideo> Videos { get; set; }
        public List<ThemeMatFile> Files { get; set; }
    }

    public class ThemeMatPart
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PreText { get; set; }

        public List<Question> OpenQuestions { get; set; }
    }

    public class ThemeMatText: ThemeMatPart
    {
        public string FullText { get; set; }
    }
    public class ThemeMatVideo : ThemeMatPart
    {
        public string Url { get; set; }
    }
    public class ThemeMatFile : ThemeMatPart
    {
        public string Url { get; set; }
    }
}