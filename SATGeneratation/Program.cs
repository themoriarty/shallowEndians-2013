using Microsoft.Z3;
using System;
using System.Collections.Generic;
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
                TopNode.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg1.AddConstraints(ctx, s, prInput, prOutput, permitted);

                TwoArg TopNode2 = new TwoArg();
                TopNode2.Arg0 = new ZeroArg();
                TopNode2.Arg1 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "left2");
                TopNode2.Arg1.Initialize(ctx, "right2");
                BitVecExpr prInput2 = ctx.MkBV(0x110, 64);
                BitVecExpr prOutput2 = ctx.MkBV(0, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg1.AddConstraints(ctx, s, prInput2, prOutput2, permitted);

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
                TopNode.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg1.AddConstraints(ctx, s, prInput, prOutput, permitted);

                TwoArg TopNode2 = new TwoArg();
                TopNode2.Arg0 = new ZeroArg();
                TopNode2.Arg1 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "left2");
                TopNode2.Arg1.Initialize(ctx, "right2");
                BitVecExpr prInput2 = ctx.MkBV(0x101, 64);
                BitVecExpr prOutput2 = ctx.MkBV(0x102, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg1.AddConstraints(ctx, s, prInput2, prOutput2, permitted);

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
                TopNode.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg1.AddConstraints(ctx, s, prInput, prOutput, permitted);

                TwoArg TopNode2 = new TwoArg();
                TopNode2.Arg0 = new ZeroArg();
                TopNode2.Arg1 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "left2");
                TopNode2.Arg1.Initialize(ctx, "right2");
                BitVecExpr prInput2 = ctx.MkBV(0x101, 64);
                BitVecExpr prOutput2 = ctx.MkBV(0x0, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg1.AddConstraints(ctx, s, prInput2, prOutput2, permitted);

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
                TopNode.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg1.AddConstraints(ctx, s, prInput, prOutput, permitted);

                TwoArg TopNode2 = new TwoArg();
                TopNode2.Arg0 = new ZeroArg();
                TopNode2.Arg1 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "left2");
                TopNode2.Arg1.Initialize(ctx, "right2");
                BitVecExpr prInput2 = ctx.MkBV(0x100, 64);
                BitVecExpr prOutput2 = ctx.MkBV(0x101, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg1.AddConstraints(ctx, s, prInput2, prOutput2, permitted);

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
                TopNode.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, prOutput, permitted);

                OneArg TopNode2 = new OneArg();
                TopNode2.Arg0 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "bottom2");
                BitVecExpr prInput2 = ctx.MkBV(0x100, 64);
                BitVecExpr prOutput2 = ctx.MkBV(0x200, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, prOutput2, permitted);

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
                TopNode.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, prOutput, permitted);

                OneArg TopNode2 = new OneArg();
                TopNode2.Arg0 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "bottom2");
                BitVecExpr prInput2 = ctx.MkBV(0x200, 64);
                BitVecExpr prOutput2 = ctx.MkBV(0x100, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, prOutput2, permitted);

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
                TopNode.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, prOutput, permitted);

                OneArg TopNode2 = new OneArg();
                TopNode2.Arg0 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "bottom2");
                BitVecExpr prInput2 = ctx.MkBV((ulong)0x1100000000000000, 64);
                BitVecExpr prOutput2 = ctx.MkBV((ulong)0xEEFFFFFFFFFFFFFF, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, prOutput2, permitted);

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
                TopNode.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, prOutput, permitted);

                OneArg TopNode2 = new OneArg();
                TopNode2.Arg0 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "bottom2");
                BitVecExpr prInput2 = ctx.MkBV((ulong)0x1000, 64);
                BitVecExpr prOutput2 = ctx.MkBV((ulong)0x100, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, prOutput2, permitted);

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
                TopNode.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, prOutput, permitted);

                OneArg TopNode2 = new OneArg();
                TopNode2.Arg0 = new ZeroArg();

                TopNode2.Initialize(ctx, "top2");
                TopNode2.Arg0.Initialize(ctx, "bottom2");
                BitVecExpr prInput2 = ctx.MkBV((ulong)0x1000000000000000, 64);
                BitVecExpr prOutput2 = ctx.MkBV((ulong)0x100000000000, 64);
                s.Assert(ctx.MkEq(TopNode2.Output, prOutput2));
                TopNode2.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, prOutput2, permitted);

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
                TopNode.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg1.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg2.AddConstraints(ctx, s, prInput, prOutput, permitted);

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
                TopNode2.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg1.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg2.AddConstraints(ctx, s, prInput2, prOutput2, permitted);

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
                TopNode.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg0.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg1.AddConstraints(ctx, s, prInput, prOutput, permitted);
                TopNode.Arg2.AddConstraints(ctx, s, prInput, prOutput, permitted);

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
                TopNode2.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg0.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg1.AddConstraints(ctx, s, prInput2, prOutput2, permitted);
                TopNode2.Arg2.AddConstraints(ctx, s, prInput2, prOutput2, permitted);

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
