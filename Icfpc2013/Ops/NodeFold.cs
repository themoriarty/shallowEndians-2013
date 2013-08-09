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

        public long Cost()
        {
            return 1 + Node0.Cost() + Node1.Cost() + Node2.Node0.Cost();
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
            throw new NotImplementedException();
        }

        #endregion
    }
}