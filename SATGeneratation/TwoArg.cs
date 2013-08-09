using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    class TwoArg : ArgNode
    {
        public ArgNode Arg0;
        public ArgNode Arg1;

        public override SatExpression ConvertToSat()
        {
            return new SatExpression();
        }
    }
}
