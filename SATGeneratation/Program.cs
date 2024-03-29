﻿using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    class Program
    {
        static void Main(string[] args)
        {
            bool[] permitted = new bool[Enum.GetValues(typeof(OpCodes)).Length];
            for (int i = 0; i < (int)OpCodes.Input + 1; ++i)
            {
                permitted[i] = true;
            }


            List<ArgNode> nodes = new List<ArgNode>();
            nodes.Add(new MetaArgNode() { Name = "n1" });
            nodes.Add(new MetaArgNode() { Name = "ar0n2" });
            nodes.Add(new MetaArgNode() { Name = "ar0n3" });

            {
                /*ulong[] inputs = { 0x1100 };
                ulong[] outputs = { 0x11  };*/
                
                ulong[] inputs = { 0x2, 0x020000 };
                ulong[] outputs = { 0x2, 0x2};
                TreeStructure.UseFwLinks = false;
                permitted[(int)OpCodes.Or] = true;
                //permitted[(int)OpCodes.One] = false;
                permitted[(int)OpCodes.Input2] = true;
                List<ArgNode> res = Utils.SolveTFoldArray(inputs, outputs, nodes, permitted);
                Console.WriteLine("[Node array] Tfold output {0}, {1} {2}", res[0].ComputedOpcode, res[1].ComputedOpcode, res[2].ComputedOpcode);
                permitted[(int)OpCodes.Or] = false;
            }

            return;

            {
                ulong[] inputs = { 0x1, 0x4 };
                ulong[] outputs = { 0x2, 0x8 };
                permitted[(int)OpCodes.Shl1] = true;
                List<ArgNode> res = Utils.SolveNodeArray(inputs, outputs, nodes, permitted);
                Console.WriteLine("[Node array] Shl1 output {0}, {1}", res[0].ComputedOpcode, res[1].ComputedOpcode);
                permitted[(int)OpCodes.Shl1] = false;
            }

            {
                ulong[] inputs = { 0x11, 0x10 };
                ulong[] outputs = { 0x1, 0x00};
                nodes.Add(new MetaArgNode() { Name = "ar0n3" });
                permitted[(int)OpCodes.And] = true;
                List<ArgNode> res = Utils.SolveNodeArray(inputs, outputs, nodes, permitted);
                Console.WriteLine("[Node array] And output {0}, {1} {2}", res[0].ComputedOpcode, res[1].ComputedOpcode, res[2].ComputedOpcode);
                permitted[(int)OpCodes.And] = false;
            }

            {
                TreeStructure.UseFwLinks = true;
                ulong[] inputs = { 0x10, 0x100, 0x0101 };
                //ulong[] outputs = { 0x11, 0x101, 0x0101, 0x011111, 0x0111111, 0x01111111 };
                //ulong[] outputs = { 0x09, 0x81, 0x081 };
                ulong[] outputs = { 0, 0, 0 };
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //ulong[] inputs = { 0x3452343534523453, 0x1C2523452335A523, 0x235247730894534 };
                //ulong[] outputs = { 0xABC3458204324524, 0xCDEF34A834532CD, 0x3452345245425345 };
                nodes.Add(new MetaArgNode() { Name = "ar0n3" });
                nodes.Add(new MetaArgNode() { Name = "ar0n4" });
                nodes.Add(new MetaArgNode() { Name = "ar0n5" });
                 nodes.Add(new MetaArgNode() { Name = "ar0n6" });
                 nodes.Add(new MetaArgNode() { Name = "ar0n7" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n8" });
                  nodes.Add(new MetaArgNode() { Name = "ar0n9" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n10" });
                  nodes.Add(new MetaArgNode() { Name = "ar0n11" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n12" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n13" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n14" });
                   /*nodes.Add(new MetaArgNode() { Name = "ar0n15" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n16" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n17" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n18" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n19" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n20" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n21" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n22" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n23" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n24" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n25" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n26" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n27" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n28" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n29" });
                   nodes.Add(new MetaArgNode() { Name = "ar0n30" });*/
                permitted[(int)OpCodes.And] = true;
                permitted[(int)OpCodes.Or] = true;
                permitted[(int)OpCodes.If0] = true;
                permitted[(int)OpCodes.Shr1] = true;
                permitted[(int)OpCodes.Shr4] = true;
                permitted[(int)OpCodes.Shr16] = true;
                permitted[(int)OpCodes.Shl1] = true;
                List<ArgNode> res = Utils.SolveNodeArray(inputs, outputs, nodes, permitted);
                Console.Write("[Node array] And example output :::");
                for (int i = 0; i < res.Count; i++)
                {
                    Console.Write(res[i].ComputedOpcode + " ");
                }
                Console.Write("\n");
                permitted[(int)OpCodes.And] = false;
                permitted[(int)OpCodes.Or] = false;

                sw.Stop();
                Console.WriteLine("It took " + sw.ElapsedMilliseconds/1000.0 + " sec to solve the problem");
            }

            return;

            {
                ulong[] inputs = { 0x10, 0x100, 0x0101 };
                ulong[] outputs = { 0x11, 0x101, 0x0101 };
                nodes.Add(new MetaArgNode() { Name = "ar0n3" });
                nodes.Add(new MetaArgNode() { Name = "ar0n4" });
                nodes.Add(new MetaArgNode() { Name = "ar0n5" });
                nodes.Add(new MetaArgNode() { Name = "ar0n6" });
                /*nodes.Add(new MetaArgNode() { Name = "ar0n7" });
                nodes.Add(new MetaArgNode() { Name = "ar0n8" });
                nodes.Add(new MetaArgNode() { Name = "ar0n9" });
                nodes.Add(new MetaArgNode() { Name = "ar0n10" });
                nodes.Add(new MetaArgNode() { Name = "ar0n11" });
                nodes.Add(new MetaArgNode() { Name = "ar0n12" });
                nodes.Add(new MetaArgNode() { Name = "ar0n13" });
                nodes.Add(new MetaArgNode() { Name = "ar0n14" });
                nodes.Add(new MetaArgNode() { Name = "ar0n15" });
                nodes.Add(new MetaArgNode() { Name = "ar0n16" });
                nodes.Add(new MetaArgNode() { Name = "ar0n17" });
                nodes.Add(new MetaArgNode() { Name = "ar0n18" });
                nodes.Add(new MetaArgNode() { Name = "ar0n19" });
                nodes.Add(new MetaArgNode() { Name = "ar0n20" });
                nodes.Add(new MetaArgNode() { Name = "ar0n21" });
                nodes.Add(new MetaArgNode() { Name = "ar0n22" });
                nodes.Add(new MetaArgNode() { Name = "ar0n23" });
                nodes.Add(new MetaArgNode() { Name = "ar0n24" });
                nodes.Add(new MetaArgNode() { Name = "ar0n25" });
                nodes.Add(new MetaArgNode() { Name = "ar0n26" });*/
                permitted[(int)OpCodes.And] = true;
                permitted[(int)OpCodes.Or] = true;
                permitted[(int)OpCodes.Shl1] = true;
                permitted[(int)OpCodes.Shr1] = true;
                List<ArgNode> res = Utils.SolveNodeArray(inputs, outputs, nodes, permitted);
                Console.Write("[Node array] And example output :::");
                for (int i = 0; i < res.Count; i++)
                {
                    Console.Write(res[i].ComputedOpcode + " ");
                }
                Console.Write("\n");
                permitted[(int)OpCodes.And] = false;
                permitted[(int)OpCodes.Or] = false;
            }
            return;


            OneArg OneArgProto = new OneArg() { Name = "OneArg" };
            OneArgProto.Arg0 = new ZeroArg() { Name = "OneArgC0" };

            TwoArg TwoArgProto = new TwoArg() { Name = "TwoArg" };
            TwoArgProto.Arg0 = new ZeroArg() { Name = "TwoArgC0" };
            TwoArgProto.Arg1 = new ZeroArg() { Name = "TwoArgC1" };

            ThreeArg ThreeArgProto = new ThreeArg() { Name = "ThreeArg" };
            ThreeArgProto.Arg0 = new ZeroArg() { Name = "ThreeArg0" };
            ThreeArgProto.Arg1 = new ZeroArg() { Name = "ThreeArg1" };
            ThreeArgProto.Arg2 = new ZeroArg() { Name = "ThreeArg2" };
            
            {
                ulong[] inputs = { 0x1, 0x2 };
                ulong[] outputs = { 0x2, 0x4 };
                permitted[(int)OpCodes.Shl1] = true;
                OneArg sol = (OneArg) Utils.SolvePrototypeTree(inputs, outputs, OneArgProto, permitted);
                Console.WriteLine("Shl1 output {0}, {1}", sol.ComputedOpcode, sol.Arg0.ComputedOpcode);
                permitted[(int)OpCodes.Shl1] = false;
            }

            {
                ulong[] inputs = { 0x2, 0x4 };
                ulong[] outputs = { 0x1, 0x2 };
                permitted[(int)OpCodes.Shr1] = true;
                OneArg sol = (OneArg)Utils.SolvePrototypeTree(inputs, outputs, OneArgProto, permitted);
                Console.WriteLine("Shr1 output {0}, {1}", sol.ComputedOpcode, sol.Arg0.ComputedOpcode);
                permitted[(int)OpCodes.Shr1] = false;
            }

            {
                ulong[] inputs = { 0x10, 0x100 };
                ulong[] outputs = { 0x1, 0x10 };
                permitted[(int)OpCodes.Shr4] = true;
                OneArg sol = (OneArg)Utils.SolvePrototypeTree(inputs, outputs, OneArgProto, permitted);
                Console.WriteLine("Shr4 output {0}, {1}", sol.ComputedOpcode, sol.Arg0.ComputedOpcode);
                permitted[(int)OpCodes.Shr4] = false;
            }

            {
                ulong[] inputs = { 0x10000, 0x100000 };
                ulong[] outputs = { 0x1, 0x10 };
                permitted[(int)OpCodes.Shr16] = true;
                OneArg sol = (OneArg)Utils.SolvePrototypeTree(inputs, outputs, OneArgProto, permitted);
                Console.WriteLine("Shr16 output {0}, {1}", sol.ComputedOpcode, sol.Arg0.ComputedOpcode);
                permitted[(int)OpCodes.Shr16] = false;
            }

            {
                ulong[] inputs = { 0x1000000000000000, 0x1100000000000000 };
                ulong[] outputs = { 0xEFFFFFFFFFFFFFFF, 0xEEFFFFFFFFFFFFFF };
                permitted[(int)OpCodes.Not] = true;
                OneArg sol = (OneArg)Utils.SolvePrototypeTree(inputs, outputs, OneArgProto, permitted);
                Console.WriteLine("Not output {0}, {1}", sol.ComputedOpcode, sol.Arg0.ComputedOpcode);
                permitted[(int)OpCodes.Not] = false;
            }

            {
                ulong[] inputs = { 0x1, 0x11111, 0x111110 };
                ulong[] outputs = { 0x1, 0x1, 0x0 };
                permitted[(int)OpCodes.And] = true;
                TwoArg sol = (TwoArg)Utils.SolvePrototypeTree(inputs, outputs, TwoArgProto, permitted);
                Console.WriteLine("And output {0}, {1}, {2}", sol.ComputedOpcode, sol.Arg0.ComputedOpcode, sol.Arg1.ComputedOpcode);
                permitted[(int)OpCodes.And] = false;
            }

            {
                ulong[] inputs = { 0x1, 0x11110, 0x111110 };
                ulong[] outputs = { 0x1, 0x11111, 0x111111 };
                permitted[(int)OpCodes.Or] = true;
                TwoArg sol = (TwoArg)Utils.SolvePrototypeTree(inputs, outputs, TwoArgProto, permitted);
                Console.WriteLine("Or output {0}, {1}, {2}", sol.ComputedOpcode, sol.Arg0.ComputedOpcode, sol.Arg1.ComputedOpcode);
                permitted[(int)OpCodes.Or] = false;
            }

            {
                ulong[] inputs = { 0x1, 7, 10, 23 };
                ulong[] outputs = { 0x2, 8, 11, 24 };
                permitted[(int)OpCodes.Plus] = true;
                TwoArg sol = (TwoArg)Utils.SolvePrototypeTree(inputs, outputs, TwoArgProto, permitted);
                Console.WriteLine("Plus output {0}, {1}, {2}", sol.ComputedOpcode, sol.Arg0.ComputedOpcode, sol.Arg1.ComputedOpcode);
                permitted[(int)OpCodes.Plus] = false;
            }

            {
                ulong[] inputs = { 0x1, 0x11110, 0x111110 };
                ulong[] outputs = { 0x0, 0x11111, 0x111111 };
                permitted[(int)OpCodes.Xor] = true;
                TwoArg sol = (TwoArg)Utils.SolvePrototypeTree(inputs, outputs, TwoArgProto, permitted);
                Console.WriteLine("Xor output {0}, {1}, {2}", sol.ComputedOpcode, sol.Arg0.ComputedOpcode, sol.Arg1.ComputedOpcode);
                permitted[(int)OpCodes.Xor] = false;
            }

            {
                ulong[] inputs = { 0x0, 0x1111, 0x1 };
                ulong[] outputs = { 0x1, 0x0, 0x0 };
                permitted[(int)OpCodes.If0] = true;
                ThreeArg sol = (ThreeArg)Utils.SolvePrototypeTree(inputs, outputs, ThreeArgProto, permitted);
                Console.WriteLine("If0 output {0}, {1}, {2}, {3}", sol.ComputedOpcode, sol.Arg0.ComputedOpcode, sol.Arg1.ComputedOpcode, sol.Arg2.ComputedOpcode);
                permitted[(int)OpCodes.If0] = false;
            }

        }

        static void Main2(string[] args)
        {
            bool[] permitted = new bool[Enum.GetValues(typeof(OpCodes)).Length];
            for (int i = 0; i < permitted.Length; ++i)
            {
                permitted[i] = true;
            }

            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                TwoArg TopNode = new TwoArg();
                TopNode.Arg0 = new ZeroArg();
                TopNode.Arg1 = new ZeroArg();

                TopNode.Initialize(ctx, "top");
                TopNode.Arg0.Initialize(ctx, "left");
                TopNode.Arg1.Initialize(ctx, "right");
                Solver s = ctx.MkSolver();
                BitVecExpr prInput = ctx.MkBV(0x111, 64);
                BitVecExpr prOutput = ctx.MkBV(1, 64);
                s.Assert(ctx.MkEq(TopNode.Output, prOutput));
                TopNode.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg1.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);

                TwoArg TopNode2 = new TwoArg();
                TopNode2.Arg0 = new ZeroArg();
                TopNode2.Arg1 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "left2");
                TopNode2.Arg1.Initialize(ctx, "right2");
                BitVecExpr prInput2 = ctx.MkBV(0x110, 64);
                BitVecExpr prOutput2 = ctx.MkBV(0, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg1.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);

                s.Assert(ctx.MkEq(TopNode2.OpCode, TopNode.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg0.OpCode, TopNode.Arg0.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg1.OpCode, TopNode.Arg1.OpCode));

                if(s.Check() == Status.SATISFIABLE)
                {
                    Console.WriteLine("Success");
                    Model m = s.Model;
                    var opcodeTop = m.Evaluate(TopNode.OpCode);
                    var opcodeLeft = m.Evaluate(TopNode.Arg0.OpCode);
                    var opcodeRight = m.Evaluate(TopNode.Arg1.OpCode);
                    Console.WriteLine(opcodeTop);
                    Console.WriteLine(opcodeLeft);
                    Console.WriteLine(opcodeRight);
                }
                else
                {
                    Console.WriteLine("Failure");
                }
            }


            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                TwoArg TopNode = new TwoArg();
                TopNode.Arg0 = new ZeroArg();
                TopNode.Arg1 = new ZeroArg();

                TopNode.Initialize(ctx, "top");
                TopNode.Arg0.Initialize(ctx, "left");
                TopNode.Arg1.Initialize(ctx, "right");
                Solver s = ctx.MkSolver();
                BitVecExpr prInput = ctx.MkBV(0x10, 64);
                BitVecExpr prOutput = ctx.MkBV(0x11, 64);
                s.Assert(ctx.MkEq(TopNode.Output, prOutput));
                TopNode.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg1.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);

                TwoArg TopNode2 = new TwoArg();
                TopNode2.Arg0 = new ZeroArg();
                TopNode2.Arg1 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "left2");
                TopNode2.Arg1.Initialize(ctx, "right2");
                BitVecExpr prInput2 = ctx.MkBV(0x101, 64);
                BitVecExpr prOutput2 = ctx.MkBV(0x102, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg1.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);

                s.Assert(ctx.MkEq(TopNode2.OpCode, TopNode.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg0.OpCode, TopNode.Arg0.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg1.OpCode, TopNode.Arg1.OpCode));

                if (s.Check() == Status.SATISFIABLE)
                {
                    Console.WriteLine("Success");
                    Model m = s.Model;
                    var opcodeTop = m.Evaluate(TopNode.OpCode);
                    var opcodeLeft = m.Evaluate(TopNode.Arg0.OpCode);
                    var opcodeRight = m.Evaluate(TopNode.Arg1.OpCode);
                    Console.WriteLine(opcodeTop);
                    Console.WriteLine(opcodeLeft);
                    Console.WriteLine(opcodeRight);
                }
                else
                {
                    Console.WriteLine("Failure");
                }
            }


            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                TwoArg TopNode = new TwoArg();
                TopNode.Arg0 = new ZeroArg();
                TopNode.Arg1 = new ZeroArg();

                TopNode.Initialize(ctx, "top");
                TopNode.Arg0.Initialize(ctx, "left");
                TopNode.Arg1.Initialize(ctx, "right");
                Solver s = ctx.MkSolver();
                BitVecExpr prInput = ctx.MkBV(0x10, 64);
                BitVecExpr prOutput = ctx.MkBV(0x0, 64);
                s.Assert(ctx.MkEq(TopNode.Output, prOutput));
                TopNode.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg1.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);

                TwoArg TopNode2 = new TwoArg();
                TopNode2.Arg0 = new ZeroArg();
                TopNode2.Arg1 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "left2");
                TopNode2.Arg1.Initialize(ctx, "right2");
                BitVecExpr prInput2 = ctx.MkBV(0x101, 64);
                BitVecExpr prOutput2 = ctx.MkBV(0x0, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg1.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);

                s.Assert(ctx.MkEq(TopNode2.OpCode, TopNode.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg0.OpCode, TopNode.Arg0.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg1.OpCode, TopNode.Arg1.OpCode));

                if (s.Check() == Status.SATISFIABLE)
                {
                    Console.WriteLine("Success");
                    Model m = s.Model;
                    var opcodeTop = m.Evaluate(TopNode.OpCode);
                    var opcodeLeft = m.Evaluate(TopNode.Arg0.OpCode);
                    var opcodeRight = m.Evaluate(TopNode.Arg1.OpCode);
                    Console.WriteLine(opcodeTop);
                    Console.WriteLine(opcodeLeft);
                    Console.WriteLine(opcodeRight);
                }
                else
                {
                    Console.WriteLine("Failure");
                }
            }


            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                TwoArg TopNode = new TwoArg();
                TopNode.Arg0 = new ZeroArg();
                TopNode.Arg1 = new ZeroArg();

                TopNode.Initialize(ctx, "top");
                TopNode.Arg0.Initialize(ctx, "left");
                TopNode.Arg1.Initialize(ctx, "right");
                Solver s = ctx.MkSolver();
                BitVecExpr prInput = ctx.MkBV(0x11, 64);
                BitVecExpr prOutput = ctx.MkBV(0x11, 64);
                s.Assert(ctx.MkEq(TopNode.Output, prOutput));
                TopNode.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg1.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);

                TwoArg TopNode2 = new TwoArg();
                TopNode2.Arg0 = new ZeroArg();
                TopNode2.Arg1 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "left2");
                TopNode2.Arg1.Initialize(ctx, "right2");
                BitVecExpr prInput2 = ctx.MkBV(0x100, 64);
                BitVecExpr prOutput2 = ctx.MkBV(0x101, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg1.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);

                s.Assert(ctx.MkEq(TopNode2.OpCode, TopNode.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg0.OpCode, TopNode.Arg0.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg1.OpCode, TopNode.Arg1.OpCode));

                if (s.Check() == Status.SATISFIABLE)
                {
                    Console.WriteLine("Success");
                    Model m = s.Model;
                    var opcodeTop = m.Evaluate(TopNode.OpCode);
                    var opcodeLeft = m.Evaluate(TopNode.Arg0.OpCode);
                    var opcodeRight = m.Evaluate(TopNode.Arg1.OpCode);
                    Console.WriteLine(opcodeTop);
                    Console.WriteLine(opcodeLeft);
                    Console.WriteLine(opcodeRight);
                }
                else
                {
                    Console.WriteLine("Failure");
                }
            }

            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                OneArg TopNode = new OneArg();
                TopNode.Arg0 = new ZeroArg();

                TopNode.Initialize(ctx, "top");
                TopNode.Arg0.Initialize(ctx, "bottom");
                Solver s = ctx.MkSolver();
                BitVecExpr prInput = ctx.MkBV(0x001, 64);
                BitVecExpr prOutput = ctx.MkBV(0x002, 64);
                s.Assert(ctx.MkEq(TopNode.Output, prOutput));
                TopNode.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);

                OneArg TopNode2 = new OneArg();
                TopNode2.Arg0 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "bottom2");
                BitVecExpr prInput2 = ctx.MkBV(0x100, 64);
                BitVecExpr prOutput2 = ctx.MkBV(0x200, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);

                s.Assert(ctx.MkEq(TopNode2.OpCode, TopNode.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg0.OpCode, TopNode.Arg0.OpCode));

                if (s.Check() == Status.SATISFIABLE)
                {
                    Console.WriteLine("Success");
                    Model m = s.Model;
                    var opcodeTop = m.Evaluate(TopNode.OpCode);
                    var opcodeLeft = m.Evaluate(TopNode.Arg0.OpCode);
                    Console.WriteLine(opcodeTop);
                    Console.WriteLine(opcodeLeft);
                }
                else
                {
                    Console.WriteLine("Failure");
                }
            }

            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                OneArg TopNode = new OneArg();
                TopNode.Arg0 = new ZeroArg();

                TopNode.Initialize(ctx, "top");
                TopNode.Arg0.Initialize(ctx, "bottom");
                Solver s = ctx.MkSolver();
                BitVecExpr prInput = ctx.MkBV(0x002, 64);
                BitVecExpr prOutput = ctx.MkBV(0x001, 64);
                s.Assert(ctx.MkEq(TopNode.Output, prOutput));
                TopNode.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);

                OneArg TopNode2 = new OneArg();
                TopNode2.Arg0 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "bottom2");
                BitVecExpr prInput2 = ctx.MkBV(0x200, 64);
                BitVecExpr prOutput2 = ctx.MkBV(0x100, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);

                s.Assert(ctx.MkEq(TopNode2.OpCode, TopNode.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg0.OpCode, TopNode.Arg0.OpCode));

                if (s.Check() == Status.SATISFIABLE)
                {
                    Console.WriteLine("Success");
                    Model m = s.Model;
                    var opcodeTop = m.Evaluate(TopNode.OpCode);
                    var opcodeLeft = m.Evaluate(TopNode.Arg0.OpCode);
                    Console.WriteLine(opcodeTop);
                    Console.WriteLine(opcodeLeft);
                }
                else
                {
                    Console.WriteLine("Failure");
                }
            }

            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                OneArg TopNode = new OneArg();
                TopNode.Arg0 = new ZeroArg();

                TopNode.Initialize(ctx, "top");
                TopNode.Arg0.Initialize(ctx, "bottom");
                Solver s = ctx.MkSolver();
                BitVecExpr prInput = ctx.MkBV((ulong)0x1000000000000000, 64);
                BitVecExpr prOutput = ctx.MkBV((ulong)0xEFFFFFFFFFFFFFFF, 64);
                s.Assert(ctx.MkEq(TopNode.Output, prOutput));
                TopNode.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);

                OneArg TopNode2 = new OneArg();
                TopNode2.Arg0 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "bottom2");
                BitVecExpr prInput2 = ctx.MkBV((ulong)0x1100000000000000, 64);
                BitVecExpr prOutput2 = ctx.MkBV((ulong)0xEEFFFFFFFFFFFFFF, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);

                s.Assert(ctx.MkEq(TopNode2.OpCode, TopNode.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg0.OpCode, TopNode.Arg0.OpCode));

                if (s.Check() == Status.SATISFIABLE)
                {
                    Console.WriteLine("Success");
                    Model m = s.Model;
                    var opcodeTop = m.Evaluate(TopNode.OpCode);
                    var opcodeLeft = m.Evaluate(TopNode.Arg0.OpCode);
                    Console.WriteLine(opcodeTop);
                    Console.WriteLine(opcodeLeft);
                }
                else
                {
                    Console.WriteLine("Failure");
                }
            }


            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                OneArg TopNode = new OneArg();
                TopNode.Arg0 = new ZeroArg();

                TopNode.Initialize(ctx, "top");
                TopNode.Arg0.Initialize(ctx, "bottom");
                Solver s = ctx.MkSolver();
                BitVecExpr prInput = ctx.MkBV((ulong)0x100, 64);
                BitVecExpr prOutput = ctx.MkBV((ulong)0x10, 64);
                s.Assert(ctx.MkEq(TopNode.Output, prOutput));
                TopNode.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);

                OneArg TopNode2 = new OneArg();
                TopNode2.Arg0 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "bottom2");
                BitVecExpr prInput2 = ctx.MkBV((ulong)0x1000, 64);
                BitVecExpr prOutput2 = ctx.MkBV((ulong)0x100, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);

                s.Assert(ctx.MkEq(TopNode2.OpCode, TopNode.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg0.OpCode, TopNode.Arg0.OpCode));

                if (s.Check() == Status.SATISFIABLE)
                {
                    Console.WriteLine("Success");
                    Model m = s.Model;
                    var opcodeTop = m.Evaluate(TopNode.OpCode);
                    var opcodeLeft = m.Evaluate(TopNode.Arg0.OpCode);
                    Console.WriteLine(opcodeTop);
                    Console.WriteLine(opcodeLeft);
                }
                else
                {
                    Console.WriteLine("Failure");
                }
            }

            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                OneArg TopNode = new OneArg();
                TopNode.Arg0 = new ZeroArg();

                TopNode.Initialize(ctx, "top");
                TopNode.Arg0.Initialize(ctx, "bottom");
                Solver s = ctx.MkSolver();
                BitVecExpr prInput = ctx.MkBV((ulong)0x100000, 64);
                BitVecExpr prOutput = ctx.MkBV((ulong)0x10, 64);
                s.Assert(ctx.MkEq(TopNode.Output, prOutput));
                TopNode.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);

                OneArg TopNode2 = new OneArg();
                TopNode2.Arg0 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "bottom2");
                BitVecExpr prInput2 = ctx.MkBV((ulong)0x1000000000000000, 64);
                BitVecExpr prOutput2 = ctx.MkBV((ulong)0x100000000000, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);

                s.Assert(ctx.MkEq(TopNode2.OpCode, TopNode.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg0.OpCode, TopNode.Arg0.OpCode));

                if (s.Check() == Status.SATISFIABLE)
                {
                    Console.WriteLine("Success");
                    Model m = s.Model;
                    var opcodeTop = m.Evaluate(TopNode.OpCode);
                    var opcodeLeft = m.Evaluate(TopNode.Arg0.OpCode);
                    Console.WriteLine(opcodeTop);
                    Console.WriteLine(opcodeLeft);
                }
                else
                {
                    Console.WriteLine("Failure");
                }
            }

            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                ThreeArg TopNode = new ThreeArg();
                TopNode.Arg0 = new ZeroArg();
                TopNode.Arg1 = new ZeroArg();
                TopNode.Arg2 = new ZeroArg();

                TopNode.Initialize(ctx, "top");
                TopNode.Arg0.Initialize(ctx, "left");
                TopNode.Arg1.Initialize(ctx, "middle");
                TopNode.Arg2.Initialize(ctx, "right");
                Solver s = ctx.MkSolver();
                BitVecExpr prInput = ctx.MkBV(0x1, 64);
                BitVecExpr prOutput = ctx.MkBV(0, 64);
                s.Assert(ctx.MkEq(TopNode.Output, prOutput));
                TopNode.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg1.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg2.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);

                ThreeArg TopNode2 = new ThreeArg();
                TopNode2.Arg0 = new ZeroArg();
                TopNode2.Arg1 = new ZeroArg();
                TopNode2.Arg2 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "left2");
                TopNode2.Arg1.Initialize(ctx, "middle2");
                TopNode2.Arg2.Initialize(ctx, "right2");
                BitVecExpr prInput2 = ctx.MkBV(0x0, 64);
                BitVecExpr prOutput2 = ctx.MkBV(0x1, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg1.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg2.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);

                s.Assert(ctx.MkEq(TopNode2.OpCode, TopNode.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg0.OpCode, TopNode.Arg0.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg1.OpCode, TopNode.Arg1.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg2.OpCode, TopNode.Arg2.OpCode));

                if (s.Check() == Status.SATISFIABLE)
                {
                    Console.WriteLine("Success");
                    Model m = s.Model;
                    var opcodeTop = m.Evaluate(TopNode.OpCode);
                    var opcodeLeft = m.Evaluate(TopNode.Arg0.OpCode);
                    var opcodeMiddle = m.Evaluate(TopNode.Arg1.OpCode);
                    var opcodeRight = m.Evaluate(TopNode.Arg2.OpCode);
                    Console.WriteLine(opcodeTop);
                    Console.WriteLine(opcodeLeft);
                    Console.WriteLine(opcodeMiddle);
                    Console.WriteLine(opcodeRight);
                }
                else
                {
                    Console.WriteLine("Failure");
                }
            }


            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                ThreeArg TopNode = new ThreeArg();
                TopNode.Arg0 = new ZeroArg();
                TopNode.Arg1 = new ZeroArg();
                TopNode.Arg2 = new ZeroArg();

                TopNode.Initialize(ctx, "top");
                TopNode.Arg0.Initialize(ctx, "left");
                TopNode.Arg1.Initialize(ctx, "middle");
                TopNode.Arg2.Initialize(ctx, "right");
                Solver s = ctx.MkSolver();
                BitVecExpr prInput = ctx.MkBV(0x0, 64);
                BitVecExpr prOutput = ctx.MkBV(0, 64);
                s.Assert(ctx.MkEq(TopNode.Output, prOutput));
                TopNode.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg1.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);
                TopNode.Arg2.AddConstraints(ctx, s, prInput, null, prOutput, permitted, null, -1, null);

                ThreeArg TopNode2 = new ThreeArg();
                TopNode2.Arg0 = new ZeroArg();
                TopNode2.Arg1 = new ZeroArg();
                TopNode2.Arg2 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "left2");
                TopNode2.Arg1.Initialize(ctx, "middle2");
                TopNode2.Arg2.Initialize(ctx, "right2");
                BitVecExpr prInput2 = ctx.MkBV(0x1111, 64);
                BitVecExpr prOutput2 = ctx.MkBV(0x1, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg1.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);
                TopNode2.Arg2.AddConstraints(ctx, s, prInput2, null, prOutput2, permitted, null, -1, null);

                s.Assert(ctx.MkEq(TopNode2.OpCode, TopNode.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg0.OpCode, TopNode.Arg0.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg1.OpCode, TopNode.Arg1.OpCode));
                s.Assert(ctx.MkEq(TopNode2.Arg2.OpCode, TopNode.Arg2.OpCode));

                if (s.Check() == Status.SATISFIABLE)
                {
                    Console.WriteLine("Success");
                    Model m = s.Model;
                    var opcodeTop = m.Evaluate(TopNode.OpCode);
                    var opcodeLeft = m.Evaluate(TopNode.Arg0.OpCode);
                    var opcodeMiddle = m.Evaluate(TopNode.Arg1.OpCode);
                    var opcodeRight = m.Evaluate(TopNode.Arg2.OpCode);
                    Console.WriteLine(opcodeTop);
                    Console.WriteLine(opcodeLeft);
                    Console.WriteLine(opcodeMiddle);
                    Console.WriteLine(opcodeRight);
                }
                else
                {
                    Console.WriteLine("Failure");
                }
            }


        }
    }
}
