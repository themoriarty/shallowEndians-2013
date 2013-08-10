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
        public BoolExpr GenerateConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            List<BoolExpr> andClauses = new List<BoolExpr>();
            BitVecExpr arg0Output = ctx.MkBVConst(Name + "_bva0_", 64);
            if (Arg0 != null)
            {
                andClauses.Add(ctx.MkEq(Arg0.Output, arg0Output));
            }
            else if (nodes != null && curNodeIndex != nodes.Count - 1)
            {
                andClauses.Add(ctx.MkEq(arg0Output, nodes[curNodeIndex + 1].Output));
            }

            BitVecExpr arg1Output = ctx.MkBVConst(Name + "_bva1_", 64);
            if (Arg1 != null)
            {
                andClauses.Add(ctx.MkEq(Arg1.Output, arg1Output));
            }
            else if (nodes != null && curNodeIndex != nodes.Count - 1 && tree != null)
            {
                bool foundArg = false;
                List<BoolExpr> orClauses = new List<BoolExpr>();
                for (int i = curNodeIndex + 2; i < nodes.Count; ++i)
                {
                    var selNode = ctx.MkSelect(tree.ReverseLink, ctx.MkInt(i));
                    var eqCheck = ctx.MkEq(selNode, ctx.MkInt(i));
                    orClauses.Add(ctx.MkAnd(eqCheck, ctx.MkEq(arg0Output, nodes[i].Output)));
                    foundArg = true;
                }
                if (!foundArg)
                {
                    andClauses.Add(ctx.MkFalse());
                }
                else
                {
                    andClauses.Add(ctx.MkOr(orClauses.ToArray()));
                }
            }
            else
            {
                andClauses.Add(ctx.MkFalse());
            }

            var andCond = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(2)), ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.And)), ctx.MkEq(Output, ctx.MkBVAND(arg0Output, arg1Output)));
            var orCond = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(2)), ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Or)), ctx.MkEq(Output, ctx.MkBVOR(arg0Output, arg1Output)));
            var xorCond = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(2)), ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Xor)), ctx.MkEq(Output, ctx.MkBVXOR(arg0Output, arg1Output)));
            var plusCond = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(2)), ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Plus)), ctx.MkEq(Output, ctx.MkBVAdd(arg0Output, arg1Output)));
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
            if (expressions.Count == 0)
            {
                andClauses.Add(ctx.MkFalse());
            }
            andClauses.Add(ctx.MkOr(expressions.ToArray()));
            return ctx.MkAnd(andClauses.ToArray());
        }

        public override void AddConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            solver.Assert(GenerateConstraints(ctx, solver, prInput, prOutput, permitted, nodes, curNodeIndex, tree));
        }

        public override ArgNode[] GetChildren()
        {
            return new ArgNode[] { Arg0, Arg1 };
        }


        public ArgNode Arg0;
        public ArgNode Arg1;
    }
}
