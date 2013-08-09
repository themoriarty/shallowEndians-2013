namespace Icfpc2013.Ops
{
    public class NodeOp1Not : NodeOp1
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp1Not { Node0 = this.Node0.Clone() };
        }

        public override string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }
            output += "( ";
            return output + "not " + this.Node0.ToString(indentLevel + 1) + " )";
        }

        public override long Eval(ExecContext context)
        {
            return ~this.Node0.Eval(context);
        }

        public override string Serialize()
        {
            return string.Format("(not {0})", this.Node0.Serialize());
        }

        #endregion
    }
}