namespace Icfpc2013
{
    internal class Lambda2
    {
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
    }
}