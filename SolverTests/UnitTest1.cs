using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;

namespace SolverTests
{
    [TestClass]
    public class UnitTest1
    {
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
                Assert.AreEqual((string)problem["program"], solution.Serialize(), "id="+(string)problem["id"]);
            }
            Console.WriteLine("Total problems that claim to be solved: {0}", solvedProblems);
        }
    }
}
