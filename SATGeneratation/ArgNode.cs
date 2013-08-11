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

        public BitVecExpr[] Output { get; set; }

        #endregion

        #region Public Methods and Operators

        public abstract void AddConstraints(Context ctx, Solver solver, BitVecExpr[] prInput, BitVecExpr[] prOutput, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree);

        public abstract ArgNode[] GetChildren();

        protected BitVecExpr[] GenerateOutputVars(Context ctx, int numberOfOutputs, string prefix)
        {
            var ret = new BitVecExpr[numberOfOutputs];
            for (int i = 0; i < numberOfOutputs; ++i )
            {
                ret[i] = ctx.MkBVConst(prefix + i, 64);
            }
            return ret;
        }
        public static BoolExpr EqAll(Context ctx, BitVecExpr[] i1, BitVecExpr[] i2)
        {
            List<BoolExpr> allexps = new List<BoolExpr>();
            for (int i = 0; i < i1.Length; ++i)
            {
                allexps.Add(ctx.MkEq(i1[i], i2[i]));
            }
            return ctx.MkAnd(allexps.ToArray());
        }

        protected BoolExpr EqAll(Context ctx, BitVecExpr[] i1, BitVecExpr i2)
        {
            List<BoolExpr> allexps = new List<BoolExpr>();
            for (int i = 0; i < i1.Length; ++i)
            {
                allexps.Add(ctx.MkEq(i1[i], i2));
            }
            return ctx.MkAnd(allexps.ToArray());
        }

        protected BoolExpr UgtAll(Context ctx, BitVecExpr[] i1, BitVecExpr i2)
        {
            List<BoolExpr> allexps = new List<BoolExpr>();
            for (int i = 0; i < i1.Length; ++i)
            {
                allexps.Add(ctx.MkBVUGT(i1[i], i2));
            }
            return ctx.MkAnd(allexps.ToArray());
        }

        protected BitVecExpr[] NotAll(Context ctx, BitVecExpr[] i1)
        {
            List<BitVecExpr> ret = new List<BitVecExpr>();
            for (int i = 0; i < i1.Length; ++i)
            {
                ret.Add(ctx.MkBVNot(i1[i]));
            }
            return ret.ToArray();
        }
        protected BitVecExpr[] ShlAll(Context ctx, BitVecExpr[] i1, BitVecExpr i2)
        {
            List<BitVecExpr> ret = new List<BitVecExpr>();
            for (int i = 0; i < i1.Length; ++i)
            {
                ret.Add(ctx.MkBVSHL(i1[i], i2));
            }
            return ret.ToArray();
        }
        protected BitVecExpr[] ShrAll(Context ctx, BitVecExpr[] i1, BitVecExpr i2)
        {
            List<BitVecExpr> ret = new List<BitVecExpr>();
            for (int i = 0; i < i1.Length; ++i)
            {
                ret.Add(ctx.MkBVLSHR(i1[i], i2));
            }
            return ret.ToArray();
        }
        protected BitVecExpr[] AndAll(Context ctx, BitVecExpr[] i1, BitVecExpr[] i2)
        {
            List<BitVecExpr> ret = new List<BitVecExpr>();
            for (int i = 0; i < i1.Length; ++i)
            {
                ret.Add(ctx.MkBVAND(i1[i], i2[i]));
            }
            return ret.ToArray();
        }

        protected BitVecExpr[] OrAll(Context ctx, BitVecExpr[] i1, BitVecExpr[] i2)
        {
            List<BitVecExpr> ret = new List<BitVecExpr>();
            for (int i = 0; i < i1.Length; ++i)
            {
                ret.Add(ctx.MkBVOR(i1[i], i2[i]));
            }
            return ret.ToArray();
        }
        protected BitVecExpr[] XorAll(Context ctx, BitVecExpr[] i1, BitVecExpr[] i2)
        {
            List<BitVecExpr> ret = new List<BitVecExpr>();
            for (int i = 0; i < i1.Length; ++i)
            {
                ret.Add(ctx.MkBVXOR(i1[i], i2[i]));
            }
            return ret.ToArray();
        }
        protected BitVecExpr[] AddAll(Context ctx, BitVecExpr[] i1, BitVecExpr[] i2)
        {
            List<BitVecExpr> ret = new List<BitVecExpr>();
            for (int i = 0; i < i1.Length; ++i)
            {
                ret.Add(ctx.MkBVAdd(i1[i], i2[i]));
            }
            return ret.ToArray();
        }


        public virtual void Initialize(Context ctx, string name, int numberOfOutputs)
        {
            Output = GenerateOutputVars(ctx, numberOfOutputs, name + "o");
            OpCode = ctx.MkIntConst(name + "_t");
            Name = name;
            Arity = ctx.MkIntConst(name + "_a");
        }

        #endregion
    }
}