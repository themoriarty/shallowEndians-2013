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

    using SATGeneratation;

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
            const int programSize = judgesProgramSize - 1;
            //var options = new string[] { "tfold" };
            //var training = API.GetTrainingProblem(new TrainRequest(judgesProgramSize, null));
            //var programId = training.id;

            //Console.WriteLine("Challenge: {0}", string.Join(", ", training.challenge));

            //var operators = training.operators;

            Solve("aW7PipK64krdEbU9OxYMoFV1", judgesProgramSize, new string[]{"or",
        "plus",
        "shl1"/*,
        "xor"*/}, false);

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

        public static Lambda1 SolveSat(int judgesProgramSize, OpTypes validOps, ulong[] inputs, ulong[] outputs)
        {
            int programSize = judgesProgramSize - 1;

            List<ArgNode> nodes = new List<ArgNode>();

            for (int i = 0; i < programSize; ++i)
            {
                nodes.Add(new MetaArgNode { Name = string.Format("n{0}",i) });
            }

            bool[] permitted = new bool[Enum.GetValues(typeof(OpCodes)).Length];
            for (int i = 0; i < (int)OpCodes.Input + 1; ++i)
            {
                permitted[i] = false;
            }

            permitted[(int)OpCodes.One] = true;
            permitted[(int)OpCodes.Zero] = true;
            permitted[(int)OpCodes.Input] = true;

            if ((validOps & OpTypes.not) != OpTypes.none)
            {
                permitted[(int)OpCodes.Not] = true;
            }

            if ((validOps & OpTypes.and) != OpTypes.none)
            {
                permitted[(int)OpCodes.And] = true;
            }

            if ((validOps & OpTypes.or) != OpTypes.none)
            {
                permitted[(int)OpCodes.Or] = true;
            }

            if ((validOps & OpTypes.xor) != OpTypes.none)
            {
                permitted[(int)OpCodes.Xor] = true;
            }

            if ((validOps & OpTypes.plus) != OpTypes.none)
            {
                permitted[(int)OpCodes.Plus] = true;
            }

            if ((validOps & OpTypes.shl1) != OpTypes.none)
            {
                permitted[(int)OpCodes.Shl1] = true;
            }

            if ((validOps & OpTypes.shr1) != OpTypes.none)
            {
                permitted[(int)OpCodes.Shr1] = true;
            }

            if ((validOps & OpTypes.shr4) != OpTypes.none)
            {
                permitted[(int)OpCodes.Shr4] = true;
            }

            if ((validOps & OpTypes.shr16) != OpTypes.none)
            {
                permitted[(int)OpCodes.Shr16] = true;
            }

            if ((validOps & OpTypes.if0) != OpTypes.none)
            {
                permitted[(int)OpCodes.If0] = true;
            }

            List<ArgNode> res = SATGeneratation.Utils.SolveNodeArray(inputs, outputs, nodes, permitted);
            Console.Write("[Node array] And example output :::");
            for (int i = 0; i < res.Count; i++)
            {
                Console.Write(res[i].ComputedOpcode + " ");
            }
            Console.Write("\n");

            int pos = 0;
            var rootNode = BuildFromSat(res, ref pos);

            if (pos != res.Count)
            {
                throw new Exception("pos != end");
            }

            var result = new ProgramTree { Program = new Lambda1 { Node0 = rootNode, Id0 = new NodeId { Name = "x" } } };

            Console.WriteLine("SAT: {0}", rootNode.Serialize());

            for (int i = 0; i < inputs.Length; ++i)
            {
                var input = inputs[i];
                var output = outputs[i];

                var computed = result.Run(input);

                Console.WriteLine("[{0}] {1:X16} {2:X16} {3:X16}", i, input, output, computed);

                if (output != computed)
                {
                    throw new Exception("mismatch");
                }
            }

                return result.Program;
        }

        public static Node BuildFromSat(List<ArgNode> solution, ref int index)
        {
            var arg = solution[index++];
            Node node1;
            Node node2;
            Node node3;
            switch (arg.ComputedOpcode)
            {
                case OpCodes.Zero:
                    return new Node0();
                    break;
                case OpCodes.One:
                    return new Node1();
                    break;
                case OpCodes.Input:
                    return new NodeId() {Name = "x"};
                    break;
                case OpCodes.And:
                    node1 = BuildFromSat(solution, ref index);
                    node2 = BuildFromSat(solution, ref index);
                    return new NodeOp2And{Node0 = node1, Node1 = node2};
                    break;
                case OpCodes.Or:
                    node1 = BuildFromSat(solution, ref index);
                    node2 = BuildFromSat(solution, ref index);
                    return new NodeOp2Or{Node0 = node1, Node1 = node2};
                    break;
                case OpCodes.Xor:
                    node1 = BuildFromSat(solution, ref index);
                    node2 = BuildFromSat(solution, ref index);
                    return new NodeOp2Xor{Node0 = node1, Node1 = node2};
                    break;
                case OpCodes.Plus:
                    node1 = BuildFromSat(solution, ref index);
                    node2 = BuildFromSat(solution, ref index);
                    return new NodeOp2Plus{Node0 = node1, Node1 = node2};
                    break;
                case OpCodes.Not:
                    node1 = BuildFromSat(solution, ref index);
                    return new NodeOp1Not { Node0 = node1 };
                    break;
                case OpCodes.Shl1:
                    node1 = BuildFromSat(solution, ref index);
                    return new NodeOp1Shl1 { Node0 = node1 };
                    break;
                case OpCodes.Shr1:
                    node1 = BuildFromSat(solution, ref index);
                    return new NodeOp1Shr1 { Node0 = node1 };
                case OpCodes.Shr4:
                    node1 = BuildFromSat(solution, ref index);
                    return new NodeOp1Shr4 { Node0 = node1 };
                case OpCodes.Shr16:
                    node1 = BuildFromSat(solution, ref index);
                    return new NodeOp1Shr16 { Node0 = node1 };
                case OpCodes.If0:
                    node1 = BuildFromSat(solution, ref index);
                    node2 = BuildFromSat(solution, ref index);
                    node3 = BuildFromSat(solution, ref index);
                    return new NodeIf0() { Node0 = node1, Node1 = node2, Node2 = node3 };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
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
            const int judgesProgramSize = 8;
            
            var programId = "mrkVrFLMXiwAZifmVWtSBCxn";
            var operators = new[] { "not","shl1","shr1","shr4","or" };
            
            ulong[] inputs = { 0x0000000000000000, 0xFFFFFFFFFFFFFFFF, 0x0580A1AB9001056D, 0x17FBEACECACE0709, 0x6E8E0097C68096A7, 0x50475BF54101FEE4, 0x880A7F62D368B805, 0xFDCFD29F7AE5550B };
            ulong[] outputs = { 0x0FFFFFFFFFFFFFFE, 0xFFFFFFFFFFFFFFFF, 0x0FFFFFFFFFFFFFFF, 0x1FFFFFFFFFFFFFFF, 0x6FFFFFFFFFFFFFFF, 0x5FFFFFFFFFFFFFFE, 0x8FFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF };

            Console.WriteLine("ProgramId: {0}", programId);
            Console.WriteLine("Training: {0}", string.Join(", ", operators));

            var ops = ProgramTree.GetOpTypes(operators);

            
            var solution = Solve(judgesProgramSize, ops, inputs, outputs);
            var finalResult = solution != null ? solution.Serialize() : "NO RESULT";

            Console.WriteLine(finalResult);
        }

        public static void SolveSatOffline()
        {
            const int judgesProgramSize = 8;

            var programId = "GBUTtMTIADxBecgU35jl63us";
            var operators = new[] { "plus", "xor" };

            // Solution: (lambda (x) (xor x (xor x (plus x x))))

            ulong[] inputs = { 0x0000000000000000, 0xFFFFFFFFFFFFFFFF, 0x23A282379AF7850C, 0xF3D35174C949BB0D, 0xFE7C0264DF27E86F, 0x06CC691C9D9CD006, 0xE809CD0767D69590, 0x736D7A70B0B2534C };
            ulong[] outputs = { 0x0000000000000000, 0xFFFFFFFFFFFFFFFE, 0x4745046F35EF0A18, 0xE7A6A2E99293761A, 0xFCF804C9BE4FD0DE, 0x0D98D2393B39A00C, 0xD0139A0ECFAD2B20, 0xE6DAF4E16164A698 };

            Console.WriteLine("ProgramId: {0}", programId);
            Console.WriteLine("Training: {0}", string.Join(", ", operators));

            var ops = ProgramTree.GetOpTypes(operators);


            var solution = SolveSat(judgesProgramSize, ops, inputs, outputs);
            var finalResult = solution != null ? solution.Serialize() : "NO RESULT";

            Console.WriteLine(finalResult);
        }

        public static bool SolveTrainingProgram(bool useSat)
        {
            int judgesProgramSize = 12;
            var options = new[] { "tfold" };
            options = new string[0];
            var training = API.GetTrainingProblem(new TrainRequest(judgesProgramSize, options));
            var programId = training.id;

            Console.WriteLine("Challenge: {0}", string.Join(", ", training.challenge));

            var operators = training.operators;

            return Solve(programId, judgesProgramSize, operators, useSat);
        }

        #endregion

        #region Methods

        private static void Main(string[] args)
        {
            SolveTrainingProgram(true);
            //SolveMyProblems();
            //SolveOffline();
            //SolveSatOffline();
        }
        
        private static bool Solve(string programId, int judgesProgramSize, string[] operators, bool useSat)
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

            Lambda1 solution = null;
            
            if (useSat)
            {
                try
                {
                    Console.WriteLine("Using SAT!");
                    solution = SolveSat(judgesProgramSize, ops, inputs, outputs);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("SAT faild: {0}", ex);

                    Console.WriteLine("Using BFS!");
                    solution = Solve(judgesProgramSize, ops, inputs, outputs);    
                }
            }
            else
            {
                Console.WriteLine("Using BFS!");
                solution = Solve(judgesProgramSize, ops, inputs, outputs);    
            }
            
            var finalResult = solution.Serialize();

            Console.WriteLine("Submitting: {0}", finalResult);
            var response = API.Guess(new Guess(programId, finalResult));
            Console.WriteLine("Gues: {0} {1} {2}", response.status, response.message, string.Join(", ", response.values ?? new string[] { }));

            return response.status == "win";
        }

        #endregion
    }
}