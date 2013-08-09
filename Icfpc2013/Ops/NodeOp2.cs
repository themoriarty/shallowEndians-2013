namespace Icfpc2013
{
    using System;

    internal class NodeOp2 : Node
    {
        #region Public Properties

        public Node Node0 { get; set; }

        public Node Node1 { get; set; }

        #endregion

        #region Public Methods and Operators

        public virtual Node Clone()
        {
            throw new NotImplementedException();
        }

        public long Cost()
        {
            return 1 + Node0.Cost() + Node1.Cost();
        }

        public virtual long Eval(ExecContext context)
        {
            throw new NotImplementedException();
        }

        public virtual string Serialize()
        {
            throw new NotImplementedException();
        }
        
        public virtual string ToString(int indentLevel)
        {
            throw new System.NotImplementedException();
        }


        #endregion
    }
}