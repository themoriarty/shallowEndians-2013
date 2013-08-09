namespace Icfpc2013
{
    class NodeOp2 : Node
    {
        public virtual long Eval()
        {
            throw new System.NotImplementedException();
        }

        public long Cost()
        {
            return 1 + Node0.Cost() + Node1.Cost();
        }

        public Node Node0 { get; set; }

        public Node Node1 { get; set; }
    }

}