﻿namespace Icfpc2013
{
    internal class NodeIf0 : Node
    {
        #region Public Properties

        public Node Node0 { get; set; }

        public Node Node1 { get; set; }

        public Node Node2 { get; set; }

        #endregion

        public long Eval(ExecContext context)
        {
            if (Node0.Eval(context) == 0)
            {
                return Node1.Eval(context);
            }

            return Node2.Eval(context);
        }

        public long Cost()
        {
            return 1 + Node0.Cost() + Node1.Cost() + Node2.Cost();
        }
    }
}