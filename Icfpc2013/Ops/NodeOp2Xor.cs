namespace Icfpc2013
{
    internal class NodeOp2Xor : NodeOp2
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp2Xor { Node0 = Node0.Clone(), Node1 = Node1.Clone() };
        }

        public override long Eval(ExecContext context)
        {
            return Node0.Eval(context) ^ Node1.Eval(context);
        }

        public override string ToString()
        {
            return string.Format("(xor {0} {1})", Node0, Node1);
        }

        #endregion
    }
}