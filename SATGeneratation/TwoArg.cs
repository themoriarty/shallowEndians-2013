using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    public class TwoArg : ArgNode
    {
        public BoolExpr GenerateConstraints(Context ctx, Solver solver, BitVecExpr[] prInput, BitVecExpr[] prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            List<BoolExpr> andClauses = new List<BoolExpr>();
            var arg0Output = GenerateOutputVars(ctx, Output.Length, Name + "_bva0_");
            if (Arg0 != null)
            {
                andClauses.Add(EqAll(ctx, Arg0.Output, arg0Output));
            }
            else if (nodes != null && curNodeIndex != nodes.Count - 1)
            {
                andClauses.Add(EqAll(ctx, arg0Output, nodes[curNodeIndex + 1].Output));
            }

            var arg1Output = GenerateOutputVars(ctx, Output.Length, Name + "_bva1_");
            if (Arg1 != null)
            {
                andClauses.Add(EqAll(ctx, Arg1.Output, arg1Output));
            }
            else if (nodes != null && curNodeIndex != nodes.Count - 1 && tree != null)
            {
                bool foundArg = false;
                List<BoolExpr> orClauses = new List<BoolExpr>();
                for (int i = curNodeIndex + 2; i < nodes.Count; ++i)
                {
                    var selNode = ctx.MkSelect(tree.ReverseLink, ctx.MkInt(i));
                    var eqCheck = ctx.MkEq(selNode, ctx.MkInt(curNodeIndex));
                    //var selPin = ctx.MkSelect(tree.PinLink, ctx.MkInt(i));
                    //var pinCheck = ctx.MkEq(selPin, ctx.MkInt(1));

                    orClauses.Add(ctx.MkAnd(eqCheck, EqAll(ctx, arg1Output, nodes[i].Output)));
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

            var andCond = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(2)), ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.And)), EqAll(ctx, Output, AndAll(ctx, arg0Output, arg1Output)));
            var orCond = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(2)), ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Or)), EqAll(ctx, Output, OrAll(ctx, arg0Output, arg1Output)));
            var xorCond = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(2)), ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Xor)), EqAll(ctx, Output, XorAll(ctx, arg0Output, arg1Output)));
            var plusCond = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(2)), ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.Plus)), EqAll(ctx, Output, AddAll(ctx, arg0Output, arg1Output)));
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

        public override void AddConstraints(Context ctx, Solver solver, BitVecExpr[] prInput, BitVecExpr[] prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
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
