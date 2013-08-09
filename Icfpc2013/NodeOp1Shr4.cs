namespace Icfpc2013
{
    internal class NodeOp1Shr4 : NodeOp1
    {
        public long Eval()
        {
            return Node0.Eval()>>4;
        }

        public Node Node0 { get; set; }
    }
}