﻿namespace Icfpc2013.Ops
{
    public class NodeOp2Or : NodeOp2
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp2Or { Node0 = Node0 == null ? null : Node0.Clone(), Node1 = Node1 == null ? null : Node1.Clone() };
        }

        public override ulong Eval(ExecContext context)
        {
            return this.Node0.Eval(context) | this.Node1.Eval(context);
        }

        public override string Serialize()
        {
            return string.Format("(or {0} {1})", this.Node0.Serialize(), this.Node1.Serialize());
        }

        public override string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }

            output += "or " + this.Node0.ToString(indentLevel + 1) + " ";
            output += this.Node1.ToString(indentLevel + 1) + " )";
            return output;
        }

        public override void Op(ref OpTypes ops)
        {
            ops |= OpTypes.or;
            Node0.Op(ref ops);
            Node1.Op(ref ops);

        }

        #endregion
    }
}