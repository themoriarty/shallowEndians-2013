namespace Icfpc2013
{
    internal class NodeOp1Not : NodeOp1
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp1Not { Node0 = Node0.Clone() };
        }

        public Node Node0 { get; set; }

        public override string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }
            output += "( ";
            return output + "not " + Node0.ToString(indentLevel + 1) + " )";
        }

        public override long Eval(ExecContext context)
        {
            return ~Node0.Eval(context);
        }

        #endregion
    }
}