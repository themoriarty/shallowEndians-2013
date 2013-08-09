namespace Icfpc2013
{
    internal class NodeOp1Shr1 : NodeOp1
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp1Shr1 { Node0 = Node0.Clone() };
        }

        public override long Eval(ExecContext context)
        {
            return Node0.Eval(context) >> 1;
        }

        public override string Serialize()
        {
            return string.Format("(shr1 {0})", Node0.Serialize());
        }

        #endregion
    }
}