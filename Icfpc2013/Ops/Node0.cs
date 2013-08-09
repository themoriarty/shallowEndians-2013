namespace Icfpc2013
{
    internal class Node0 : Node
    {
        #region Public Methods and Operators

        public Node Clone()
        {
            return new Node0();
        }

        public string ToString(int indentLevel)
        {
            return "0";
        }

        public long Cost()
        {
            return 1;
        }

        public long Eval(ExecContext context)
        {
            return 0;
        }

        public string Serialize()
        {
            return "0";
        }

        #endregion
    }
}