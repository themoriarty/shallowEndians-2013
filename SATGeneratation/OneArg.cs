using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    public class OneArg : ArgNode
    {
        public ArgNode Arg0;

        public BoolExpr GenerateOutputConstraints(Context ctx, Solver solver, List<ArgNode> nodes, int curNodeIndex)
        {
            List<BoolExpr> andExpressions = new List<BoolExpr>();
            if (nodes != null)
            {
            }


            return ctx.MkAnd(andExpressions.ToArray());
        }

        public BoolExpr GenerateConstraints(Context ctx, Solver solver, BitVecExpr[] prInput, BitVecExpr[] prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            List<BoolExpr> andExpressions = new List<BoolExpr>();
            var arg0Output = GenerateOutputVars(ctx, Output.Length, Name + "_bva0_");
            if (Arg0 != null)
            {
                andExpressions.Add(EqAll(ctx, Arg0.Output, arg0Output));
            }
            else if (nodes != null && curNodeIndex != nodes.Count - 1)
            {
                andExpressions.Add(EqAll(ctx, arg0Output, nodes[curNodeIndex + 1].Output));
            }
            else
            {
                andExpressions.Add(ctx.MkFalse());
            }

            var notCond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Not)), EqAll(ctx, Output, NotAll(ctx, arg0Output)));
            var shl1Cond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Shl1)), EqAll(ctx, Output, ShlAll(ctx, arg0Output, ctx.MkBV(1, 64))));
            var shr1Cond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Shr1)), EqAll(ctx, Output, ShrAll(ctx, arg0Output, ctx.MkBV(1, 64))));
            var shr4Cond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Shr4)), EqAll(ctx, Output, ShrAll(ctx, arg0Output, ctx.MkBV(4, 64))));
            var shr16Cond = ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Shr16)), EqAll(ctx, Output, ShrAll(ctx, arg0Output, ctx.MkBV(16, 64))));

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

            if (expressions.Count == 0)
            {
                andExpressions.Add(ctx.MkFalse());
            }
            andExpressions.Add(ctx.MkOr(expressions.ToArray()));
            return ctx.MkAnd(andExpressions.ToArray());
        }

        public override void AddConstraints(Context ctx, Solver solver, BitVecExpr[] prInput, BitVecExpr[] prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            solver.Assert(GenerateConstraints(ctx, solver, prInput, prOutput, permitted, nodes, curNodeIndex, tree));
        }

        public override ArgNode[] GetChildren()
        {
            return new ArgNode[] { Arg0 };
        }
    }
}
