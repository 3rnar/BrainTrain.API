using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class KaznituUserInfo
    {
        public int id { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string position { get; set; }
        public bool hasPhoto { get; set; }
        public List<string> roles { get; set; }
        public double debtAmount { get; set; }
    }

    public class KanituUserFullInfo
    {
        public int? citizenCategory { get; set; }
        public int? citizenship { get; set; }
        public string surname { get; set; }
        public string name { get; set; }
        public string secondName { get; set; }
        public DateTime birthDate { get; set; }
        public string iin { get; set; }
        public object homePhone { get; set; }
        public string cellphone { get; set; }
        public int? messenger { get; set; }
        public int? documentType { get; set; }
        public int? documentIssueOrganization { get; set; }
        public object documentIssueOrganizationForeigner { get; set; }
        public string documentNumber { get; set; }
        public DateTime documentReceiveDate { get; set; }
        public string birthPlace { get; set; }
        public int? nationality { get; set; }
        public int? maritalStatus { get; set; }
        public string email { get; set; }
        public string personalEmail { get; set; }
        public bool? needInAccomodation { get; set; }
        public bool male { get; set; }
        public bool? hasPhoto { get; set; }
        public int? levelID { get; set; }
        public object lastUpdatedBy { get; set; }
        public int? entrantId { get; set; }
        public int id { get; set; }
    }
}