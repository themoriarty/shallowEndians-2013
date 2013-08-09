namespace Icfpc2013
{
    internal class Lambda2
    {
        #region Public Properties

        public NodeId Id0 { get; set; }

        public NodeId Id1 { get; set; }

        public Node Node0 { get; set; }

        #endregion

        #region Public Methods and Operators

        public Lambda2 Clone()
        {
            return new Lambda2 { Id0 = (NodeId)Id0.Clone(), Id1 = (NodeId)Id1.Clone(), Node0 = Node0.Clone() };
        }

        public long Eval(long value1, long value2)
        {
            var state = new ExecContext();
            state.Vars[Id0.Name] = value1;
            state.Vars[Id1.Name] = value2;

            return Node0.Eval(state);
        }

        public override string ToString()
        {
            return string.Format("(lambda ({0} {1}) {2})", Id0, Id1, Node0);
        }

        #endregion
    }
}