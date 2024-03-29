﻿using Microsoft.Z3;
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

        public BoolExpr GenerateConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prInput2, BitVecExpr prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            List<BoolExpr> andExpressions = new List<BoolExpr>();
            BitVecExpr arg0Output = ctx.MkBVConst(Name + "_bva0_", 64);
            if (Arg0 != null)
            {
                andExpressions.Add(ctx.MkEq(Arg0.Output, arg0Output));
            }
            else if (nodes != null && curNodeIndex != nodes.Count - 1)
            {
                andExpressions.Add(ctx.MkEq(arg0Output, nodes[curNodeIndex + 1].Output));
            }
            else
            {
                andExpressions.Add(ctx.MkFalse());
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

            if (expressions.Count == 0)
            {
                andExpressions.Add(ctx.MkFalse());
            }
            andExpressions.Add(ctx.MkOr(expressions.ToArray()));
            return ctx.MkAnd(andExpressions.ToArray());
        }

        public override void AddConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prInput2, BitVecExpr prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            solver.Assert(GenerateConstraints(ctx, solver, prInput, prInput2, prOutput, permitted, nodes, curNodeIndex, tree));
        }

        public override ArgNode[] GetChildren()
        {
            return new ArgNode[] { Arg0 };
        }
    }
}
