using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Dapper
{
    public class FunctionParameter
    {
        public FunctionParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public object Value { get; set; }
    }
}
