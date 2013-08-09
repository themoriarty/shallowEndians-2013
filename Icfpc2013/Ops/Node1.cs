namespace Icfpc2013
{
    internal class Node1 : Node
    {
        #region Public Methods and Operators

        public Node Clone()
        {
            return new Node1();
        }

        public long Cost()
        {
            return 1;
        }

        public long Eval(ExecContext context)
        {
            return 1;
        }

        public string Serialize()
        {
            return "1";
        }

        #endregion
    }
}