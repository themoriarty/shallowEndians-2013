namespace Icfpc2013
{
    internal class Node0 : Node
    {
        #region Public Methods and Operators

        public Node Clone()
        {
            return new Node0();
        }

        public long Cost()
        {
            return 1;
        }

        public long Eval(ExecContext context)
        {
            return 0;
        }

        public override string ToString()
        {
            return "0";
        }

        #endregion
    }
}