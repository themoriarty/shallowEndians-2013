using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    class MetaArgNode : ArgNode
    {

        public override void Initialize(Context ctx, string name)
        {
            base.Initialize(ctx, name);
            ZeroArgNode = new ZeroArg();
            ZeroArgNode.Initialize(ctx, name + "_0a_");
            OneArgNode = new OneArg();
            OneArgNode.Initialize(ctx, name + "_1a_");
            TwoArgNode = new TwoArg();
            TwoArgNode.Initialize(ctx, name + "_2a_");
            ThreeArgNode = new ThreeArg();
            ThreeArgNode.Initialize(ctx, name + "_3a_");
        }

        public override void AddConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prOutput, bool[] permitted)
        {
            var zeroConst = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(0)), ctx.MkEq(Output, ZeroArgNode.Output));
            var oneConst = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(1)), ctx.MkEq(Output, OneArgNode.Output));
            var twoConst = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(2)), ctx.MkEq(Output, TwoArgNode.Output));
            var threeConst = ctx.MkAnd(ctx.MkEq(Arity, ctx.MkInt(3)), ctx.MkEq(Output, ThreeArgNode.Output));
            solver.Assert(ctx.MkOr(zeroConst, oneConst, twoConst, threeConst));

            ZeroArgNode.AddConstraints(ctx, solver, prInput, prOutput, permitted);
            OneArgNode.AddConstraints(ctx, solver, prInput, prOutput, permitted);
            TwoArgNode.AddConstraints(ctx, solver, prInput, prOutput, permitted);
            ThreeArgNode.AddConstraints(ctx, solver, prInput, prOutput, permitted);
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
