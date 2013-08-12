using System;
namespace Icfpc2013.Ops
{
    public class Node1 : Node
    {
        #region Public Methods and Operators

        public static Node1 Instance = new Node1();

        public Node Clone()
        {
            return Instance;
        }

        public ulong Eval(ExecContext context)
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

        public void Op(ref OpTypes ops)
        {
        }

        public OpTypes GetMainOp()
        {
            return OpTypes.one;
        }

        #endregion
    }
}