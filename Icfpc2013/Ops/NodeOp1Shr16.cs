namespace Icfpc2013
{
    internal class NodeOp1Shr16 : NodeOp1
    {
        public override long Eval()
        {
            return Node0.Eval()>>16;
        }

        public override string ToString(int indentLevel)
        {
            string output = "";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += " ";
            }
            return output + "shr16 (" + Node0.ToString(indentLevel) + ")";
        }
    }
}