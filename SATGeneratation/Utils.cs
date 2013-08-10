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
            else if (root is MetaArgNode)
            {
                var ret = new MetaArgNode();
                ret.Initialize(ctx, root.Name + nameSuffix);
                return ret;
            }

            throw new InvalidProgramException("Unknown type " + root.GetType());
        }

        public static void PopulateConstraints(ArgNode root, Context ctx, Solver solv, BitVecExpr input, BitVecExpr output, bool[] permitted, List<ArgNode> nodes, int curNodeIndex, TreeStructure tree)
        {
            root.AddConstraints(ctx, solv, input, output, permitted, nodes, curNodeIndex, tree);
            foreach (ArgNode child in root.GetChildren())
            {
                PopulateConstraints(child, ctx, solv, input, output, permitted, nodes, curNodeIndex, tree);
            }
        }

        public static void PopulateSolution(ArgNode root, Solver solv)
        {
            Model m = solv.Model;
            var opcodeTop = (IntExpr)m.Evaluate(root.OpCode);
            root.ComputedOpcode = (OpCodes)Int32.Parse(opcodeTop.ToString());
            Console.WriteLine("Opcode for node " + root.Name + " is " + root.ComputedOpcode);
            Console.WriteLine("Output for node " + root.Name + " is " + m.Evaluate(root.Output));
            Console.WriteLine("Arity for node " + root.Name + " is " + m.Evaluate(root.Arity));
            foreach (ArgNode child in root.GetChildren())
            {
                PopulateSolution(child, solv);
            }
        }

        public static void RecurseSubnode(List<ArgNode> nodes, Solver solv, int nodeIndex, ref int progIndex, int[] rev, int[] pins)
        {
            Model m = solv.Model;
            var opcodeTop = (IntExpr)m.Evaluate(nodes[nodeIndex].OpCode);
            nodes[progIndex++].ComputedOpcode = (OpCodes)Int32.Parse(opcodeTop.ToString());

            for (int i = 1; i < rev.Length; i++)
            {
                if (rev[i] == nodeIndex && pins[i] == 0)
                {
                    RecurseSubnode(nodes, solv, i, ref progIndex, rev, pins);
                }
            }

            for (int i = 1; i < rev.Length; i++)
            {
                if (rev[i] == nodeIndex && pins[i] == 1)
                {
                    RecurseSubnode(nodes, solv, i, ref progIndex, rev, pins);
                }
            }

            for (int i = 1; i < rev.Length; i++)
            {
                if (rev[i] == nodeIndex && pins[i] == 2)
                {
                    RecurseSubnode(nodes, solv, i, ref progIndex, rev, pins);
                }
            }
        }

        public static void PopulateSolution(List<ArgNode> nodes, Solver solv, TreeStructure tree)
        {
            Model m = solv.Model;
            var revZ3 = m.FuncInterp(tree.ReverseLink.FuncDecl);
            var pinsZ3 = m.FuncInterp(tree.PinLink.FuncDecl);
            int[] rev = new int[tree.TreeSize];
            int[] pins = new int[tree.TreeSize];
            for (int i = 0; i < rev.Length; ++i)
            {
                rev[i] = Int32.Parse(revZ3.Else.ToString());
                pins[i] = Int32.Parse(pinsZ3.Else.ToString());
            }

            for (int i = 0; i < revZ3.Entries.Length; ++i) 
            {
                rev[Int32.Parse(revZ3.Entries[i].Args[0].ToString())] = Int32.Parse(revZ3.Entries[i].Value.ToString());
                pins[Int32.Parse(pinsZ3.Entries[i].Args[0].ToString())] = Int32.Parse(pinsZ3.Entries[i].Value.ToString());
            }

            int progIndex = 0;
            RecurseSubnode(nodes, solv, 0, ref progIndex, rev, pins);
            if (progIndex != rev.Length)
            {
                Console.WriteLine("Warning - invalid program output...\n\n");
            }


            /*
            foreach (ArgNode n in nodes)
            {
                var opcodeTop = (IntExpr)m.Evaluate(n.OpCode);
                n.ComputedOpcode = (OpCodes)Int32.Parse(opcodeTop.ToString());
                // Console.WriteLine("The evaluated arity for {0} is {1}", n.Name, m.Evaluate(n.Arity));
                // Console.WriteLine("The evaluated opcode for {0} is {1}", n.Name, m.Evaluate(n.OpCode));
                // Console.WriteLine("The evaluated opcode for {0} is {1}", n.Name, m.Evaluate((n as MetaArgNode).ZeroArgNode.OpCode));


            }*/
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
            foreach (ArgNode n in nodes)
            {
                expressions.Add(ctx.MkEq(root.OpCode, ctx.MkInt((int)opcode)));
            }
            solv.Assert(ctx.MkOr(expressions.ToArray()));
        }

        public static void MakeSureOpcodeAppearsAtLeastOnce(Context ctx, Solver solv, List<ArgNode> nodes, OpCodes opcode)
        {
            List<BoolExpr> expressions = new List<BoolExpr>();
            foreach (ArgNode n in nodes)
            {
                expressions.Add(ctx.MkEq(n.OpCode, ctx.MkInt((int)opcode)));
            }
            solv.Assert(ctx.MkOr(expressions.ToArray()));
        }

        public static List<ArgNode> SolveNodeArray(ulong[] inputs, ulong[] outputs, List<ArgNode> nodes, bool[] permitted)
        {
            if (inputs.Length == 0 || inputs.Length != outputs.Length)
            {
                throw new ArgumentException("Invalid program inputs/outputs provided");
            }

            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                Solver s = ctx.MkSolver();
                List<ArgNode>[] results = new List<ArgNode>[inputs.Length];
                TreeStructure tree = new TreeStructure(ctx, "progtree", nodes.Count);
                s.Assert(tree.GetTreeLevelConstrains(ctx));


                for (int i = 0; i < inputs.Length; ++i)
                {
                    results[i] = new List<ArgNode>();
                    foreach(ArgNode n in nodes)
                    {
                        results[i].Add(CloneTreeStructure(n, ctx, "_" + inputs[i]));
                    }

                    for(int j = 0; j < nodes.Count; ++j)
                    {
                        PopulateConstraints(results[i][j], ctx, s, ctx.MkBV(inputs[i], 64), ctx.MkBV(outputs[i], 64), permitted, results[i], j, tree);
                    }
                    s.Assert(ctx.MkEq(results[i][0].Output, ctx.MkBV(outputs[i], 64)));
                }

                // Makes multiple inputs work at the same time
                for (int i = 1; i < inputs.Length; ++i)
                {
                    for (int j = 0; j < results[0].Count; ++j)
                    {
                        MakeNodeTypesEqual(ctx, s, results[0][j], results[i][j]);
                    }
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


                        Console.WriteLine("Success Tree");

                        Console.WriteLine("ArgCount:\n" + s.Model.FuncInterp(tree.ArgumentCount.FuncDecl));
                        Console.WriteLine("Revlinks:\n" + s.Model.FuncInterp(tree.ReverseLink.FuncDecl));
                        Console.WriteLine("Pins:\n" + s.Model.FuncInterp(tree.PinLink.FuncDecl));
                        Console.WriteLine("FW1:\n" + s.Model.FuncInterp(tree.FwLink1.FuncDecl));
                        //var af = s.Model.FuncInterp(a.FuncDecl); 
                        //var rf = s.Model.FuncInterp(r.FuncDecl); 
                        //for (int i = 0; i < n; ++i) 
                        //{ 
                        //    Console.WriteLine("-- {0}", af.Entries[i]); 
                        //}
                    PopulateSolution(results[0], s, tree);
                }
                else
                {
                    Console.WriteLine("Failure");
                }

                return results[0];
            }

        }

        public static ArgNode SolvePrototypeTree(ulong[] inputs, ulong[] outputs, ArgNode prototypeTreeRoot, bool[] permitted)
        {
            if (inputs.Length == 0 || inputs.Length != outputs.Length)
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
                    PopulateConstraints(results[i], ctx, s, ctx.MkBV(inputs[i], 64), ctx.MkBV(outputs[i], 64), permitted, null, -1, null);
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
}
