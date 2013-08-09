using System;
using System.Collections.Generic;
using System.Linq;


namespace Icfpc2013
{
    using System;

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            const int programSize = 3;

            List<Node> validNodes = new List<Node>();
            var inputArg = new NodeId();
            inputArg.Name = "x";
            validNodes.Add(inputArg);
            validNodes.Add(new Node0());
            validNodes.Add(new Node1());

            validNodes.Add(new NodeOp1Shl1());
            validNodes.Add(new NodeOp1Not());
            //validNodes.Add(new NodeOp1Shr16());
            //validNodes.Add(new NodeOp1Shr4());
            //validNodes.Add(new NodeOp1Shr1());

            //validNodes.Add(new NodeOp2And());
            //validNodes.Add(new NodeOp2Plus());
            //validNodes.Add(new NodeOp2Xor());
            //validNodes.Add(new NodeOp2Or());

            int[] inputs = {12, 137};
            ulong[] outputs = { 0xFFFFFFFFFFFFFFDA, 0xFFFFFFFFFFFFFD90 };

            var builder = new TreeGenerator(validNodes, programSize);
            foreach (var root in builder.GenerateAllPrograms())
            {
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
                    }
                    if (valid)
                    {
                        Console.WriteLine(root.Serialize());
                    }
                    Console.WriteLine();
                }
            }
        }

        private static void Main2(string[] args)
        {
            //API.Test();

            Lambda2 p = new Lambda2
            {
                Node0 = new NodeFold
                {
                    Node0 = new NodeId { Name = "X" },
                    Node1 = new Node0(),
                    Node2 = new Lambda2
                    {
                        Id0 = new NodeId { Name = "X" },
                        Id1 = new NodeId { Name = "Y" },
                        Node0 = new NodeIf0
                        {
                            Node0 = new Node1(),
                            Node1 = new NodeOp1Not()
                            {
                                Node0 = new NodeId { Name = "Y" }
                            },
                            Node2 = new NodeOp2And()
                            {
                                Node0 = new Node1(),
                                Node1 = new Node1()
                            }
                        }
                    }
                },
                Id0 = new NodeId { Name = "X" },
                Id1 = new NodeId { Name = "Y" }
            };
            System.Console.WriteLine(p.ToString(0));
            
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
        }

        #endregion
    }
}