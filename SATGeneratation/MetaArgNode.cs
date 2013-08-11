using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    public class MetaArgNode : ArgNode
    {

        public override void Initialize(Context ctx, string name, int numberOfOutputs)
        {
            base.Initialize(ctx, name, numberOfOutputs);
            ZeroArgNode = new ZeroArg();
            ZeroArgNode.Initialize(ctx, name + "_0a_", numberOfOutputs);
            OneArgNode = new OneArg();
            OneArgNode.Initialize(ctx, name + "_1a_", numberOfOutputs);
            TwoArgNode = new TwoArg();
            TwoArgNode.Initialize(ctx, name + "_2a_", numberOfOutputs);
            ThreeArgNode = new ThreeArg();
            ThreeArgNode.Initialize(ctx, name + "_3a_", numberOfOutputs);
        }

        public override void AddConstraints(Context ctx, Solver solver, BitVecExpr[] prInput, BitVecExpr[] prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            if(tree != null)
            {
                var sel = ctx.MkSelect(tree.ArgumentCount, ctx.MkInt(curNodeIndex));
                solver.Assert(ctx.MkEq(Arity, sel));
            }
            var zeroConst = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(0)), EqAll(ctx, Output, ZeroArgNode.Output), ctx.MkEq(OpCode, ZeroArgNode.OpCode),
                ZeroArgNode.GenerateConstraints(ctx, solver, prInput, prOutput, permitted, nodes, curNodeIndex, tree));
            var oneConst = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(1)), EqAll(ctx, Output, OneArgNode.Output), ctx.MkEq(OpCode, OneArgNode.OpCode),
                OneArgNode.GenerateConstraints(ctx, solver, prInput, prOutput, permitted, nodes, curNodeIndex, tree));
            var twoConst = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(2)), EqAll(ctx, Output, TwoArgNode.Output), ctx.MkEq(OpCode, TwoArgNode.OpCode),
                TwoArgNode.GenerateConstraints(ctx, solver, prInput, prOutput, permitted, nodes, curNodeIndex, tree));
            var threeConst = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(3)), EqAll(ctx, Output, ThreeArgNode.Output), ctx.MkEq(OpCode, ThreeArgNode.OpCode),
                ThreeArgNode.GenerateConstraints(ctx, solver, prInput, prOutput, permitted, nodes, curNodeIndex, tree));
            solver.Assert(ctx.MkOr(zeroConst, oneConst, twoConst, threeConst));
        }

        public override ArgNode[] GetChildren()
        {
            return new ArgNode[] {};
        }

        public ZeroArg ZeroArgNode;
        public OneArg OneArgNode;
        public TwoArg TwoArgNode;
        public ThreeArg ThreeArgNode;
    }
}
