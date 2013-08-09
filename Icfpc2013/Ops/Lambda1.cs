namespace Icfpc2013
{
    internal class Lambda1
    {
        #region Public Properties

        public NodeId Id0 { get; set; }

        public Node Node0 { get; set; }

        public string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "\t";
            }
            output += "( ";
            output += "lambda " + Id0.ToString(indentLevel + 1) + " ";
            output += Node0.ToString(indentLevel + 1) + " )";
            return output;
        }
        #endregion

        #region Public Methods and Operators

        public Lambda1 Clone()
        {
            return new Lambda1 { Id0 = (NodeId)Id0.Clone(), Node0 = Node0.Clone() };
        }

        #endregion
    }
}