namespace Icfpc2013
{
    internal class NodeOp1Shr16 : NodeOp1
    {
        public long Eval()
        {
            return Node0.Eval()>>16;
        }

        public Node Node0 { get; set; }
    }
}