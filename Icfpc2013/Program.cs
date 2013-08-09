namespace Icfpc2013
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Icfpc2013.Ops;

    public class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            const int judgesProgramSize = 5;
            const int programSize = judgesProgramSize - 1;

            var operators = new string[] { "plus", "shr16" };

            var ops = ProgramTree.GetOpTypes(operators);
            var validNodes = ProgramTree.GetAvailableNodes(ops);

            ulong[] inputs = { 0, 0x12, 0x137 }; //{0x12, 0x137};
            ulong[] outputs = { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000 };

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
                            Console.WriteLine(new Lambda1 { Id0 = new NodeId { Name = "x" }, Node0 = root }.Serialize());
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