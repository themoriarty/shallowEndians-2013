using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icfpc2013.Ops;

namespace Icfpc2013
{
    class Bonus1Solver
    {
        private ulong[] Inputs;
        private ulong[] Outputs;
        private long MaxSize;
        private List<Node> ValidNodes;

        class IO
        {
            public List<ulong> Inputs0;
            public List<ulong> Outputs0;
            public Node Program0;

            public List<ulong> Inputs1;
            public List<ulong> Outputs1;
            public Node Program1;

            public IO()
            {
                Inputs0 = new List<ulong>();
                Outputs0 = new List<ulong>();
                Inputs1 = new List<ulong>();
                Outputs1 = new List<ulong>();
            }

            public IO Clone()
            {
                var ret = new IO();
                ret.Inputs0.AddRange(Inputs0);
                ret.Outputs0.AddRange(Outputs0);
                ret.Program0 = Program0 == null ? null : Program0.Clone();
                ret.Inputs1.AddRange(Inputs1);
                ret.Outputs1.AddRange(Outputs1);
                ret.Program1 = Program1 == null ? null : Program1.Clone();
                return ret;
            }
        }

        public Bonus1Solver(ulong[] inputs, ulong[] outputs, long maxSize, List<Node> validNodes)
        {
            Inputs = inputs;
            Outputs = outputs;
            MaxSize = maxSize;
            ValidNodes = validNodes;
        }

        IEnumerable<IO> SplitIOs()
        {
            List<IO> ret = new List<IO>();
            var io = new IO();
            io.Inputs0.Add(Inputs[0]);
            io.Outputs0.Add(Outputs[0]);
            ret.Add(io);
            io = new IO();
            io.Inputs1.Add(Inputs[0]);
            io.Outputs1.Add(Outputs[0]);
            ret.Add(io);
            for (int i = 1; i < Inputs.Length; ++i)
            {
                var input = Inputs[i];
                var output = Outputs[i];

                List<IO> newRet = new List<IO>();
                foreach (var currentIO in ret)
                {
                    var newIO = currentIO.Clone();
                    newIO.Inputs0.Add(input);
                    newIO.Outputs0.Add(output);
                    newRet.Add(newIO);
                    newIO = currentIO.Clone();
                    newIO.Inputs1.Add(input);
                    newIO.Outputs1.Add(output);
                    newRet.Add(newIO);
                }
                ret = newRet;
            }
            return ret;
        }

        private bool Ok(Node program, ulong[] inputs, ulong[] outputs)
        {
            bool valid = true;
            for (int i = 0; i < inputs.Length; ++i)
            {
                var ctx = new ExecContext();
                ctx.Vars["x"] = inputs[i];
                var output = program.Eval(ctx);
                if (output != outputs[i])
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerable<Node> Solve()
        {
            int cnt = 0;
            var nonIfNodes = ValidNodes.Where(p => !(p is NodeIf0)).ToList();
            //var gen = new FTreeGenerator(nonIfNodes, 7);
            //var tree7 = gen.GenerateAllPrograms().ToArray();
            //var allIos = SplitIOs();
            List<IO> seenIos = new List<IO>();
            foreach (var io in SplitIOs())
            {
                if (seenIos.Any(sio => sio.Inputs0 == io.Inputs0 || sio.Inputs0 == io.Inputs1))
                {
                    continue;
                }
                seenIos.Add(io);
                //if (io.Inputs0.Count == 1 && io.Inputs0[0] == 18446744073709551615)
                //{
                //    Console.WriteLine("XXX");
                //}
                //var slv0 = new AStarSolver(io.Inputs0.ToArray(), io.Outputs0.ToArray(), 7, nonIfNodes);
                //var slv1 = new AStarSolver(io.Inputs1.ToArray(), io.Outputs1.ToArray(), 7, nonIfNodes);
                Console.WriteLine("0: {0} -> {1}", String.Join(", ", io.Inputs0), String.Join(", ", io.Outputs0));
                Console.WriteLine("1: {0} -> {1}", String.Join(", ", io.Inputs1), String.Join(", ", io.Outputs1));
                var s0l  = new Lambda1{Node0 = Node0.Instance};
                if (io.Inputs0.Count != 0)
                {
                    if (true/*io.Inputs0.Count == 1 && io.Inputs0[0] == 18446744073709551615*/)
                    {
                        s0l = Program.SolveSat(8, nonIfNodes, io.Inputs0.ToArray(), io.Outputs0.ToArray());
                        //s0l = Program.SolveSat(8, new List<Node> {new NodeOp2Plus(), new NodeOp1Shr1()}, io.Inputs0.ToArray(), io.Outputs0.ToArray());
                    }
                    else
                    {
                        s0l = null;
                    }
                }
                if (s0l != null)
                //foreach (var s0 in slv0.Solve())
                {
                    var s0 = s0l.Node0;
                    //if (Ok(s0, io.Inputs0.ToArray(), io.Outputs0.ToArray()))
                    {
                        var s1l = new Lambda1 { Node0 = Node0.Instance };
                        if (io.Inputs1.Count != 0)
                        {
                            s1l = Program.SolveSat(8, nonIfNodes, io.Inputs1.ToArray(), io.Outputs1.ToArray());
                        }
                        if (s1l != null)
                        //foreach (var s1 in slv1.Solve())
                        {
                            var s1 = s1l.Node0;
                            //if (Ok(s1, io.Inputs1.ToArray(), io.Outputs1.ToArray()))
                            {
                                // branches found, now we have to reconstruct the condition
                                ulong[] condOutputs = new ulong[Outputs.Length];
                                for (int i = 0; i < Inputs.Length; ++i)
                                {
                                    if (io.Inputs0.IndexOf(Inputs[i]) >= 0)
                                    {
                                        condOutputs[i] = 0;
                                    }
                                    else
                                    {
                                        condOutputs[i] = 1;
                                    }
                                }
                                //var slvCond = new AStarSolver(Inputs.ToArray(), condOutputs, 9, ValidNodes);
                                var slvCondL = Program.SolveSat(10, ValidNodes, Inputs.ToArray(), condOutputs);
                                if (slvCondL != null)
                                //foreach (var cond in slvCond.Solve())
                                {
                                    //if (Ok(cond, Inputs, condOutputs))
                                    {
                                        yield return new NodeIf0 { Node0 = slvCondL.Node0, Node1 = s0, Node2 = s1 };
                                    }
                                }
                            }
                        }
                        goto ok;
                    }
                }
            ok:
                Console.WriteLine("{0} done", cnt++);
            }
        }

    }
}
