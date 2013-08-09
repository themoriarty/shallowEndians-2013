using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icfpc2013
{
    public class RequestWindow
    {
        public double resetsIn { get; set; }
        public double amount { get; set; }
        public double limit { get; set; }

        public override string ToString()
        {
            return Utils.ToString(this);
        }
    }
}
