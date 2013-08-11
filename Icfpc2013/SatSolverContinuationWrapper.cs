namespace Icfpc2013
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    using Icfpc2013.Ops;

    public class SatSolverContinuationWrapper : SolverContinuationWrapper
    {
        #region Fields

        private readonly bool tfoldMode;

        private readonly List<Node> validFoldNodes;

        private readonly List<Node> validNodes;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private int programSize;

        private string[] operators;

        private StreamWriter sw;
        private StreamReader sr;

        private Process p;

        #endregion

        #region Constructors and Destructors

        public SatSolverContinuationWrapper(int judgesProgramSize, OpTypes validOps, string[] operators, ulong[] inputs, ulong[] outputs, Func<ulong[], ulong[], Node, bool> filter, Func<Node, Tuple<bool, bool, ulong, ulong>> checker, bool tfoldmode)
            : base(judgesProgramSize, validOps, inputs, outputs, filter, checker)
        {
            this.operators = operators;
            this.tfoldMode = tfoldmode;
        }

        #endregion

        #region Methods

        protected override Tuple<bool,bool, ulong, ulong> CheckerImpl(Node node)
        {
            if (Canceled)
            {
                return new Tuple<bool, bool, ulong, ulong>(false, false, 0, 0);
            }

            return Checker(node);
        }

        protected override IEnumerable<Node> RunImpl()
        {
            Console.WriteLine("SAT START &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
            p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = @"..\..\..\..\SatSolverRunner\bin\x64\Release\SatSolverRunner.exe",
                    Arguments = tfoldMode ? "simple simple simple" : string.Empty,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };

            p.Start();
            sw = p.StandardInput;
            sr = p.StandardOutput;

            var start = Program.GenerateOutput("dummy", JudgesProgramSize, operators, Inputs, Outputs);

            sw.WriteLine(start);

            while (!Canceled)
            {
                string output = sr.ReadLine();

                if (output == null || !output.StartsWith("SOLUTION:"))
                {
                    throw new FormatException("SOLUTION: expected");
                }

                var solution = output.Substring("SOLUTION:".Length);

                if (string.IsNullOrEmpty(solution))
                {
                    Console.WriteLine("NO RESULT");

                    yield break;
                }

                Console.WriteLine("SAT SOLUTION $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$ {0}", solution);

                var node = ProgramTree.Parse(solution);

                yield return node.Program.Node0;
            }

            try
            {
                sw.WriteLine("STATUS:ok");
                p.Close();
            }
            catch (Exception)
            {
            }
        }

        protected override void StopImpl()
        {
            cts.Cancel();
            Canceled = true;

            try
            {
                sw.WriteLine("STATUS:ok");
                p.Close();
            }
            catch (Exception)
            {
            }
        }

        protected override void AddCounterExampleImpl(ulong newInput, ulong newOutput)
        {
            sw.WriteLine("STATUS:mismatch");
            sw.WriteLine("0x{0:X16}", newInput);
            sw.WriteLine("0x{0:X16}", newOutput);
        }

        #endregion
    }
}