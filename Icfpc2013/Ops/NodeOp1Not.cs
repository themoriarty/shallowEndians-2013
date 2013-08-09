namespace Icfpc2013
{
    internal class NodeOp1Not : NodeOp1
    {
        public override long Eval()
        {
            return ~Node0.Eval();
        }

        public Node Node0 { get; set; }

        public override string ToString(int indentLevel)
        {
            string output = "";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += " ";
            }
            return output + "not (" + Node0.ToString(indentLevel) + ")";
        }

    }
}