namespace Icfpc2013
{
    internal class NodeOp2Xor : NodeOp2
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp2Xor { Node0 = Node0.Clone(), Node1 = Node1.Clone() };
        }

        public override string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }
            output += "xor " + Node0.ToString(indentLevel + 1) + " ";
            output += Node1.ToString(indentLevel + 1) + " )";
            return output;
        }
        public override long Eval(ExecContext context)
        {
            return Node0.Eval(context) ^ Node1.Eval(context);
        }

        #endregion
    }
}