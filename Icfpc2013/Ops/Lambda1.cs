namespace Icfpc2013
{
    internal class Lambda1
    {
        #region Public Properties

        public NodeId Id0 { get; set; }

        public Node Node0 { get; set; }

        #endregion

        #region Public Methods and Operators

        public Lambda1 Clone()
        {
            return new Lambda1 { Id0 = (NodeId)Id0.Clone(), Node0 = Node0.Clone() };
        }

        public override string ToString()
        {
            return string.Format("(lambda ({0}) {1})", Id0, Node0);
        }

        #endregion
    }
}