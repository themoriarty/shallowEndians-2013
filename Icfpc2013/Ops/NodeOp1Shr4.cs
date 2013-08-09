namespace Icfpc2013.Ops
{
    public class NodeOp1Shr4 : NodeOp1
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp1Shr4 { Node0 = Node0 != null ? Node0.Clone() : null };
        }

        public override string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }
            output += "( ";
            return output + "shr4 " + this.Node0.ToString(indentLevel + 1) + " )";
        }
        public override long Eval(ExecContext context)
        {
            return this.Node0.Eval(context) >> 4;
        }

        public override string Serialize()
        {
            return string.Format("(shr4 {0})", this.Node0.Serialize());
        }

        #endregion
    }
}