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

        public long Eval()
        {
            throw new NotImplementedException();
        }

        public long Cost()
        {
            return 1 + Node0.Cost() + Node1.Cost() + Node2.Node0.Cost();
        }

        public string ToString(int indentLevel)
        {
            string output = "";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += " ";
            }
            output += "fold (" + Node0.ToString(indentLevel + 1) + ")";
            output += "(" + Node1.ToString(indentLevel + 2) + ")\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += " ";
            }
            output += "(" + Node2.ToString(indentLevel) + ")";
            return output;
        }
    }
}