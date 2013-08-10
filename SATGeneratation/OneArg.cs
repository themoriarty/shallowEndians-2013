using Microsoft.Z3;
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

        public override void AddConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prOutput, bool[] permitted)
        {
            BitVecExpr arg0Output = ctx.MkBVConst(Name + "_bva0_", 64);
            if (Arg0 != null)
            {
                solver.Assert(ctx.MkEq(Arg0.Output, arg0Output));
            }

            var notCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Not)), ctx.MkEq(Output, ctx.MkBVNot(arg0Output)));
            var shl1Cond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Shl1)), ctx.MkEq(Output, ctx.MkBVSHL(arg0Output, ctx.MkBV(1, 64))));
            var shr1Cond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Shr1)), ctx.MkEq(Output, ctx.MkBVLSHR(arg0Output, ctx.MkBV(1, 64))));
            var shr4Cond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Shr4)), ctx.MkEq(Output, ctx.MkBVLSHR(arg0Output, ctx.MkBV(4, 64))));
            var shr16Cond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Shr16)), ctx.MkEq(Output, ctx.MkBVLSHR(arg0Output, ctx.MkBV(16, 64))));

            List<BoolExpr> expressions = new List<BoolExpr>();
            if (permitted[(int)OpCodes.Not])
            {
                expressions.Add(notCond);
            }
            if (permitted[(int)OpCodes.Shl1])
            {
                expressions.Add(shl1Cond);
            }
            if (permitted[(int)OpCodes.Shr1])
            {
                expressions.Add(shr1Cond);
            }
            if (permitted[(int)OpCodes.Shr4])
            {
                expressions.Add(shr4Cond);
            }
            if (permitted[(int)OpCodes.Shr16])
            {
                expressions.Add(shr16Cond);
            }
            solver.Assert(ctx.MkOr(ctx.MkNot(ctx.MkEq(Arity, ctx.MkInt(1))), ctx.MkOr(expressions.ToArray())));
        }

        public override ArgNode[] GetChildren()
        {
            return new ArgNode[] { Arg0 };
        }
    }
}
