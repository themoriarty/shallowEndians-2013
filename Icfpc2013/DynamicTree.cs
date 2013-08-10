using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Icfpc2013.Ops;

namespace Icfpc2013
{
    /// <summary>
    /// sample code, not tested 
    /// </summary>
    public class DynamicTree
    {
        public const int MaxSize = 10;
        public const int MaxNodes = 4;

        public List<ProgramItem>[,] Prgs = new List<ProgramItem>[MaxSize + 1, MaxNodes];
        public OpTypes[] ValidNodes = { OpTypes.not, OpTypes.or, OpTypes.plus, OpTypes.xor };

        public class ProgramItem
        {
            public string Code { get; set; }
            public List<ulong> Output { get; set; }
        }

        public void PrecomputePrograms(int size) 
        {
            // set initial values - 1,0,x 
            var init = new List<ProgramItem>
                {
                    new ProgramItem { Output = new List<ulong>()/* =input */, Code = new Node0().ToString()},
                    new ProgramItem { Output = new List<ulong>()/* =input */, Code = new Node1().ToString()},
                    new ProgramItem { Output = new List<ulong>()/* =input */, Code = new NodeId {Name = "x"}.ToString()},
                };
            for (int n = 0; n < MaxNodes; n++)
                Prgs[0, n] = init;
            

            for (int s = 1; s <= size; s++)
            {
                for (int n = 0; n < MaxNodes; n++)
                {
                    // get program arguments
                    var args = new List<ProgramItem>[2];
                    int argCount = 0;
                    switch(ValidNodes[n])
                    {
                        case OpTypes.not: argCount = 1; break;
                        case OpTypes.or : argCount = 2; break;
                    }
                    for (int a = 0; a < argCount; a++)
                    {
                        args[a] = Prgs[s - 1, n];
                    }

                    // assemble the program depending on node function
                    if (ValidNodes[n] == OpTypes.not)
                    {
                        for (int a1 = 0; a1 < args[0].Count; a1++)
                        {
                            var p = new ProgramItem();
                            p.Output = new List<ulong>();

                            for (int v = 0; v < args[0][a1].Output.Count; a1++)
                            {
                                ulong vector = args[0][a1].Output[v];
                                ulong pvector = ~vector;
                                p.Output.Add(pvector);
                            }

                            string code = args[0][a1].Code;
                            p.Code = (new NodeOp1Not {Node0 = Parser.Parse(code)}).ToString();

                            Prgs[s, n].Add(p);
                        }
                    }

                    else if (ValidNodes[n] == OpTypes.or)
                    {
                        int vcount = args[0][0].Output.Count;
                        for (int a1 = 0; a1 < args[0].Count; a1++)
                            for (int a2 = a1; a2 < args[1].Count; a2++)
                            {
                                var p = new ProgramItem();
                                p.Output = new List<ulong>();

                                for (int v = 0; v < vcount; a1++)
                                {
                                    ulong vector1 = args[0][a1].Output[v];
                                    ulong vector2 = args[1][a2].Output[v];
                                    ulong pvector = vector1 | vector2;
                                    p.Output.Add(pvector);
                                }

                                string code1 = args[0][a1].Code;
                                string code2 = args[1][a2].Code;
                                p.Code = (new NodeOp2Or { 
                                    Node0 = Parser.Parse(code1),
                                    Node1 = Parser.Parse(code1)}).ToString();

                                Prgs[s, n].Add(p);
                            }
                    }

                    // todo: support more nodes
                }
            }

        }
    }
}
