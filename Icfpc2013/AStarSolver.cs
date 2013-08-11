using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icfpc2013.Ops;

namespace Icfpc2013
{
    class AStarSolver
    {
        private ulong[] Inputs;
        private ulong[] Outputs;
        private long MaxSize;
        private List<Node> ValidNodes;

        public AStarSolver(ulong[] inputs, ulong[] outputs, long maxSize, List<Node> validNodes)
        {
            Inputs = inputs;
            Outputs = outputs;
            MaxSize = maxSize;
            ValidNodes = validNodes;
        }

        public delegate BigInteger Measurer(int nth, ulong programResult);

        BigInteger DefaultMeasurer(int i, ulong programResult)
        {
            return BitDistance(programResult, Outputs[i]);
        }

        public BigInteger Distance(Node program, Measurer measurer)
        {
            BigInteger ret = 0;
            for (int i = 0; i < Inputs.Length; ++i)
            {
                var ctx = new ExecContext();
                ctx.Vars["x"] = Inputs[i];
                var output = program.Eval(ctx);
                BigInteger distance = measurer(i, output);
                ret += distance * distance;
            }
            return ret;
        }

        ulong[] GetOutput(Node program)
        {
            ulong[] ret = new ulong[Inputs.Length];
            for (int i = 0; i < Inputs.Length; ++i)
            {
                var ctx = new ExecContext();
                ctx.Vars["x"] = Inputs[i];
                ret[i] = program.Eval(ctx);
            }
            return ret;
        }

        int BitDistance(ulong l1, ulong l2)
        {
            int ret = 0;
            for (int i = 0; i < 64; ++i)
            {
                if ((l1 & 1) != (l2 & 1))
                {
                    ret++;
                }
                l1 >>= 1;
                l2 >>= 1;
            }
            return ret;
        }

        Measurer ChooseMeasurer(Node op, ulong[] currentOutput)
        {
            if (op is NodeOp2Or)
            {
                return (i, programResult) => (BitDistance(programResult | currentOutput[i], Outputs[i]));
            }
            if (op is NodeOp2Xor)
            {
                return (i, programResult) => (BitDistance(programResult ^ currentOutput[i], Outputs[i]));
            }
            if (op is NodeOp2And)
            {
                return (i, programResult) => (BitDistance(programResult & currentOutput[i],  Outputs[i]));
            }
            if (op is NodeOp2Plus)
            {
                return (i, programResult) => (BitDistance(programResult + currentOutput[i], Outputs[i]));
            }
            throw new NotImplementedException();
        }

        private bool DependsOnInput(Node root)
        {
            if (root is NodeId)
            {
                return true;
            }
            if (root is NodeOp1)
            {
                return DependsOnInput((root as NodeOp1).Node0);
            }
            if (root is NodeOp2)
            {
                if (!DependsOnInput((root as NodeOp2).Node0))
                {
                    return DependsOnInput((root as NodeOp2).Node1);
                }
                return false;
            }
            if (root is NodeOp3)
            {
                if (!DependsOnInput((root as NodeOp3).Node0))
                {
                    if (!DependsOnInput((root as NodeOp3).Node1))
                    {
                        return DependsOnInput((root as NodeOp3).Node2);
                    }
                }
                return false;
            }
            return false;
        }

        bool SplitInputsAndOutputs(Node currentProgram, out ulong[] newInputs0, out ulong[] newInputs1, out ulong[] newOutputs0, out ulong[] newOutputs1)
        {
            List<ulong> ni0 = new List<ulong>();
            List<ulong> ni1 = new List<ulong>();
            List<ulong> no0 = new List<ulong>();
            List<ulong> no1 = new List<ulong>();

            for (int i = 0; i < Inputs.Length; ++i)
            {
                var ctx = new ExecContext();
                ctx.Vars["x"] = Inputs[i];
                if (currentProgram.Eval(ctx) == 0)
                {
                    ni0.Add(Inputs[i]);
                    no0.Add(Outputs[i]);
                }
                else
                {
                    ni1.Add(Inputs[i]);
                    no1.Add(Outputs[i]);
                }
            }
            newInputs0 = ni0.ToArray();
            newInputs1 = ni1.ToArray();
            newOutputs0 = no0.ToArray();
            newOutputs1 = no1.ToArray();

            return (newInputs0.Length > 0 && newInputs1.Length > 0);
        }

        IEnumerable<Node> GetAllPossibilities(Node root, long maxSize)
        {
            foreach (var op in ValidNodes)
            {
                if (op is NodeOp1)
                {
                    if (root != null)
                    {
                        var newRoot = op.Clone();
                        (newRoot as NodeOp1).Node0 = root;
                        yield return newRoot;
                    }
                }
                else if (op is NodeOp2)
                {
                    if (root != null)
                    {
                        var measurer = ChooseMeasurer(op, GetOutput(root));
                        if (measurer != null && root.Size() < maxSize - 1)
                        {
                            //Console.WriteLine("decided to do {0}", op.Serialize());
                            var newRoot = op.Clone();
                            (newRoot as NodeOp2).Node0 = root;
                            foreach (var subTree in SolveImpl(null, 0, maxSize - root.Size() - 1, measurer, (BigInteger)(1UL << 63) * (BigInteger)(1UL << 63)))
                            {
                                (newRoot as NodeOp2).Node1 = subTree;
                                yield return newRoot;
                            }
                        }
                    }
                }
                else if (op is NodeIf0)
                {
                    if (root != null && DependsOnInput(root))
                    {
                        ulong[] newInputs0 = null;
                        ulong[] newInputs1 = null;
                        ulong[] newOutputs0 = null;
                        ulong[] newOutputs1 = null;
                        if (SplitInputsAndOutputs(root, out newInputs0, out newInputs1, out newOutputs0, out newOutputs1))
                        {
                            var subSolver0 = new AStarSolver(newInputs0, newOutputs0, maxSize - root.Size() - 1, ValidNodes);
                            foreach (var p0 in subSolver0.Solve())
                            {
                                var subSolver1 = new AStarSolver(newInputs1, newOutputs1, maxSize - root.Size() - p0.Size() - 1, ValidNodes);
                                foreach (var p1 in subSolver1.Solve())
                                {
                                    var ret = op.Clone();
                                    (ret as NodeIf0).Node0 = root;
                                    (ret as NodeIf0).Node1 = p0;
                                    (ret as NodeIf0).Node2 = p1;
                                    yield return ret;
                                    yield break; // solution found
                                }
                            }
                        }
                    }
                }

                else
                {
                    if (root == null)
                    {
                        yield return op.Clone();
                    }
                }
            }
        }
        public IEnumerable<Node> Solve()
        {
            var infinity = (BigInteger)(1UL << 63) * (BigInteger)(1UL << 63);
            foreach (var p in SolveImpl(null, 0, MaxSize, DefaultMeasurer, infinity))
            {
                yield return p;
            }
        }

        private string Indent(long n)
        {
            string ret = "";
            for (int i = 0; i < n; ++i)
            {
                ret = ret + "\t";
            }
            return ret;
        }

        private IEnumerable<Node> SolveImpl(Node root, long currentSize, long maxSize, Measurer measurer, BigInteger prevScore)
        {
            if (currentSize > maxSize)
            {
                yield break;
            }
            var currentDistance = (root == null ? prevScore : Distance(root, measurer));
            var newPrograms = GetAllPossibilities(root, maxSize);
            foreach (var newProgram in newPrograms.OrderBy(x => Distance(x, measurer)))
            {
                currentSize = newProgram.Size();
                //Console.WriteLine("{0}currentSize {1}/{2}: {3}. {4}", Indent(currentSize), currentSize, maxSize, newProgram.Serialize(), Distance(newProgram, measurer));
                var newDistance = Distance(newProgram, measurer);
                if (newDistance == 0)
                {
                    yield return newProgram;
                    yield break;
                }
                if (currentDistance > prevScore && newDistance > currentDistance)
                {
                    // discard
                    yield break;
                }
                if (newProgram.Size() <= maxSize)
                {
                    foreach (var p in SolveImpl(newProgram, currentSize + 1, maxSize, measurer, root == null ? prevScore : Distance(root, measurer)))
                    {
                        if (Distance(p, measurer) == 0)
                        {
                            yield return p;
                            yield break;
                            //Console.WriteLine("A");
                        }
                        yield return p;
                    }
                }
            }
        }
    }
}
