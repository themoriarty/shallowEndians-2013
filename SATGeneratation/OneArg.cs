using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    class OneArg : ArgNode
    {
        public ArgNode Arg0;

        public override SatExpression ConvertToSat()
        {
            AndExpression OneExp = new AndExpression
            {
                Exp1 = Utils.GetBitsForOpCode(Name + "_T", 0),
                Exp2 = Utils.GetBitsFromNumber(Name + "_V", 1, 0)
            };

            AndExpression ZeroExp = new AndExpression
            {
                Exp1 = Utils.GetBitsForOpCode(Name + "_T", 0),
                Exp2 = Utils.GetBitsFromNumber(Name + "_V", 0, 0)
            };

            AndExpression IdExp = new AndExpression
            {
                Exp1 = Utils.GetBitsForOpCode(Name + "_T", 0),
                Exp2 = Utils.GetBitsFromNumber(Name + "_I", 0, 0)
            };

            return new OrExpression()
            {
                Exp1 = OneExp,
                Exp2 = new OrExpression()
                {
                    Exp1 = ZeroExp,
                    Exp2 = IdExp
                }
            };
        }
    }
}
