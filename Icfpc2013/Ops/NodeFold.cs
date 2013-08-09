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

        public long Eval(ExecContext context)
        {
            throw new NotImplementedException();
        }

        public long Cost()
        {
            return 1 + Node0.Cost() + Node1.Cost() + Node2.Node0.Cost();
        }
    }
}