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
            var notCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Not)), ctx.MkEq(Output, ctx.MkBVNot(Arg0.Output)));
            var shl1Cond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Shl1)), ctx.MkEq(Output, ctx.MkBVSHL(Arg0.Output, ctx.MkBV(1, 64))));
            var shr1Cond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Shr1)), ctx.MkEq(Output, ctx.MkBVLSHR(Arg0.Output, ctx.MkBV(1, 64))));
            var shr4Cond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Shr4)), ctx.MkEq(Output, ctx.MkBVLSHR(Arg0.Output, ctx.MkBV(4, 64))));
            var shr16Cond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Shr16)), ctx.MkEq(Output, ctx.MkBVLSHR(Arg0.Output, ctx.MkBV(16, 64))));

            List<BoolExpr> expressions = new List<BoolExpr>(); ;
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
            solver.Assert(ctx.MkOr(expressions.ToArray()));
        }
    }
}
