﻿namespace Icfpc2013
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

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

        static IEnumerable<Node> GenerateCorrectPrograms(List<Node> validNodes, int targetSize, CancellationToken token)
        {

#if true
            var builder = new PTreeGeneratorContainer(validNodes, targetSize);
            foreach (var root in builder.GenerateAllPrograms(token))
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

            var tfoldMode = (validOps & OpTypes.tfold) != OpTypes.none;

            List<ArgNode> nodes = new List<ArgNode>();
            int size = programSize;
            if (tfoldMode)
            {
                size = programSize - 4;
            }
            for (int i = 0; i < size; ++i)
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

            if (tfoldMode)
            {
                permitted[(int)OpCodes.Input2] = true;
            }

            List<ArgNode> res;
            if (tfoldMode)
            {
                TreeStructure.UseFwLinks = false;
                res = SATGeneratation.Utils.SolveTFoldArray(inputs, outputs, nodes, permitted);
            }
            else
            {
                res = SATGeneratation.Utils.SolveNodeArray(inputs, outputs, nodes, permitted);
            }
            Console.Write("[Node array] And example output :::");
            for (int i = 0; i < res.Count; i++)
            {
                Console.Write(res[i].ComputedOpcode + " ");
            }
            Console.Write("\n");

            int pos = 0;
            var rootNode = BuildFromSat(res, ref pos, tfoldMode);

            if (pos != res.Count)
            {
                //throw new Exception("pos != end");
            }

            if (tfoldMode)
            {
                rootNode = new NodeFold { Node0 = new NodeId { Name = "x" }, Node1 = new Node0(), Node2 = new Lambda2 { Id0 = new NodeId { Name = "x1" }, Id1 = new NodeId { Name = "x2" }, Node0 = rootNode } };
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

        public static Node BuildFromSat(List<ArgNode> solution, ref int index, bool tfoldMode)
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
                    return new NodeId() {Name = tfoldMode ? "x1" : "x"};
                    break;
                case OpCodes.Input2:
                    return new NodeId() { Name = "x2" };
                case OpCodes.And:
                    node1 = BuildFromSat(solution, ref index, tfoldMode);
                    node2 = BuildFromSat(solution, ref index, tfoldMode);
                    return new NodeOp2And{Node0 = node1, Node1 = node2};
                    break;
                case OpCodes.Or:
                    node1 = BuildFromSat(solution, ref index, tfoldMode);
                    node2 = BuildFromSat(solution, ref index, tfoldMode);
                    return new NodeOp2Or{Node0 = node1, Node1 = node2};
                    break;
                case OpCodes.Xor:
                    node1 = BuildFromSat(solution, ref index, tfoldMode);
                    node2 = BuildFromSat(solution, ref index, tfoldMode);
                    return new NodeOp2Xor{Node0 = node1, Node1 = node2};
                    break;
                case OpCodes.Plus:
                    node1 = BuildFromSat(solution, ref index, tfoldMode);
                    node2 = BuildFromSat(solution, ref index, tfoldMode);
                    return new NodeOp2Plus{Node0 = node1, Node1 = node2};
                    break;
                case OpCodes.Not:
                    node1 = BuildFromSat(solution, ref index, tfoldMode);
                    return new NodeOp1Not { Node0 = node1 };
                    break;
                case OpCodes.Shl1:
                    node1 = BuildFromSat(solution, ref index, tfoldMode);
                    return new NodeOp1Shl1 { Node0 = node1 };
                    break;
                case OpCodes.Shr1:
                    node1 = BuildFromSat(solution, ref index, tfoldMode);
                    return new NodeOp1Shr1 { Node0 = node1 };
                case OpCodes.Shr4:
                    node1 = BuildFromSat(solution, ref index, tfoldMode);
                    return new NodeOp1Shr4 { Node0 = node1 };
                case OpCodes.Shr16:
                    node1 = BuildFromSat(solution, ref index, tfoldMode);
                    return new NodeOp1Shr16 { Node0 = node1 };
                case OpCodes.If0:
                    node1 = BuildFromSat(solution, ref index, tfoldMode);
                    node2 = BuildFromSat(solution, ref index, tfoldMode);
                    node3 = BuildFromSat(solution, ref index, tfoldMode);
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

            CancellationTokenSource cts = new CancellationTokenSource();

            foreach (var root in GenerateCorrectPrograms(ProgramTree.GetAvailableNodes(validOps, tfoldMode), programSize, cts.Token))
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
                            cts.Cancel();

                            var finalResult = new Lambda1 { Id0 = new NodeId { Name = "x" }, Node0 = solution };
                            return finalResult;
                        }
                    }
                }
            }

            cts.Cancel();

            return null;
        }

        public static void SolveMyProblems()
        {
            var todo = JArray.Parse(File.ReadAllText(@"..\..\..\..\myproblems.json"));

            int cnt = 0;
            int cntSolved = 0;
            foreach (var task in todo)
            {
                var solved = (bool?)task["solved"];
                var id = (string)task["id"];
                var size = (int)task["size"];
                var operators = task["operators"].Select(s => (string)s).ToArray();
                var ops = ProgramTree.GetOpTypes(operators);

                //if (solved.HasValue == false && size == 14 && ((ops & (OpTypes.fold | OpTypes.bonus /*| OpTypes.if0*/)) == OpTypes.none) && (ops & OpTypes.tfold) == OpTypes.none /* && Bits(ops) == 3*/)
                //if (solved.HasValue == false && size == 12)
                if (solved.HasValue == false && size < 15 && ((ops & (OpTypes.fold | OpTypes.bonus /*| OpTypes.if0*/)) == OpTypes.none) && (ops & OpTypes.tfold) == OpTypes.none/* && Bits(ops) == 5*/)
                //if (id == "Jb6H9d6n4E9QUCnBGdMwDfQx")
                //if (solved.HasValue == true && solved.Value == false && size < 12)
                {
                    Console.WriteLine("{0} {1} {2}", id, size, ops);

                    //if (Solve(id, size, operators, true))
                    //{
                    //    ++cntSolved;
                    //}
                    //Thread.Sleep(20000);

                    ++cnt;
                }
            }

            Console.WriteLine("cnt={0} cntSolved={1}", cnt, cntSolved);
        }

        public static void SolveOffline()
        {
            const int judgesProgramSize = 13;

            var programId = "D9lVlxOdxauIiafOLkDS9om6";
            var operators = new[] { "if0", "tfold", "shr4", "shr16", "or" };

            ulong[] inputs = { 0x0000000000000000, 0xFFFFFFFFFFFFFFFF, 0x8B8DCC5011923FDE, 0x22006E622C672963, 0x4801A2390D5DCD16, 0xC9F88E422EC10F4C, 0x3CB96DDBC67E6503, 0x8041DA9DBD03076B, 0x3C68DE0EADC71541, 0xD4495F0A40C44A3D, 0xA9473AF0FC289D30, 0x72AA449E32A22113, 0x25BB4BC62CEC41CE, 0x0C3278A9908842FD, 0x079044B1008C2FBE, 0x0700000000000000 };
            ulong[] outputs = { 0x0000000000000000, 0x00000000000000FF, 0x000000000000008B, 0x0000000000000022, 0x0000000000000048, 0x00000000000000C9, 0x000000000000003C, 0x0000000000000080, 0x000000000000003C, 0x00000000000000D4, 0x00000000000000A9, 0x0000000000000072, 0x0000000000000025, 0x000000000000000C, 0x0000000000000007, 0x0000000000000000 };

            Console.WriteLine("ProgramId: {0}", programId);
            Console.WriteLine("Training: {0}", string.Join(", ", operators));

            Console.WriteLine("Input: {{{0}}}", string.Join(", ", inputs.Select(s => string.Format("0x{0:X16}", s)).ToArray()));
            Console.WriteLine("Output: {{{0}}}", string.Join(", ", outputs.Select(s => string.Format("0x{0:X16}", s)).ToArray()));


            var ops = ProgramTree.GetOpTypes(operators);

            var solution = Solve(judgesProgramSize, ops, inputs, outputs);
            var finalResult = solution != null ? solution.Serialize() : "NO RESULT";

            Console.WriteLine(finalResult);
        }

        public static void SolveSatOffline()
        {
            const int judgesProgramSize = 13;

            var programId = "6nE6n1FvE1QtnmOeriZCqv6O";
            var operators = new[] { "if0", "shr1", "shr4", "shr16", "plus" };

            // Solution: (lambda (x) (xor x (xor x (plus x x))))
            ulong[] inputs = { 0x0000000000000000, 0xFFFFFFFFFFFFFFFF, 0x097E6E055D07F036, 0xA30604E66793F909, 0x000000000001F8EC, 0x000000000003F4C2 };
            ulong[] outputs = { 0x0000000000000000, 0x00FFFFFFFFFFFFFF, 0x00097E6E055D07F0, 0x00A30604E66793F9, 0x0000000000000218, 0x00000000000003F4 };
            Console.WriteLine("ProgramId: {0}", programId);
            Console.WriteLine("Training: {0}", string.Join(", ", operators));

            var ops = ProgramTree.GetOpTypes(operators);

            var startTime = DateTime.Now;
            //TreeStructure.UseFwLinks = false;
            var solution = SolveSat(judgesProgramSize, ops, inputs, outputs);

            var finalResult = solution != null ? solution.Serialize() : "NO RESULT";

            Console.WriteLine("Result: {0}, execution time: {1}", finalResult, DateTime.Now - startTime);
        }

        public static bool SolveTrainingProgram(bool useSat)
        {
            int judgesProgramSize = 11;
            var options = new[] { "tfold" };
            //options = new string[0];
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

            ulong[] inputs = ProgramTree.GetInputVectorList(15).ToArray(); //{0x12, 0x137};

            if (useSat)
            {
                Console.WriteLine("Reducing number of inputs...");
                inputs = inputs.Take(4).ToArray();
            }

            var inputStrings = inputs.Select(s => string.Format("0x{0:X16}", s)).ToArray();

            Console.WriteLine("Input: {{{0}}}", string.Join(", ", inputStrings));

            var lastrequest = Stopwatch.StartNew();
            var outputsResponse = API.Eval(new EvalRequest(programId, null, inputStrings));

            if (outputsResponse.status != "ok" || outputsResponse.outputs == null)
            {
                throw new Exception("eval failed");
            }

            Console.WriteLine("Output: {{{0}}}", string.Join(", ", outputsResponse.outputs));

            ulong[] outputs = outputsResponse.outputs.Select(s => ulong.Parse(s.Replace("0x", string.Empty), NumberStyles.HexNumber)).ToArray();

            //var inputs = new ulong[] { 0x0000000000000000, 0xFFFFFFFFFFFFFFFF, 0xD8E4755D6F460C1A, 0xC8DB19F5D56567AD, 0x0085F8373B347C2B, 0x0DB3935300645EEC, 0x0F6C74CF7529404A, 0x4BEB041E4DC2BEF4 };
            //var outputs = new ulong[] { 0x0000000000000000, 0x0000FFFFFFFFFFFF, 0x0000D8E4755D6F46, 0x0000C8DB19F5D565, 0x00000085F8373B34, 0x00000DB393530064, 0x00000F6C74CF7529, 0x00004BEB041E4DC2 };
                
            Lambda1 solution = null;

            var sw = Stopwatch.StartNew();

            try
            {


                if (useSat)
                {
                    try
                    {
                        Console.WriteLine("Using SAT!");

                        if ((ops & OpTypes.if0) != OpTypes.none || judgesProgramSize > 0)
                        {
                            Console.WriteLine("Using SAT only!");
                            solution = SolveSat(judgesProgramSize, ops, inputs, outputs);
                        }
                        else
                        {

                            Lambda1 solution1 = null;
                            Lambda1 solution2 = null;

                            var task1 = Task.Run(() => solution1 = SolveSat(judgesProgramSize, ops, inputs, outputs));
                            var task2 = Task.Run(
                                () =>
                                    {
                                        try
                                        {
                                            solution2 = SolveSat(judgesProgramSize, ops, inputs, outputs);
                                        }
                                        catch (Exception exbfs)
                                        {
                                            Console.WriteLine("BFS failed: {0}", exbfs);
                                            throw;
                                        }

                                    });

                            Task.WaitAny(task1, task2);

                            if (solution1 == null && solution2 == null)
                            {
                                Task.WaitAll(task1, task2);
                            }

                            if (solution1 != null)
                            {
                                Console.WriteLine("SAT was faster");
                                solution = solution1;
                            }

                            if (solution2 != null)
                            {
                                Console.WriteLine("BFS was faster");
                                solution = solution2;
                            }
                        }
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
            }
            finally
            {
                Console.WriteLine("* computation completed in {0}", sw.ElapsedMilliseconds / 1000);
            }

            var finalResult = solution.Serialize();

            Console.WriteLine("Submitting: {0}", finalResult);
            var response = API.Guess(new Guess(programId, finalResult));
            Console.WriteLine("Gues: {0} {1} {2}", response.status, response.message, string.Join(", ", response.values ?? new string[] { }));

            if (response.status == "mismatch")
            {
                lastrequest.Restart();

                while (response.status == "mismatch")
                {
                    Console.WriteLine("MISMATCH!!!");
                    var newInput = ulong.Parse(response.values[0].Replace("0x", string.Empty), NumberStyles.HexNumber);
                    var newOutput = ulong.Parse(response.values[1].Replace("0x", string.Empty), NumberStyles.HexNumber);

                    var newInputs = new List<ulong>(inputs);
                    newInputs.Add(newInput);
                    inputs = newInputs.ToArray();

                    var newOutputs = new List<ulong>(outputs);
                    newOutputs.Add(newOutput);
                    outputs = newOutputs.ToArray();
                    
                    Console.WriteLine("New Input: {{{0}}}", string.Join(", ", inputs.Select(s => string.Format("0x{0:X16}", s)).ToArray()));
                    Console.WriteLine("New Output: {{{0}}}", string.Join(", ", outputs.Select(s => string.Format("0x{0:X16}", s)).ToArray()));

                    sw.Restart();
                    if (useSat)
                    {
                        solution = SolveSat(judgesProgramSize, ops, inputs, outputs);
                    }
                    else
                    {
                        solution = Solve(judgesProgramSize, ops, inputs, outputs);
                    }
                    Console.WriteLine("* computation completed in {0}", sw.ElapsedMilliseconds / 1000);

                    finalResult = solution.Serialize();

                    var elapsed = lastrequest.ElapsedMilliseconds;

                    if (elapsed < 10000)
                    {
                        var sleepSecs = 10000 - (int)elapsed;
                        Console.WriteLine("Wait for {0} msec before resubmitting result.", sleepSecs);
                        Thread.Sleep(sleepSecs);
                    }

                    Console.WriteLine("Submitting: {0}", finalResult);
                    response = API.Guess(new Guess(programId, finalResult));
                    Console.WriteLine("Gues: {0} {1} {2}", response.status, response.message, string.Join(", ", response.values ?? new string[] { }));

                    lastrequest.Restart();
                }
            }

            return response.status == "win";
        }

        #endregion
    }
}