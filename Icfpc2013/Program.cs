namespace Icfpc2013
{
    using System;

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
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
        }

        #endregion
    }
}