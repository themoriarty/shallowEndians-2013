namespace Icfpc2013
{
    internal class NodeOp1Shr1 : NodeOp1
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp1Shr1 { Node0 = Node0.Clone() };
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
        public override long Eval(ExecContext context)
        {
            return Node0.Eval(context) >> 1;
        }

        #endregion
    }
}