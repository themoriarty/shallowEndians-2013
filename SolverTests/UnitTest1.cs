using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icfpc2013;

using Newtonsoft.Json.Linq;

namespace SolverTests
{
    [TestClass]
    public class UnitTest1
    {
        private ulong[] inputs = { 0, 12, 137, 1337, 0xFFFFFFFFFFFFFFFF, 0x143365BE8C18E891, 0x8695A9C52208381A, 0xCE45128B104DD1FC, 0x760442CEB4894690, 0xBBE30C4F1CC4FB4E, 0x755333D90B930A73 };
        [TestMethod]
        public void TestAlreadySubmittedSolutions()
        {
            var problems = JArray.Parse(File.ReadAllText(@"..\..\..\problems.json"));
            int solvedProblems = 0;
            foreach (var problem in problems.Where(p => p["solved"] != null && (bool)p["solved"] == true && p["inputs"] != null))
            {
                solvedProblems++;
                int judgesProgramSize = (int)problem["size"];
                var validOps = Icfpc2013.ProgramTree.GetOpTypes(problem["operators"].Select(s => (string)s).ToArray());
                ulong[] inputs = problem["inputs"].Select(s => ulong.Parse("0" + ((string)s).TrimStart(new char[]{'0', 'x'}), System.Globalization.NumberStyles.AllowHexSpecifier)).ToArray();
                ulong[] outputs = problem["outputs"].Select(s => ulong.Parse("0" + ((string)s).TrimStart(new char[] { '0', 'x' }), System.Globalization.NumberStyles.AllowHexSpecifier)).ToArray();

                //Solve(int judgesProgramSize, OpTypes validOps, ulong[] inputs, ulong[] outputs)
                var solution = Icfpc2013.Program.Solve(judgesProgramSize, validOps, inputs, outputs);
                var idealSolution = Icfpc2013.ProgramTree.Parse((string)problem["program"]);

                foreach (var input in inputs)
                {
                    var ctx = new Icfpc2013.ExecContext();
                    ctx.Vars["x"] = input;
                    var solutionOutput = solution.Node0.Eval(ctx);
                    var idealOutput = idealSolution.Run(input);
                    Assert.AreEqual(idealOutput, solutionOutput);
                }
            }
            Console.WriteLine("Total problems that claim to be solved: {0}", solvedProblems);
        }
    }
}
