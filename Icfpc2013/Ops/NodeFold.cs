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
            return 2 + Node0.Cost() + Node1.Cost() + Node2.Node0.Cost();
        }

        public long Eval(ExecContext context)
        {
            var input = Node0.Eval(context);
            var init = Node1.Eval(context);

            var current = init;

            for (int i = 0; i < 8; ++i)
            {
                var left = (input >> (i * 8)) & 0x00ff;

                current = Node2.Eval(left, current);
            }

            return current;
        }

        #endregion
    }
}