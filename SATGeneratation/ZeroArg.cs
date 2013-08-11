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
        public BoolExpr GenerateConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prInput2, BitVecExpr prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            var oneCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.One)), ctx.MkEq(Output, ctx.MkBV(1, 64)));
            var zeroCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Zero)), ctx.MkEq(Output, ctx.MkBV(0, 64)));
            var inputCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Input)), ctx.MkEq(Output, prInput));
            List<BoolExpr> expressions = new List<BoolExpr>();
            if(prInput2 != null)
            {
                var input2Cond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Input2)), ctx.MkEq(Output, prInput2));
                if (!permitted[(int)OpCodes.Input2])
                {
                    throw new ArgumentException("Tfold and Input2 must be enabled at the same time"); 
                }
                else
                {
                    expressions.Add(input2Cond);
                }
            }
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

        public override void AddConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prInput2, BitVecExpr prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            solver.Assert(GenerateConstraints(ctx, solver, prInput, prInput2, prOutput, permitted, nodes, curNodeIndex, tree));
        }

        public override ArgNode[] GetChildren()
        {
            return new ArgNode[] {};
        }
    }
}
