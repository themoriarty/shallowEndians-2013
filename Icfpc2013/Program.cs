namespace Icfpc2013
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

        private static void Main(string[] args)
        {

            //const int judgesProgramSize = 4;
            //const int programSize = judgesProgramSize - 1;
            //var training = API.GetTrainingProblem(new TrainRequest(judgesProgramSize, null));
            //var programId = training.id;

            //Console.WriteLine("Challenge: {0}", string.Join(", ", training.challenge));

            //var operators = training.operators;

            //const int judgesProgramSize = 4;
            //const int programSize = judgesProgramSize - 1;
            //var programId = "IQiqcWp8Cmr4TuBbHNE4OOBI";
            //var operators = new string[] { "shr1", "shr4" };

            var todo = JArray.Parse(File.ReadAllText(@"..\..\..\todo.json"));

            foreach (var task in todo)
            {
                Console.WriteLine("\n");
                var id = (string)task["id"];
                var size = (int)task["size"];
                var operators = task["operators"].Select(s => (string)s).ToArray();

                Solve(id, size, operators);

                Thread.Sleep(20000);
            }

            //Solve("JJwJSyQnSB4DzZ0t17Xw5Aj6", 4, new string[] {"shr1", "shr4"});
        }

        private static void Solve(string programId, int judgesProgramSize, string[] operators)
        {
            int programSize = judgesProgramSize - 1;

            Console.WriteLine("ProgramId: {0}", programId);
            Console.WriteLine("Training: {0}", string.Join(", ", operators));

            var ops = ProgramTree.GetOpTypes(operators);
            var validNodes = ProgramTree.GetAvailableNodes(ops);

            ulong[] inputs = ProgramTree.GetInputVectorList(0).ToArray(); //{0x12, 0x137};
            var inputStrings = inputs.Select(s => string.Format("0x{0:X16}", s)).ToArray();

            Console.WriteLine("Input: {{{0}}}", string.Join(", ", inputStrings));

            var outputsResponse = API.Eval(new EvalRequest(programId, null, inputStrings));

            if (outputsResponse.status != "ok" || outputsResponse.outputs == null)
            {
                throw new Exception("eval failed");
            }

            Console.WriteLine("Output: {{{0}}}", string.Join(", ", outputsResponse.outputs));

            ulong[] outputs = outputsResponse.outputs.Select(s => ulong.Parse(s.Replace("0x", string.Empty), NumberStyles.HexNumber)).ToArray();

            //var outputs = new ulong[] { 0x0000000000000001, 0x0000000000000000, 0x0000000000000003, 0x0000000000000002, 0x0000000000000005, 0x0000000000000004, 0x0000000000000007, 0x0000000000000006, 0x0000000000000009, 0x0000000000000008, 0x000000000000000B, 0x000000000000000A, 0x000000000000000D, 0x000000000000000C, 0x000000000000000F, 0x000000000000000E };

            var builder = new TreeGenerator(validNodes, programSize);
            int totalCount = 0;
            foreach (var root in builder.GenerateAllPrograms())
            {
                //Console.WriteLine(root.Serialize());
                //totalCount++;
                if (root.Size() == programSize)
                {
                    //Console.WriteLine(root.Serialize());
                    bool valid = true;
                    for (int i = 0; i < inputs.Length; ++i)
                    {
                        ExecContext ctx = new ExecContext();
                        ctx.Vars["x"] = inputs[i];
                        var output = root.Eval(ctx);
                        if (output != outputs[i])
                        {
                            valid = false;
                        }
                        //Console.WriteLine("{0} ({1}) = {2:X} ({3})", root.Serialize(), inputs[i], output, valid);
                    }
                    if (valid)
                    {
                        var currentOps = OpTypes.none;
                        root.Op(ref currentOps);

                        if (currentOps == ops)
                        {
                            var finalResult = new Lambda1 { Id0 = new NodeId { Name = "x" }, Node0 = root }.Serialize();
                            Console.WriteLine("Submitting: {0}", finalResult);

                            var response = API.Guess(new Guess(programId, finalResult));

                            Console.WriteLine("Gues: {0} {1} {2}", response.status, response.message, string.Join(", ", response.values ?? new string[] { }));

                            break;
                        }
                    }

                    //Console.WriteLine();
                }
            }
            Console.WriteLine(totalCount);
        }

        private static void Main2(string[] args)
        {
            // API.Test();
            var p = new Lambda2
                        {
                            Node0 =
                                new NodeFold
                                    {
                                        Node0 = new NodeId { Name = "X" }, 
                                        Node1 = new Node0(), 
                                        Node2 =
                                            new Lambda2
                                                {
                                                    Id0 = new NodeId { Name = "X" }, 
                                                    Id1 = new NodeId { Name = "Y" }, 
                                                    Node0 =
                                                        new NodeIf0
                                                            {
                                                                Node0 = new Node1(), 
                                                                Node1 = new NodeOp1Not { Node0 = new NodeId { Name = "Y" } }, 
                                                                Node2 = new NodeOp2And { Node0 = new Node1(), Node1 = new Node1() }
                                                            }
                                                }
                                    }, 
                            Id0 = new NodeId { Name = "X" }, 
                            Id1 = new NodeId { Name = "Y" }
                        };
            Console.WriteLine(p.ToString(0));

            /*
             (lambda (x_18991) 
                (
                    shl1 (
                        shr16 (
                            shr4 (
                                if0 (and (xor (xor (not 1) 1) x_18991) x_18991) x_18991 x_18991
                            )
                        )
                    )
                )
            ) 
             */
            var program = new ProgramTree
                              {
                                  Program =
                                      new Lambda1
                                          {
                                              Id0 = new NodeId { Name = "x_18991" }, 
                                              Node0 =
                                                  new NodeOp1Shl1
                                                      {
                                                          Node0 =
                                                              new NodeOp1Shr16
                                                                  {
                                                                      Node0 =
                                                                          new NodeOp1Shr4
                                                                              {
                                                                                  Node0 =
                                                                                      new NodeIf0
                                                                                          {
                                                                                              Node0 =
                                                                                                  new NodeOp2And
                                                                                                      {
                                                                                                          Node0 =
                                                                                                              new NodeOp2Xor
                                                                                                                  {
                                                                                                                      Node0 =
                                                                                                                          new NodeOp2Xor
                                                                                                                              {
                                                                                                                                  Node0 =
                                                                                                                                      new NodeOp1Not
                                                                                                                                          {
                                                                                                                                              Node0
                                                                                                                                                  =
                                                                                                                                                  new Node1
                                                                                                                                                  (
                                                                                                                                                  )
                                                                                                                                          }, 
                                                                                                                                  Node1 =
                                                                                                                                      new Node1(
                                                                                                                                      )
                                                                                                                              }, 
                                                                                                                      Node1 =
                                                                                                                          new NodeId { Name = "x_18991" }
                                                                                                                  }, 
                                                                                                          Node1 = new NodeId { Name = "x_18991" }
                                                                                                      }, 
                                                                                              Node1 = new NodeId { Name = "x_18991" }, 
                                                                                              Node2 = new NodeId { Name = "x_18991" }
                                                                                          }
                                                                              }
                                                                  }
                                                      }
                                          }
                              };

            Console.WriteLine("{0:X16}", program.Run(0x0011223344556677));
            Console.WriteLine("{0:X16}", program.Clone().Run(0x0011223344556677));

            Console.WriteLine(new Compiler().Run());

            Console.WriteLine(program.Program.ToString(0));
            Console.WriteLine("{0}", program.Serialize());

            var input = "(lambda (x_68407) (fold (not (shr4 (or 0 (if0 (xor (and (shr16 (plus (and x_68407 (plus x_68407 x_68407)) x_68407)) x_68407) 1) x_68407 x_68407)))) x_68407 (lambda (x_68408 x_68409) (xor (shr4 x_68409) x_68408))))";
            var output = ProgramTree.Parse(input).Serialize();

            Debug.Assert(string.Equals(input, output));
        }

        #endregion
    }
}