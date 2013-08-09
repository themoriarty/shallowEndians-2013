namespace Icfpc2013
{
    using System;

    internal class NodeFold : Node
    {
        #region Public Properties

        public Node Node0 { get; set; }

        public Node Node1 { get; set; }

        public Lambda2 Node2 { get; set; }

        #endregion

        #region Public Methods and Operators

        public Node Clone()
        {
            return new NodeFold { Node0 = Node0.Clone(), Node1 = Node1.Clone(), Node2 = Node2.Clone() };
        }

        public long Size()
        {
            return 2 + Node0.Size() + Node1.Size() + Node2.Node0.Size();
        }

        public string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }
            output += "( ";
            output += "fold " + Node0.ToString(indentLevel + 1) + " ";
            output += Node1.ToString(indentLevel + 1) + " ";
            output += Node2.ToString(indentLevel + 1) + " )";
            return output;
        }

        public long Eval(ExecContext context)
        {
            var input = Node0.Eval(context);
            var init = Node1.Eval(context);

            var current = init;

            for (int i = 0; i < 8; ++i)
            {
                var left = (input >> (i * 8)) & 0x00ff;

                current = Node2.Eval(left, current);
            }

            return current;
        }

        public string Serialize()
        {
            return string.Format("(fold {0} {1} {2})", Node0.Serialize(), Node1.Serialize(), Node2.Serialize());
        }

        #endregion
    }
}