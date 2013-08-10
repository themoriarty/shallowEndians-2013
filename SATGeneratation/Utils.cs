using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    using Microsoft.Z3;

    class Utils
    {
        public static ArgNode CloneTreeStructure(ArgNode root, Context ctx, string nameSuffix)
        {
            if (root is ZeroArg)
            {
                var ret = new ZeroArg();
                ret.Initialize(ctx, root.Name + nameSuffix);
                return ret;
            }
            else if (root is OneArg)
            {
                var ret = new OneArg();
                ret.Initialize(ctx, root.Name + nameSuffix);
                ret.Arg0 = CloneTreeStructure((root as OneArg).Arg0, ctx, nameSuffix);
                return ret;
            }
            else if (root is TwoArg)
            {
                var ret = new TwoArg();
                ret.Initialize(ctx, root.Name + nameSuffix);
                ret.Arg0 = CloneTreeStructure((root as TwoArg).Arg0, ctx, nameSuffix);
                ret.Arg1 = CloneTreeStructure((root as TwoArg).Arg1, ctx, nameSuffix);
                return ret;
            }
            else if (root is ThreeArg)
            {
                var ret = new ThreeArg();
                ret.Initialize(ctx, root.Name + nameSuffix);
                ret.Arg0 = CloneTreeStructure((root as ThreeArg).Arg0, ctx, nameSuffix);
                ret.Arg1 = CloneTreeStructure((root as ThreeArg).Arg1, ctx, nameSuffix);
                ret.Arg2 = CloneTreeStructure((root as ThreeArg).Arg2, ctx, nameSuffix);
                return ret;
            }

            throw new InvalidProgramException("Unknown type " + root.GetType());
        }

        public static void PopulateConstraints(ArgNode root, Context ctx, Solver solv, BitVecExpr input, BitVecExpr output, bool[] permitted)
        {
            root.AddConstraints(ctx, solv, input, output, permitted);
            foreach(ArgNode child in root.GetChildren())
            {
                PopulateConstraints(child, ctx, solv, input, output, permitted);
            }
        }

        public static void PopulateSolution(ArgNode root, Solver solv)
        {
            Model m = solv.Model;
            var opcodeTop = (IntExpr) m.Evaluate(root.OpCode);
            root.ComputedOpcode = (OpCodes)Int32.Parse(opcodeTop.ToString());
            foreach (ArgNode child in root.GetChildren())
            {
                PopulateSolution(child, solv);
            }
        }


        public static void MakeNodeTypesEqual(Context ctx, Solver solv, ArgNode root1, ArgNode root2)
        {
            if (root1.GetType() != root2.GetType())
            {
                throw new ArgumentException("Non-equals trees");
            }
            solv.Assert(ctx.MkEq(root1.OpCode, root2.OpCode));
            for (int i = 0; i < root1.GetChildren().Length; ++i)
            {
                MakeNodeTypesEqual(ctx, solv, root1.GetChildren()[i], root2.GetChildren()[i]);
            }
        }

        private static void GetAllNodesThatCanServeOpcode(ArgNode root, OpCodes opcode, List<ArgNode> accumulator)
        {
            List<ArgNode> res = new List<ArgNode>();
            if (opcode <= OpCodes.Input)
            {
                if (root is ZeroArg)
                {
                    accumulator.Add(root);
                }
            }
            else if (opcode <= OpCodes.Plus)
            {
                if (root is TwoArg)
                {
                    accumulator.Add(root);
                }
            }
            else if (opcode <= OpCodes.Shr16)
            {
                if (root is OneArg)
                {
                    accumulator.Add(root);
                }
            }
            else if (opcode <= OpCodes.If0)
            {
                if (root is ThreeArg)
                {
                    accumulator.Add(root);
                }
            }
            foreach (ArgNode child in root.GetChildren())
            {
                GetAllNodesThatCanServeOpcode(child, opcode, accumulator);
            }
            
        }

        public static void MakeSureOpcodeAppearsAtLeastOnce(Context ctx, Solver solv, ArgNode root, OpCodes opcode)
        {
            List<ArgNode> nodes = new List<ArgNode>();
            GetAllNodesThatCanServeOpcode(root, opcode, nodes);
            List<BoolExpr> expressions = new List<BoolExpr>();
            foreach(ArgNode n in nodes)
            {
                expressions.Add(ctx.MkEq(root.OpCode, ctx.MkInt((int)opcode)));
            }
            solv.Assert(ctx.MkOr(expressions.ToArray()));
        }

        public static ArgNode SolvePrototypeTree(ulong[] inputs, ulong[] outputs, ArgNode prototypeTreeRoot, bool[] permitted)
        {
            if(inputs.Length == 0 || inputs.Length != outputs.Length)
            {
                throw new ArgumentException("Invalid program inputs/outputs provided");
            }
            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                Solver s = ctx.MkSolver();
                ArgNode[] results = new ArgNode[inputs.Length];
                for (int i = 0; i < inputs.Length; ++i)
                {
                    results[i] = CloneTreeStructure(prototypeTreeRoot, ctx, "_" + inputs[i]);
                    PopulateConstraints(results[i], ctx, s, ctx.MkBV(inputs[i], 64), ctx.MkBV(outputs[i], 64), permitted);
                    s.Assert(ctx.MkEq(results[i].Output, ctx.MkBV(outputs[i], 64)));
                }

                // Makes multiple inputs work at the same time
                for (int i = 1; i < inputs.Length; ++i)
                {
                    MakeNodeTypesEqual(ctx, s, results[0], results[i]);
                }

                // Input, 0 and 1 are always permitted
                for (int i = (int)OpCodes.Input + 1; i < permitted.Length; ++i)
                {
                    if (permitted[i])
                    {
                        MakeSureOpcodeAppearsAtLeastOnce(ctx, s, results[0], (OpCodes)i);
                    }
                }


                if (s.Check() == Status.SATISFIABLE)
                {
                    Console.WriteLine("Success");
                    PopulateSolution(results[0], s);
                }
                else
                {
                    Console.WriteLine("Failure");
                }

                return results[0];
            }
        }


}
