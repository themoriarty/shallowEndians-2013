﻿namespace Icfpc2013
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using Icfpc2013.Ops;

    using Newtonsoft.Json.Linq;

    public class Program
    {
        #region Methods

        static int Bits(OpTypes ops)
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

        private static void Main(string[] args)
        {
            const int judgesProgramSize = 6;
            const int programSize = judgesProgramSize - 1;
            var options = new string[] { "tfold" };
            var training = API.GetTrainingProblem(new TrainRequest(judgesProgramSize, null));
            var programId = training.id;

            Console.WriteLine("Challenge: {0}", string.Join(", ", training.challenge));

            var operators = training.operators;

            Solve(programId, judgesProgramSize, operators);

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

        public static Lambda1 Solve(int judgesProgramSize, OpTypes validOps, ulong[] inputs, ulong[] outputs)
        {
            int programSize = judgesProgramSize - 1;


            bool tfoldMode = false;
            if ((validOps & OpTypes.tfold) != OpTypes.none)
            {
                tfoldMode = true;
                programSize -= 2;
            }

            var builder = new TreeGenerator(ProgramTree.GetAvailableNodes(validOps, tfoldMode), programSize);
            foreach (var root in builder.GenerateAllPrograms())
            {
                //Console.WriteLine(root.Serialize());
                if (root.Size() == programSize)
                {
                    var solution = root;

                    if (tfoldMode)
                    {
                        solution = new NodeFold { Node0 = new NodeId { Name = "x" }, Node1 = new Node0(), Node2 = new Lambda2 { Id0 = new NodeId { Name = "x1" }, Id1 = new NodeId { Name = "x2" }, Node0 = root } };
                    }

                    //Console.WriteLine(root.Serialize());
                    bool valid = true;
                    for (int i = 0; i < inputs.Length; ++i)
                    {
                        ExecContext ctx = new ExecContext();
                        ctx.Vars["x"] = inputs[i];
                        var output = solution.Eval(ctx);
                        if (output != outputs[i])
                        {
                            valid = false;
                        }
                        //Console.WriteLine("{0} ({1}) = {2:X} ({3})", root.Serialize(), inputs[i], output, valid);
                    }
                    if (valid)
                    {
                        var currentOps = OpTypes.none;
                        solution.Op(ref currentOps);

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
        
        private static void Solve(string programId, int judgesProgramSize, string[] operators)
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

            var solution = Solve(judgesProgramSize, ops, inputs, outputs);
            var finalResult = solution.Serialize();

            Console.WriteLine("Submitting: {0}", finalResult);
            var response = API.Guess(new Guess(programId, finalResult));
            Console.WriteLine("Gues: {0} {1} {2}", response.status, response.message, string.Join(", ", response.values ?? new string[] { }));
        }

        
        #endregion
    }
}