namespace Icfpc2013
{
    internal class NodeOp2Or : NodeOp2
    {
        public override long Eval()
        {
            return Node0.Eval() | Node1.Eval();
        }

        public override string ToString(int indentLevel)
        {
            string output = "";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += " ";
            }
            output += "or (" + Node0.ToString(indentLevel) + ")\n";
            for (int i = 0; i < indentLevel; i++)
            {
                output += " ";
            }
            output += "(" + Node1.ToString(indentLevel + 1) + ")";
            return output;
        }
    }
}