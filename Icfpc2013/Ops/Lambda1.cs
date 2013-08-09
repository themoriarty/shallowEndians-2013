namespace Icfpc2013
{
    internal class Lambda1
    {
        public NodeId Id0 { get; set; }

        public Node Node0 { get; set; }

        public Lambda1 Clone()
        {
            return new Lambda1 { Id0 = (NodeId)Id0.Clone(), Node0 = Node0.Clone() };
        }

    }
}