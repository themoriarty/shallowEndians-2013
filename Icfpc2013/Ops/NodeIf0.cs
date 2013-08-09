namespace Icfpc2013
{
    internal class NodeIf0 : Node
    {
        #region Public Properties

        public Node Node0 { get; set; }

        public Node Node1 { get; set; }

        public Node Node2 { get; set; }

        #endregion

        #region Public Methods and Operators

        public Node Clone()
        {
            return new NodeIf0 { Node0 = Node0.Clone(), Node1 = Node1.Clone(), Node2 = Node2.Clone() };
        }

        public long Cost()
        {
            return 1 + Node0.Cost() + Node1.Cost() + Node2.Cost();
        }

        public long Eval(ExecContext context)
        {
            if (Node0.Eval(context) == 0)
            {
                return Node1.Eval(context);
            }

            return Node2.Eval(context);
        }

        #endregion
    }
}