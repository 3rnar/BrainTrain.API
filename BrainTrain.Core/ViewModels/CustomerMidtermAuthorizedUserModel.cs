using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class CustomerMidtermAuthorizedUserModel
    {
        public int Id { get; set; }
        public double? SecondsSpent { get; set; }
        public int? StatusId { get; set; }
        public string FullName { get; set; }
        public bool? IsPrinted { get; set; }
    }
}