using BrainTrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainTrain.Core.ViewModels
{
    public class MaterialsPagingViewModel
    {
        public IEnumerable<Material> Materials { get; set; }
        public int MaterialsCount { get; set; }
    }
}