namespace Icfpc2013
{
    internal class NodeIf0 : Node
    {
        #region Public Properties

        public Node Node0 { get; set; }

        public Node Node1 { get; set; }

        public Node Node2 { get; set; }

        #endregion

        public long Eval()
        {
            if (Node0.Eval() == 0)
            {
                return Node1.Eval();
            }

            return Node2.Eval();
        }

        public string ToString(int indentLevel)
        {
            string output = "if0 (" + Node0.ToString(indentLevel) + ")\n";
            for(int i = 0; i < indentLevel; ++i)
                output += " ";
            output += "(" + Node1.ToString(indentLevel + 1) + ")\n";
            for(int i = 0; i < indentLevel; ++i)
                output += " ";
            output += "(" + Node2.ToString(indentLevel + 1) + ")\n";
            return output;
        }
    }
}