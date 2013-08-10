using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    class TwoArg : ArgNode
    {

        public override void AddConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prOutput, bool[] permitted)
        {
            var andCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.And)), ctx.MkEq(Output, ctx.MkBVAND(Arg0.Output, Arg1.Output)));
            var orCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Or)), ctx.MkEq(Output, ctx.MkBVOR(Arg0.Output, Arg1.Output)));
            var xorCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Xor)),  ctx.MkEq(Output, ctx.MkBVXOR(Arg0.Output, Arg1.Output)));
            var plusCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Plus)),  ctx.MkEq(Output, ctx.MkBVAdd(Arg0.Output, Arg1.Output)));
            List<BoolExpr> expressions = new List<BoolExpr>(); ;
            if (permitted[(int)OpCodes.And])
            {
                expressions.Add(andCond);
            }
            if (permitted[(int)OpCodes.Or])
            {
                expressions.Add(orCond);
            }
            if (permitted[(int)OpCodes.Xor])
            {
                expressions.Add(xorCond);
            }
            if (permitted[(int)OpCodes.Plus])
            {
                expressions.Add(plusCond);
            }

            solver.Assert(ctx.MkOr(expressions.ToArray()));
        }

        public override ArgNode[] GetChildren()
        {
            return new ArgNode[] { Arg0, Arg1 };
        }


        public ArgNode Arg0;
        public ArgNode Arg1;
    }
}
