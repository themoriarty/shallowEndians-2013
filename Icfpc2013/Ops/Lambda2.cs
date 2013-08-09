namespace Icfpc2013
{
    internal class Lambda2
    {
        #region Public Properties

        public NodeId Id0 { get; set; }

        public NodeId Id1 { get; set; }

        public Node Node0 { get; set; }

        public string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }
            output += "( ";
            output += "lambda " + Id0.ToString(indentLevel + 1) + " " + Id1.ToString(indentLevel + 1) + " ";
            output += Node0.ToString(indentLevel + 1) + " )";
            return output;
        }
        #endregion

        #region Public Methods and Operators

        public Lambda2 Clone()
        {
            return new Lambda2 { Id0 = (NodeId)Id0.Clone(), Id1 = (NodeId)Id1.Clone(), Node0 = Node0.Clone() };
        }

        #endregion
    }
}