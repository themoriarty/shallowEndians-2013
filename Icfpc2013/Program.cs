namespace Icfpc2013
{
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
            
        }

        #endregion
    }
}