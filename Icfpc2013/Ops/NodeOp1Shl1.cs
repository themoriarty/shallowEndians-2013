namespace Icfpc2013
{
    internal class NodeOp1Shl1 : NodeOp1
    {
        public override long Eval(ExecContext context)
        {
            return Node0.Eval(context)<<1;
        }
    }
}