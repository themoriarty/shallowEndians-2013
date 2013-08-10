namespace Icfpc2013
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using Icfpc2013.Ops;

    using Newtonsoft.Json.Linq;

    public class Program
    {
        #region Public Methods and Operators

        public static int Bits(OpTypes ops)
        {
            int cnt = 0;
            for (int i = 0; i < 32; ++i)
            {
                if ((ops & (OpTypes)(1 << i)) != 0)
                {
                    cnt++;
                }
            }

            return cnt;
        }

        private static void Main4(string[] args)
        {
            const int judgesProgramSize = 7;
            //var options = new string[] { "tfold" };
            //var training = API.GetTrainingProblem(new TrainRequest(judgesProgramSize, null));
            //var programId = training.id;

            //Console.WriteLine("Challenge: {0}", string.Join(", ", training.challenge));

            //var operators = training.operators;

            Solve("aW7PipK64krdEbU9OxYMoFV1", judgesProgramSize, new string[]{"or",
        "plus",
        "shl1"/*,
        "xor"*/});

            //const int judgesProgramSize = 8;
            //var programId = "9WqCcqFo4tIoJVnBm1OW9gFX";
            //var operators = new string[] { "shr1", "shr4", "tfold" };

            //Solve(programId, judgesProgramSize, operators);

            //var todo = JArray.Parse(File.ReadAllText(@"..\..\..\..\myproblems.json"));

            //foreach (var task in todo)
            //{
            //    //Console.WriteLine("\n");
            //    var solved = (bool?)task["solved"];
            //    var id = (string)task["id"];
            //    var size = (int)task["size"];
            //    var operators = task["operators"].Select(s => (string)s).ToArray();
            //    var ops = ProgramTree.GetOpTypes(operators);

            //    if (solved.HasValue == false && size == 8 && ((ops & (OpTypes.fold | OpTypes.if0)) == OpTypes.none) && (ops & OpTypes.tfold) == OpTypes.tfold)
            //    {

            //        Console.WriteLine("{0} {1} {2}", id, size, ops);

            //        //Solve(id, size, operators);
            //        //Thread.Sleep(20000);
            //    }

            //}
        }

        static IEnumerable<Node> AddOp(Node oldRoot, Node newNode)
        {
            Node ret = newNode.Clone();
            //Node ret = ProgramTree.GetAvailableNodes(op, false)[0];
            if (newNode is NodeOp1)
            {
                (ret as NodeOp1).Node0 = oldRoot; 
                yield return ret;
            } else if (ret is NodeOp2)
            {
                (ret as NodeOp2).Node0 = oldRoot;
                (ret as NodeOp2).Node1 = oldRoot;
                yield return ret;
                (ret as NodeOp2).Node1 = Node0.Instance;
                yield return ret;
                (ret as NodeOp2).Node1 = Node1.Instance;
                yield return ret;
            }
        }

        static IEnumerable<Node> GenerateProgramsOfCorrectSize(long totalSize, long targetSize, Node startPoint, List<Node> validOps)
        {
            if (startPoint.Size() < targetSize)
            {
                //Console.WriteLine("generating {0} for {1}", targetSize, startPoint.Serialize());
                foreach (var op in validOps)
                {
                    foreach (var subProgram in GenerateProgramsOfCorrectSize(totalSize, targetSize - 1, startPoint, validOps))
                    {
                        //Console.WriteLine("\tusing generated {0} for {1}: {2}. op: {3}", targetSize - 1, startPoint.Serialize(), subProgram.Serialize(), op.Serialize());
                        //foreach (var s in AddOp(subProgram, op))
                        //{
                        //    Console.WriteLine("\t\taddedOp: {0}", s.Serialize());
                        //}
                        foreach (var neededProgram in AddOp(subProgram, op).Where(p => p.Size() <= totalSize))
                        {
                            //Console.WriteLine("generated {0} for {1}: {2}", targetSize, startPoint.Serialize(), neededProgram.Serialize());
                            yield return neededProgram;
                        }
                        if (subProgram.Size() >= targetSize - 1)
                        {
                            //Console.WriteLine("generated {0} for {1}: {2}", targetSize, startPoint.Serialize(), subProgram.Serialize());
                            yield return subProgram;
                        }
                    }
                }
            }
            else
            {
                yield return startPoint;
            }
        }

        static IEnumerable<Node> GenerateCorrectPrograms(List<Node> validNodes, int targetSize)
        {

#if true
            var builder = new FTreeGenerator(validNodes, targetSize);
            foreach (var root in builder.GenerateAllPrograms())
            {
                yield return root;
            }
#else

            int bfsSize = targetSize > 5 ? 5 : targetSize;
            var builder = new TreeGenerator(validNodes, bfsSize);
            foreach (var root in builder.GenerateAllPrograms().Where(x => x.Size() >= bfsSize - 1))
            {                
                foreach (var p in GenerateProgramsOfCorrectSize(targetSize, targetSize, root, validNodes))
                {
                    //Console.WriteLine("{0} -> {1}", root.Serialize(), p.Serialize());
                    yield return p;
                }
                //yield break;
            }
#endif
        }

        public static Lambda1 Solve(int judgesProgramSize, OpTypes validOps, ulong[] inputs, ulong[] outputs)
        {
            int programSize = judgesProgramSize - 1;

            bool tfoldMode = false;
            if ((validOps & OpTypes.tfold) != OpTypes.none)
            {
                tfoldMode = true;
                programSize -= 4;
            }

            foreach (var root in GenerateCorrectPrograms(ProgramTree.GetAvailableNodes(validOps, tfoldMode), programSize))
            {
                if (root.Size() == programSize)
                {
                    var solution = root;

                    if (tfoldMode)
                    {
                        solution = new NodeFold { Node0 = new NodeId { Name = "x" }, Node1 = new Node0(), Node2 = new Lambda2 { Id0 = new NodeId { Name = "x1" }, Id1 = new NodeId { Name = "x2" }, Node0 = root } };
                    }

                    bool valid = true;
                    for (int i = 0; i < inputs.Length; ++i)
                    {
                        var ctx = new ExecContext();
                        ctx.Vars["x"] = inputs[i];
                        var output = solution.Eval(ctx);
                        if (output != outputs[i])
                        {
                            valid = false;
                            break;
                        }

                        //Console.WriteLine("{0} ({1}) = {2:X} ({3})", root.Serialize(), inputs[i], output, valid);
                    }

                    //Console.WriteLine("{0}", root.Serialize());

                    if (valid)
                    {
                        var currentOps = OpTypes.none;
                        solution.Op(ref currentOps);

                        //Console.WriteLine("currentOps={0}", currentOps);

                        if (tfoldMode)
                        {
                            currentOps &= ~OpTypes.fold;
                            currentOps |= OpTypes.tfold;
                        }

                        if (currentOps == validOps)
                        {
                            var finalResult = new Lambda1 { Id0 = new NodeId { Name = "x" }, Node0 = solution };
                            return finalResult;
                        }
                    }
                }
            }

            return null;
        }

        public static void SolveMyProblems()
        {
            var todo = JArray.Parse(File.ReadAllText(@"..\..\..\..\myproblems.json"));

            foreach (var task in todo)
            {
                var solved = (bool?)task["solved"];
                var id = (string)task["id"];
                var size = (int)task["size"];
                var operators = task["operators"].Select(s => (string)s).ToArray();
                var ops = ProgramTree.GetOpTypes(operators);

                if (solved.HasValue == false && size < 12 && ((ops & (OpTypes.fold | OpTypes.if0)) == OpTypes.none) /*&& (ops & OpTypes.tfold) == OpTypes.tfold*/ /* && Bits(ops) == 3*/)
                {
                    Console.WriteLine("{0} {1} {2}", id, size, ops);

                    //Solve(id, size, operators);
                    //Thread.Sleep(20000);
                }
            }
        }

        public static void SolveOffline()
        {
            const int judgesProgramSize = 11;

            var programId = "7eegF4nrTKkUz0fkrdnBwPvz";
            var operators = new[] { "if0",
        "plus",
        "shr16",
        "shr4",
        "xor"};
            
            ulong[] inputs = { 0x0000000000000000, 0xFFFFFFFFFFFFFFFF, 0x0580A1AB9001056D, 0x17FBEACECACE0709, 0x6E8E0097C68096A7, 0x50475BF54101FEE4, 0x880A7F62D368B805, 0xFDCFD29F7AE5550B };
            ulong[] outputs = { 0x0000000000000000,
        0x0000FFFFFFFFFFFF,
        0x00000580A1AB9001,
        0x000017FBEACECACE,
        0x00006E8E0097C680,
        0x000050475BF54101,
        0x0000880A7F62D368,
        0x0000FDCFD29F7AE5 };

            Console.WriteLine("ProgramId: {0}", programId);
            Console.WriteLine("Training: {0}", string.Join(", ", operators));

            var ops = ProgramTree.GetOpTypes(operators);

            
            var solution = Solve(judgesProgramSize, ops, inputs, outputs);
            var finalResult = solution != null ? solution.Serialize() : "NO RESULT";

            Console.WriteLine(finalResult);
        }

        public static bool SolveTrainingProgram()
        {
            int judgesProgramSize = 8;
            var options = new[] { "tfold" };
            options = null;
            var training = API.GetTrainingProblem(new TrainRequest(judgesProgramSize, options));
            var programId = training.id;

            Console.WriteLine("Challenge: {0}", string.Join(", ", training.challenge));

            var operators = training.operators;

            return Solve(programId, judgesProgramSize, operators);
        }

        #endregion

        #region Methods

        private static void Main(string[] args)
        {
            //SolveTrainingProgram();
            //SolveMyProblems();
            SolveOffline();
        }
        
        private static bool Solve(string programId, int judgesProgramSize, string[] operators)
        {
            Console.WriteLine("ProgramId: {0}", programId);
            Console.WriteLine("Training: {0}", string.Join(", ", operators));

            var ops = ProgramTree.GetOpTypes(operators);


            ulong[] inputs = ProgramTree.GetInputVectorList(8).ToArray(); //{0x12, 0x137};
            var inputStrings = inputs.Select(s => string.Format("0x{0:X16}", s)).ToArray();

            Console.WriteLine("Input: {{{0}}}", string.Join(", ", inputStrings));

            var outputsResponse = API.Eval(new EvalRequest(programId, null, inputStrings));

            if (outputsResponse.status != "ok" || outputsResponse.outputs == null)
            {
                throw new Exception("eval failed");
            }

            Console.WriteLine("Output: {{{0}}}", string.Join(", ", outputsResponse.outputs));

            ulong[] outputs = outputsResponse.outputs.Select(s => ulong.Parse(s.Replace("0x", string.Empty), NumberStyles.HexNumber)).ToArray();

            //ulong[] inputs = { 0x0000000000000000, 0xFFFFFFFFFFFFFFFF, 0x4E5B3679C799A739, 0x4EEBF16E2198469F, 0xA986C998F78B9A65, 0xA2C57077E8DDF691, 0x3C6E1A9F8F3F3625, 0xE526A4724D1CFCBD };
            //ulong[] outputs = { 0x0000000000000000, 0x0000000000000007, 0x0000000000000002, 0x0000000000000002, 0x0000000000000005, 0x0000000000000005, 0x0000000000000001, 0x0000000000000007 };

            //ulong[] inputs = { 0x0000000000000000, 0xFFFFFFFFFFFFFFFF, 0x143365BE8C18E891, 0x8695A9C52208381A, 0xCE45128B104DD1FC, 0x760442CEB4894690, 0xBBE30C4F1CC4FB4E, 0x755333D90B930A73};
            //ulong[] outputs = { 0x0000000000000002, 0xFFFFFFFFFFFFFFFD, 0x3C9A313BA44AB9B3, 0x93C0FD4F6618A850, 0x6ACF37A130E975F6, 0x620CC86C1D9BD3B2, 0x33A924ED564EF1EC, 0x5FF99B8B22B91F59};

            var solution = Solve(judgesProgramSize, ops, inputs, outputs);
            var finalResult = solution.Serialize();

            Console.WriteLine("Submitting: {0}", finalResult);
            var response = API.Guess(new Guess(programId, finalResult));
            Console.WriteLine("Gues: {0} {1} {2}", response.status, response.message, string.Join(", ", response.values ?? new string[] { }));

            return response.status == "win";
        }

        #endregion
    }
}