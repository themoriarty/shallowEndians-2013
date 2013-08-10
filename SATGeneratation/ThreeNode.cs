using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    class ThreeArg : ArgNode
    {
        public ArgNode Arg0;
        public ArgNode Arg1;
        public ArgNode Arg2;

        public override void AddConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prOutput, bool[] permitted)
        {
            if (permitted[(int)OpCodes.If0])
            {
                solver.Assert(
                        ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.If0)),
                        ctx.MkOr(
                            ctx.MkAnd(ctx.MkEq(Arg0.Output, ctx.MkBV(0, 64)), ctx.MkEq(Output, Arg1.Output)),
                            ctx.MkAnd(ctx.MkEq(ctx.MkBVUGT(Arg0.Output, ctx.MkBV(0, 64)), ctx.MkBool(true)), ctx.MkEq(Output, Arg2.Output))
                    )));
            }
        }

        public override ArgNode[] GetChildren()
        {
            return new ArgNode[] { Arg0, Arg1, Arg2 };
        }
    }
}
