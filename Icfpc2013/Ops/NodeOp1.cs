namespace Icfpc2013.Ops
{
    using System;

    public class NodeOp1 : Node
    {
        #region Public Properties

        public Node Node0 { get; set; }

        #endregion

        #region Public Methods and Operators

        public virtual Node Clone()
        {
            throw new NotImplementedException();
        }

        public long Size()
        {
            return 1 + this.Node0.Size();
        }

        public virtual string ToString(int indentLevel)
        {
            throw new System.NotImplementedException();
        }
        public virtual long Eval(ExecContext context)
        {
            throw new NotImplementedException();
        }

        public virtual string Serialize()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}