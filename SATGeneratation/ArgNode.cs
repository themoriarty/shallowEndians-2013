namespace SATGeneratation
{
    using System.Collections.Generic;

    using Microsoft.Z3;

    public abstract class ArgNode
    {
        #region Fields

        public IntExpr Arity;

        #endregion

        #region Public Properties

        public OpCodes ComputedOpcode { get; set; }

        public string Name { get; set; }

        public IntExpr OpCode { get; set; }

        public BitVecExpr Output { get; set; }

        #endregion

        #region Public Methods and Operators

        public abstract void AddConstraints(Context ctx, Solver solver, BitVecExpr prInput, BitVecExpr prInput2, BitVecExpr prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree);

        public abstract ArgNode[] GetChildren();

        public virtual void Initialize(Context ctx, string name)
        {
            Output = ctx.MkBVConst(name + "_o", 64);
            OpCode = ctx.MkIntConst(name + "_t");
            Name = name;
            Arity = ctx.MkIntConst(name + "_a");
        }

        #endregion
    }
}