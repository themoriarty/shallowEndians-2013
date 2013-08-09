namespace Icfpc2013
{
    internal class NodeOp2And : NodeOp2
    {
        public override long Eval(ExecContext context)
        {
            return Node0.Eval(context) & Node1.Eval(context);
        }

        public override Node Clone()
        {
            return new NodeOp2And { Node0 = Node0.Clone(), Node1 = Node1.Clone() };
        }
    }
}