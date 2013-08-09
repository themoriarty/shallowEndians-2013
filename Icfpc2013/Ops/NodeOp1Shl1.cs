namespace Icfpc2013
{
    internal class NodeOp1Shl1 : NodeOp1
    {
        public override long Eval()
        {
            return Node0.Eval()<<1;
        }

        public override string ToString(int indentLevel)
        {
            string output = "";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += " ";
            }
            return output + "shl1 (" + Node0.ToString(indentLevel) + ")";
        }
    }
}