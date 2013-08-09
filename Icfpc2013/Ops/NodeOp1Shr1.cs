namespace Icfpc2013
{
    internal class NodeOp1Shr1 : NodeOp1
    {
        public override long Eval()
        {
            return Node0.Eval()>>1;
        }

        public override string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }
            output += "( ";
            return output + "shr1 " + Node0.ToString(indentLevel + 1) + " )";
        }
    }
}