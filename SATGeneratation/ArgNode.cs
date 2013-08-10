using Icfpc2013.Ops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Z3;

namespace SATGeneratation
{
    public abstract class ArgNode
    {
        public abstract void AddConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prOutput, bool[] permitted);
        public abstract ArgNode[] GetChildren();

        public virtual void Initialize(Context ctx, string name)
        {
            Output = ctx.MkBVConst(name + "_o", 64);
            OpCode = ctx.MkIntConst(name + "_t");
            Name = name;
            Arity = ctx.MkIntConst(name + "_a");
        }

        public string Name { get; set; }
        public BitVecExpr Output { get; set; }
        public IntExpr OpCode { get; set; }
        public IntExpr Arity;
        public OpCodes ComputedOpcode { get; set; }
    }
}
