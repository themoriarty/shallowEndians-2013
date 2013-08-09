using Icfpc2013.Ops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    public abstract class ArgNode
    {
        public Node ExactNode;
        public string Name { get; set; }
        public abstract SatExpression ConvertToSat();
    }
}
