using Icfpc2013;
using Icfpc2013.Ops;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatSolverRunner
{
    class Program
    {
        public static string GetNextLine()
        {
            string line = Console.ReadLine().Trim();
            while (line == "")
            {
                line = Console.ReadLine().Trim();
            }
            return line;
        }

        public static void ParseInput(out string progId, out int progSize, out string[] ops, out ulong[] inputs, out ulong[] outputs)
        {
            string line = GetNextLine();
            if (!line.StartsWith("START:"))
            {
                throw new Exception("The first line " + line + " does not start with START:");
            }
            progId = line.Substring("START:".Length);

            line = GetNextLine();
            progSize = Int32.Parse(line);

            line = GetNextLine();
            ops = line.Split(' ');


            line = GetNextLine();
            string[] ins = line.Split(' ');

            line = GetNextLine();
            string[] outs = line.Split(' ');
            if (ins.Length != outs.Length)
            {
                throw new Exception("There were " + ins.Length + " inputs and " + outs.Length + " outputs provided");
            }
            int size = Math.Max(4, ins.Length);
            inputs = new ulong[size];
            outputs = new ulong[size];

            for (int i = 0; i < size; ++i)
            {
                inputs[i] = ulong.Parse(ins[i].Substring(2), System.Globalization.NumberStyles.HexNumber);
                outputs[i] = ulong.Parse(outs[i].Substring(2), System.Globalization.NumberStyles.HexNumber);
            }


        }

        static void Main(string[] args)
        {
            string fName = "sat-std-out.txt";
            if (args.Length > 0 && args[0] == "simple")
            {
                fName = "sat-std-out2.txt";
            }

            var commStream = new StreamWriter(System.Console.OpenStandardOutput());
            var newStream = new StreamWriter(fName);
            System.Console.SetOut(newStream);
            if (args.Length > 0 && args[0] == "simple")
            {
                Icfpc2013.Program.UseSimpleTFold = true;
                System.Console.WriteLine("Using simpler tfold");
            }
            string progId;
            int progSize;
            string[] ops;
            ulong[] inputs;
            ulong[] outputs;
            ParseInput(out progId, out progSize, out ops, out inputs, out outputs);
            var operators = ProgramTree.GetOpTypes(ops);


            while (true)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Lambda1 res = null;
                try
                {
                    res = Icfpc2013.Program.SolveSat(progSize, operators, inputs, outputs);
                }
                catch (Exception ex)
                {
                }
                sw.Stop();

                var finalResult = res != null ? res.Serialize() : "NO RESULT";
                Console.WriteLine("Result: {0}, execution time: {1}", finalResult, sw.ElapsedMilliseconds / 1000.0);
                if (res != null)
                {
                    commStream.Write("SOLUTION:" + finalResult + "\n");
                    commStream.Flush();
                }
                else
                {
                    commStream.Write("SOLUTION:\n");
                    commStream.Flush();
                    break;
                }

                string line = GetNextLine();
                if (line == "STATUS:ok")
                {
                    Console.WriteLine("Success!");
                    break;
                }
                else
                {
                    ulong addInput = ulong.Parse(GetNextLine().Substring(2), System.Globalization.NumberStyles.HexNumber);
                    ulong addOutputs = ulong.Parse(GetNextLine().Substring(2), System.Globalization.NumberStyles.HexNumber);
                    ulong[] newInputs = new ulong[inputs.Length + 1];
                    ulong[] newOutputs = new ulong[outputs.Length + 1];
                    Array.Copy(inputs, newInputs, inputs.Length);
                    Array.Copy(outputs, newOutputs, outputs.Length);
                    outputs = newOutputs;
                    inputs = newInputs;
                    inputs[inputs.Length - 1] = addInput;
                    outputs[outputs.Length - 1] = addOutputs;
                    
                }
            }

            
        }
    }
}
