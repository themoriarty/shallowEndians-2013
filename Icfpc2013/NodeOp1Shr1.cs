namespace Icfpc2013
{
    internal class NodeOp1Shr1 : NodeOp1
    {
        public long Eval()
        {
            return Node0.Eval()>>1;
        }

        public Node Node0 { get; set; }
    }
}