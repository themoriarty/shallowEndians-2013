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
            BitVecExpr arg0Output = ctx.MkBVConst(Name + "_bva0_", 64);
            if (Arg0 != null)
            {
                solver.Assert(ctx.MkEq(Arg0.Output, arg0Output));
            }

            BitVecExpr arg1Output = ctx.MkBVConst(Name + "_bva1_", 64);
            if (Arg1 != null)
            {
                solver.Assert(ctx.MkEq(Arg1.Output, arg1Output));
            }

            BitVecExpr arg2Output = ctx.MkBVConst(Name + "_bva2_", 64);
            if (Arg2 != null)
            {
                solver.Assert(ctx.MkEq(Arg2.Output, arg1Output));
            }

            if (permitted[(int)OpCodes.If0])
            {
                solver.Assert(ctx.MkOr(ctx.MkNot(ctx.MkEq(Arity, ctx.MkInt(3))), 
                        ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.If0)),
                        ctx.MkOr(
                            ctx.MkAnd(ctx.MkEq(arg0Output, ctx.MkBV(0, 64)), ctx.MkEq(Output, arg1Output)),
                            ctx.MkAnd(ctx.MkEq(ctx.MkBVUGT(arg0Output, ctx.MkBV(0, 64)), ctx.MkBool(true)), ctx.MkEq(Output, arg2Output))
                    ))));
            }
        }

        public override ArgNode[] GetChildren()
        {
            return new ArgNode[] { Arg0, Arg1, Arg2 };
        }
    }
}
