namespace Icfpc2013.Ops
{
    public class NodeOp1Shr16 : NodeOp1
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp1Shr16 { Node0 = Node0 != null ? Node0.Clone() : null };
        }

        public override long Eval(ExecContext context)
        {
            return this.Node0.Eval(context) >> 16;
        }

        public override string Serialize()
        {
            return string.Format("(shr16 {0})", this.Node0.Serialize());
        }

        public override string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }

            output += "( ";
            return output + "shr16 " + this.Node0.ToString(indentLevel + 1) + " )";
        }

        #endregion
    }
}