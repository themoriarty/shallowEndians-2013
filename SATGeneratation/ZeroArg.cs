using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    class ZeroArg : ArgNode
    {
        public override void AddConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prOutput, bool[] permitted)
        {
            var oneCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.One)), ctx.MkEq(Output, ctx.MkBV(1, 64)));
            var zeroCond = ctx.MkOr(ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Zero)), ctx.MkEq(Output, ctx.MkBV(0, 64))));
            var inputCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Input)), ctx.MkEq(Output, prInput));
            List<BoolExpr> expressions = new List<BoolExpr>();;
            if(permitted[(int)OpCodes.One])
            {
                expressions.Add(oneCond);
            }
            if(permitted[(int)OpCodes.Zero])
            {
                expressions.Add(zeroCond);
            }
            if(permitted[(int)OpCodes.Input])
            {
                expressions.Add(inputCond);
            }
            solver.Assert(ctx.MkOr(expressions.ToArray()));
        }

        public override ArgNode[] GetChildren()
        {
            return new ArgNode[] {};
        }
    }
}
