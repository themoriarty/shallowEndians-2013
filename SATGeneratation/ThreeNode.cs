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

        public BoolExpr GenerateConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            List<BoolExpr> andClauses = new List<BoolExpr>();
            BitVecExpr arg0Output = ctx.MkBVConst(Name + "_bva0_", 64);
            if (Arg0 != null)
            {
                andClauses.Add(ctx.MkEq(Arg0.Output, arg0Output));
            }

            BitVecExpr arg1Output = ctx.MkBVConst(Name + "_bva1_", 64);
            if (Arg1 != null)
            {
                andClauses.Add(ctx.MkEq(Arg1.Output, arg1Output));
            }

            BitVecExpr arg2Output = ctx.MkBVConst(Name + "_bva2_", 64);
            if (Arg2 != null)
            {
                andClauses.Add(ctx.MkEq(Arg2.Output, arg2Output));
            }

            if (permitted[(int)OpCodes.If0])
            {
                andClauses.Add(
                        ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.If0)),
                        ctx.MkOr(
                            ctx.MkAnd(ctx.MkEq(arg0Output, ctx.MkBV(0, 64)), ctx.MkEq(Output, arg1Output)),
                            ctx.MkAnd(ctx.MkEq(ctx.MkBVUGT(arg0Output, ctx.MkBV(0, 64)), ctx.MkBool(true)), ctx.MkEq(Output, arg2Output))
                    )));
            }
            if (andClauses.Count == 0)
            {
                andClauses.Add(ctx.MkFalse());
            }

            return ctx.MkAnd(andClauses.ToArray());
        }

        public override void AddConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            solver.Assert(GenerateConstraints(ctx, solver, prInput, prOutput, permitted, nodes, curNodeIndex, tree));
        }

        public override ArgNode[] GetChildren()
        {
            return new ArgNode[] { Arg0, Arg1, Arg2 };
        }
    }
}
