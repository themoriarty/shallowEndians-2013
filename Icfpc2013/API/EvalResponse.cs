using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icfpc2013
{
    public class EvalResponse
    {
        public string status { get; set; }
        public string[] outputs { get; set; }
        public string message { get; set; }

        public override string ToString()
        {
            return Utils.ToString(this);
        }
    }
}
