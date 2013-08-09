namespace Icfpc2013
{
    using System;

    internal class NodeFold : Node
    {
        #region Public Properties

        public Node Node0 { get; set; }

        public Node Node1 { get; set; }

        public Lambda2 Node2 { get; set; }

        #endregion

        #region Public Methods and Operators

        public Node Clone()
        {
            return new NodeFold { Node0 = Node0.Clone(), Node1 = Node1.Clone(), Node2 = Node2.Clone() };
        }

        public long Cost()
        {
            return 1 + Node0.Cost() + Node1.Cost() + Node2.Node0.Cost();
        }

        public long Eval(ExecContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}