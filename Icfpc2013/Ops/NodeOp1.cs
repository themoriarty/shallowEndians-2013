namespace Icfpc2013
{
    using System;

    internal class NodeOp1 : Node
    {
        #region Public Properties

        public Node Node0 { get; set; }

        #endregion

        #region Public Methods and Operators

        public virtual Node Clone()
        {
            throw new NotImplementedException();
        }

        public long Cost()
        {
            return 1 + Node0.Cost();
        }

        public virtual long Eval(ExecContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}