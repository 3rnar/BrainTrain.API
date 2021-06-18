using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Models.Models
{
    public class MaterialsPagingViewModel
    {
        public IEnumerable<Material> Materials { get; set; }
        public int MaterialsCount { get; set; }
    }
}