namespace Icfpc2013.Ops
{
    public class Node0 : Node
    {
        #region Public Methods and Operators

        public static Node0 Instance = new Node0();

        public OpTypes GetMainOp()
        {
            return OpTypes.none;
        }

        public Node Clone()
        {
            return Instance;
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