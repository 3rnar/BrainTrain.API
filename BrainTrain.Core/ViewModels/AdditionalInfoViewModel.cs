using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class AdditionalInfoViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GradeId { get; set; }
        public AdditionalInfoSubject[] SubjectIds { get; set; }
        public int DesiredScore { get; set; }
    }

    public class AdditionalInfoSubject
    {
        public int SubjectId { get; set; }        
    }
}