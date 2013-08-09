using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icfpc2013
{
    public class TrainingProblem
    {
        public string challenge { get; set; }
        public string id { get; set; }
        public double size { get; set; }
        public string[] operators { get; set; }

        public override string ToString()
        {
            return Utils.ToString(this);
        }
    }
}
