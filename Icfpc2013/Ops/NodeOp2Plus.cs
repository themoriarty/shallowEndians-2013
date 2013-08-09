namespace Icfpc2013
{
    internal class NodeOp2Plus : NodeOp2
    {
        public override long Eval(ExecContext context)
        {
            return Node0.Eval(context) + Node1.Eval(context);
        }

        public override Node Clone()
        {
            return new NodeOp2Plus { Node0 = Node0.Clone(), Node1 = Node1.Clone() };
        }
    }
}