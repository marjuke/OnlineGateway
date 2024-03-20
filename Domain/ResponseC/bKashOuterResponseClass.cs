using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ResponseC
{
    public class bKashOuterResponseClass
    {
        public string ResponseCode { get; set; }
        public string ResponseDesc { get; set; }
        public Parameter[] Parameters { get; set; }
    }
}
