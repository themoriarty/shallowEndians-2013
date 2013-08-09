namespace Icfpc2013
{
    internal class NodeOp2Xor : NodeOp2
    {
        public override long Eval(ExecContext context)
        {
            return Node0.Eval(context) ^ Node1.Eval(context);
        }
    }
}