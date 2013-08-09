namespace Icfpc2013
{
    internal class NodeOp2Plus : NodeOp2
    {
        public override long Eval()
        {
            return Node0.Eval() + Node1.Eval();
        }

        public override string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }
            output += "plus " + Node0.ToString(indentLevel + 1) + " ";
            output += Node1.ToString(indentLevel + 1) + " )";
            return output;
        }
    }
}