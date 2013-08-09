namespace Icfpc2013
{
    internal class NodeOp1Shl1 : NodeOp1
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp1Shl1 { Node0 = Node0.Clone() };
        }

        public override long Eval(ExecContext context)
        {
            return Node0.Eval(context) << 1;
        }

        public override string ToString()
        {
            return string.Format("(shl1 {0})", Node0);
        }

        #endregion
    }
}