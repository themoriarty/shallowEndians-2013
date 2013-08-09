namespace Icfpc2013
{
    internal class Node0 : Node
    {
        public long Eval(ExecContext context)
        {
            return 0;
        }

        public long Cost()
        {
            return 1;
        }

        public Node Clone()
        {
            return new Node0();
        }
    }
}