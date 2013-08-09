﻿namespace Icfpc2013
{
    internal class NodeOp1Not : NodeOp1
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp1Not { Node0 = Node0 != null ? Node0.Clone() : null };
        }

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

        public override string Serialize()
        {
            return string.Format("(not {0})", Node0.Serialize());
        }

        #endregion
    }
}