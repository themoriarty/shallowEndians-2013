namespace Icfpc2013
{
    internal class NodeOp1Shr16 : NodeOp1
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp1Shr16 { Node0 = Node0.Clone() };
        }

        public override long Eval(ExecContext context)
        {
            return Node0.Eval(context) >> 16;
        }

        public override string ToString()
        {
            return string.Format("(shr16 {0})", Node0);
        }

        #endregion
    }
}