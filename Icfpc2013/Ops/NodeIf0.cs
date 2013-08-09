﻿namespace Icfpc2013.Ops
{
    public class NodeIf0 : Node
    {
        #region Public Properties

        public Node Node0 { get; set; }

        public Node Node1 { get; set; }

        public Node Node2 { get; set; }

        #endregion

        #region Public Methods and Operators

        public Node Clone()
        {
            return new NodeIf0 { Node0 = this.Node0.Clone(), Node1 = this.Node1.Clone(), Node2 = this.Node2.Clone() };
        }


        public string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }
            output += "( ";
            output += "if0 " + this.Node0.ToString(indentLevel + 1) + " ";
            output += this.Node1.ToString(indentLevel + 1) + " ";
            output += this.Node2.ToString(indentLevel + 1);
            output += " )";
            return output;
        }

        public long Size()
        {
            return 1 + this.Node0.Size() + this.Node1.Size() + this.Node2.Size();
        }

        public long Eval(ExecContext context)
        {
            if (this.Node0.Eval(context) == 0)
            {
                return this.Node1.Eval(context);
            }

            return this.Node2.Eval(context);
        }

        public string Serialize()
        {
            return string.Format("(if0 {0} {1} {2})", this.Node0.Serialize(), this.Node1.Serialize(), this.Node2.Serialize());
        }

        #endregion
    }
}