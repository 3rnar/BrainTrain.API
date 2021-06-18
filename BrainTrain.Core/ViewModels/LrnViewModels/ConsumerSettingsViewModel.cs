using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels.LrnViewModels
{
    public class ConsumerSettingsViewModel
    {
        public string action { get; set; }
        public ConsumerSettingsSecurity security { get; set; }
        public ConsumerSettingsRequest request { get; set; }
    }

    public class ConsumerSettingsSecurity
    {
        public string domain { get; set; }
        public string consumer_key { get; set; }
        public string timestamp { get; set; }
        public string signature { get; set; }
    }

    public class ConsumerSettingsRequest
    {
        public string mode { get; set; }
        public string titleShow { get; set; }
        public ConsumerSettingsUser user { get; set; }
    }

    public class ConsumerSettingsUser
    {
        public string id { get; set; }
    }



}