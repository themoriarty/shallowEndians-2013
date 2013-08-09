namespace Icfpc2013.Ops
{
    public class Node0 : Node
    {
        #region Public Methods and Operators

        public Node Clone()
        {
            return new Node0();
        }

        public ulong Eval(ExecContext context)
        {
            return 0;
        }

        public string Serialize()
        {
            return "0";
        }

        public long Size()
        {
            return 1;
        }

        public string ToString(int indentLevel)
        {
            return "0";
        }

        public void Op(ref OpTypes ops)
        {
        }

        #endregion
    }
}