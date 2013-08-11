using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    public class ThreeArg : ArgNode
    {
        public ArgNode Arg0;
        public ArgNode Arg1;
        public ArgNode Arg2;

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
                    var selPin = ctx.MkSelect(tree.PinLink, ctx.MkInt(i));
                    var pinCheck = ctx.MkEq(selPin, ctx.MkInt(1));

                    orClauses.Add(ctx.MkAnd(pinCheck, eqCheck, EqAll(ctx, arg1Output, nodes[i].Output)));
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

            var arg2Output = GenerateOutputVars(ctx, Output.Length, Name + "_bva2_");
            if (Arg2 != null)
            {
                andClauses.Add(EqAll(ctx, Arg2.Output, arg2Output));
            }
            else if (nodes != null && curNodeIndex < nodes.Count - 2 && tree != null)
            {
                bool foundArg = false;
                List<BoolExpr> orClauses = new List<BoolExpr>();
                for (int i = curNodeIndex + 3; i < nodes.Count; ++i)
                {
                    var selNode = ctx.MkSelect(tree.ReverseLink, ctx.MkInt(i));
                    var eqCheck = ctx.MkEq(selNode, ctx.MkInt(curNodeIndex));
                    var selPin = ctx.MkSelect(tree.PinLink, ctx.MkInt(i));
                    var pinCheck = ctx.MkEq(selPin, ctx.MkInt(2));

                    orClauses.Add(ctx.MkAnd(pinCheck, eqCheck, EqAll(ctx, arg2Output, nodes[i].Output)));
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


            if (permitted[(int)OpCodes.If0])
            {
                andClauses.Add(
                        ctx.MkAnd(ctx.MkEq(OpCode, ctx.MkInt((int)OpCodes.If0)),
                        ctx.MkOr(
                            ctx.MkAnd(EqAll(ctx, arg0Output, ctx.MkBV(0, 64)), EqAll(ctx, Output, arg1Output)),
                            ctx.MkAnd(ctx.MkEq(UgtAll(ctx, arg0Output, ctx.MkBV(0, 64)), ctx.MkBool(true)), EqAll(ctx, Output, arg2Output))
                    )));
            }
            else
            {
                andClauses.Add(ctx.MkFalse());
            }

            return ctx.MkAnd(andClauses.ToArray());
        }

        public override void AddConstraints(Context ctx, Solver solver, BitVecExpr[] prInput, BitVecExpr[] prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            solver.Assert(GenerateConstraints(ctx, solver, prInput, prOutput, permitted, nodes, curNodeIndex, tree));
        }

        public override ArgNode[] GetChildren()
        {
            return new ArgNode[] { Arg0, Arg1, Arg2 };
        }
    }
}
