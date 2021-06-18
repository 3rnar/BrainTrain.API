using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class CustomerMidtermUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IIN { get; set; }
        public double? Maths { get; set; }
        public double? Physics { get; set; }
        public int? NumberOfComplaints { get; set; }
        public int? LanguageId { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public bool? IsPrinted { get; set; }
    }
}