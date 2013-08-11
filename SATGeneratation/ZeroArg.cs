using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    public class ZeroArg : ArgNode
    {
        public BoolExpr GenerateConstraints(Context ctx, Solver solver, BitVecExpr[] prInput, BitVecExpr[] prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            var oneCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.One)), EqAll(ctx, Output, ctx.MkBV(1, 64)));
            var zeroCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Zero)), EqAll(ctx, Output, ctx.MkBV(0, 64)));
            var inputCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Input)), EqAll(ctx, Output, prInput));
            List<BoolExpr> expressions = new List<BoolExpr>();
            if (permitted[(int)OpCodes.One])
            {
                expressions.Add(oneCond);
            }
            if (permitted[(int)OpCodes.Zero])
            {
                expressions.Add(zeroCond);
            }
            if (permitted[(int)OpCodes.Input])
            {
                expressions.Add(inputCond);
            }
            return ctx.MkOr(expressions.ToArray());
        }

        public override void AddConstraints(Context ctx, Solver solver, BitVecExpr[] prInput, BitVecExpr[] prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            solver.Assert(GenerateConstraints(ctx, solver, prInput, prOutput, permitted, nodes, curNodeIndex, tree));
        }

        public override ArgNode[] GetChildren()
        {
            return new ArgNode[] {};
        }
    }
}
