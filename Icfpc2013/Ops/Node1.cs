namespace Icfpc2013.Ops
{
    public class Node1 : Node
    {
        #region Public Methods and Operators

        public Node Clone()
        {
            return new Node1();
        }

        public long Eval(ExecContext context)
        {
            return 1;
        }

        public string Serialize()
        {
            return "1";
        }

        public long Size()
        {
            return 1;
        }

        public string ToString(int indentLevel)
        {
            return "1";
        }

        #endregion
    }
}