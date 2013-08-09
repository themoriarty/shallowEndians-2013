namespace Icfpc2013
{
    internal class NodeIf0 : Node
    {
        #region Public Properties

        public Node Node0 { get; set; }

        public Node Node1 { get; set; }

        public Node Node2 { get; set; }

        #endregion

        #region Public Methods and Operators

        public Node Clone()
        {
            return new NodeIf0 { Node0 = Node0.Clone(), Node1 = Node1.Clone(), Node2 = Node2.Clone() };
        }


        public string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }
            output += "( ";
            output += "if0 " + Node0.ToString(indentLevel + 1) + " ";
            output += Node1.ToString(indentLevel + 1) + " ";
            output += Node2.ToString(indentLevel + 1);
            output += " )";
            return output;
        }

        public long Size()
        {
            return 1 + Node0.Size() + Node1.Size() + Node2.Size();
        }

        public long Eval(ExecContext context)
        {
            if (Node0.Eval(context) == 0)
            {
                return Node1.Eval(context);
            }

            return Node2.Eval(context);
        }

        public string Serialize()
        {
            return string.Format("(if0 {0} {1} {2})", Node0.Serialize(), Node1.Serialize(), Node2.Serialize());
        }

        #endregion
    }
}