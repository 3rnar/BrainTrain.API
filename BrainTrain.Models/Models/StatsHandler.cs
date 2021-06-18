using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public static class StatsHandler
    {
        public static List<UserTimeStore> ConnectedIds = new List<UserTimeStore>();
        public static List<UserTimeStore> ExamConnectedIds = new List<UserTimeStore>();
    }

    public class UserTimeStore
    {
        public string UserName { get; set; }
        public DateTime StartTime { get; set; }
    }
}