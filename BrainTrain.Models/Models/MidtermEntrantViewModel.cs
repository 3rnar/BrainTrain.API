using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class MidtermEntrantViewModel
    {
        public int StudentID { get; set; }
        public DateTime? EntryDate { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string IIN { get; set; }
    }
}