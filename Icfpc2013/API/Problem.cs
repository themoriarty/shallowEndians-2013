using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icfpc2013
{
    public class Problem
    {
        public string id { get; set; }
        double size { get; set; }
        public string[] operators { get; set; }
        bool? solved { get; set; }
        double? timeLeft { get; set; }

        public override string ToString()
        {
            return Utils.ToString(this);
        }
    }
}
