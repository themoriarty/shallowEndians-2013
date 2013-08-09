namespace Icfpc2013
{
    class NodeOp1 : Node
    {
        public Node Node0 { get; set; }

        public virtual long Eval()
        {
            throw new System.NotImplementedException();
        }

        public long Cost()
        {
            return 1 + Node0.Cost();
        }

        public string ToString(int indentLevel)
        {
            throw new System.NotImplementedException();
        }
    }
}