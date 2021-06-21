using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.ViewModels
{
    [NotMapped]
    public class IntView
    {
        public int Value { get; set; }
    }
}
