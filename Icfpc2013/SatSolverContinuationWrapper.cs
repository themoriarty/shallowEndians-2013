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

        #endregion

        #region Constructors and Destructors

        public SatSolverContinuationWrapper(int judgesProgramSize, OpTypes validOps, string[] operators, ulong[] inputs, ulong[] outputs, Func<ulong[], ulong[], Node, bool> filter, Func<Node, Tuple<bool, bool, ulong, ulong>> checker)
            : base(judgesProgramSize, validOps, inputs, outputs, filter, checker)
        {
            this.operators = operators;
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
            Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = @"..\..\..\..\SatSolverRunner\bin\x64\Release\SatSolverRunner.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };

            p.Start();
            StreamWriter sw = p.StandardInput;
            StreamReader sr = p.StandardOutput;

            var start = Program.GenerateOutput("dummy", JudgesProgramSize, operators, Inputs, Outputs);

            sw.WriteLine(start);

            string output = sr.ReadLine();
            Console.WriteLine("output=<<" + output + ">>");
            
            yield break;

            //p.WaitForExit();
            //p.Close();


        }

        protected override void StopImpl()
        {
            cts.Cancel();
            Canceled = true;
        }

        #endregion
    }
}