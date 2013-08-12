namespace Icfpc2013.Ops
{
    public class NodeOp1Shr1 : NodeOp1
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp1Shr1 { Node0 = Node0 != null ? Node0.Clone() : null };
        }

        public override ulong Eval(ExecContext context)
        {
            return this.Node0.Eval(context) >> 1;
        }

        public override string Serialize()
        {
            return string.Format("(shr1 {0})", this.Node0.Serialize());
        }

        public override string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }

            output += "( ";
            return output + "shr1 " + this.Node0.ToString(indentLevel + 1) + " )";
        }

        public override void Op(ref OpTypes ops)
        {
            ops |= OpTypes.shr1;
            Node0.Op(ref ops);
        }

        public override OpTypes GetMainOp()
        {
            return OpTypes.shr1;
        }

        #endregion
    }
}