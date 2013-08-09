namespace Icfpc2013
{
    internal class NodeOp2Plus : NodeOp2
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp2Plus { Node0 = Node0 == null ? null : Node0.Clone(), Node1 = Node1 == null ? null : Node1.Clone() };
        }

        public override long Eval(ExecContext context)
        {
            return Node0.Eval(context) + Node1.Eval(context);
        }

        public override string Serialize()
        {
            return string.Format("(plus {0} {1})", Node0.Serialize(), Node1.Serialize());
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
        #endregion
    }
}