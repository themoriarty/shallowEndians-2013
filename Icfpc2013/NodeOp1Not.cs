namespace Icfpc2013
{
    internal class NodeOp1Not : NodeOp1
    {
        public long Eval()
        {
            return ~Node0.Eval();
        }

        public Node Node0 { get; set; }

        public string ToString(int indentLevel)
        {
            string output = "not " + Node0.ToString(indentLevel);
        }
    }
}