using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    public class SatExpression
    {
    }

    public class PosLeafExpresssion : SatExpression
    {
        public string VarName;
        public override string ToString()
        {
            return VarName;
        }
    }

    public class NegLeafExpresssion : SatExpression
    {
        public string VarName;
        public override string ToString()
        {
            return "~" + VarName;
        }
    }

    public class NonLeafExpression : SatExpression
    {
        public SatExpression Exp1;
        public SatExpression Exp2;
    }

    public class OrExpression : NonLeafExpression
    {
        public override string ToString()
        {
            return "(" + Exp1.ToString() + " OR " + Exp2.ToString() + ")";
        }
    }

    public class AndExpression : NonLeafExpression
    {
        public override string ToString()
        {
            return "(" + Exp1.ToString() + " AND " + Exp2.ToString() + ")";
        }
    }
}
