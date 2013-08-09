namespace Icfpc2013
{
    internal class NodeOp1Shr4 : NodeOp1
    {
        public override long Eval()
        {
            return Node0.Eval()>>4;
        }

        public override string ToString(int indentLevel)
        {
            string output = "";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += " ";
            }
            return output + "shr4 (" + Node0.ToString(indentLevel) + ")";
        }
    }
}