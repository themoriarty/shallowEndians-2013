using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icfpc2013
{
    public class TrainRequest
    {
        public double? size { get; set; }
        public string[] operators { get; set; }

        public TrainRequest(double? size, string[] operators)
        {
            this.size = size;
            this.operators = operators;
        }

        public override string ToString()
        {
            return Utils.ToString(this);
        }
    }
}
