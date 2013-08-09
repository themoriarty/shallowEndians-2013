namespace Icfpc2013
{
    internal class NodeOp1Not : NodeOp1
    {
        public override long Eval(ExecContext context)
        {
            return ~Node0.Eval(context);
        }
    }
}