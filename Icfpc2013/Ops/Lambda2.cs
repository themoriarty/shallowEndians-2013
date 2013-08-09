namespace Icfpc2013
{
    internal class Lambda2
    {
        public NodeId Id0 { get; set; }
        public NodeId Id1 { get; set; }

        public Node Node0 { get; set; }

        public Lambda2 Clone()
        {
            return new Lambda2 { Id0 = (NodeId)Id0.Clone(), Id1 = (NodeId)Id1.Clone(), Node0 = Node0.Clone() };

        }
    }
}