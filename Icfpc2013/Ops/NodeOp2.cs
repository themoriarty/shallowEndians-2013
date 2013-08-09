namespace Icfpc2013
{
    class NodeOp2 : Node
    {
        public virtual long Eval(ExecContext context)
        {
            throw new System.NotImplementedException();
        }

        public long Cost()
        {
            return 1 + Node0.Cost() + Node1.Cost();
        }

        public virtual Node Clone()
        {
            throw new System.NotImplementedException();
        }

        public Node Node0 { get; set; }

        public Node Node1 { get; set; }
    }

}