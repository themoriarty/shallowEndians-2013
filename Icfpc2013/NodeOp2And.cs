namespace Icfpc2013
{
    internal class NodeOp2And : NodeOp2
    {
        public long Eval()
        {
            return Node0.Eval() & Node1.Eval();
        }

        public Node Node0 { get; set; }

        public Node Node1 { get; set; }
    }
}