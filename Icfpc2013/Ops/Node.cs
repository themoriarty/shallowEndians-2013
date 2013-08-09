namespace Icfpc2013.Ops
{
    using System.Collections.Generic;

    public interface Node
    {
        #region Public Methods and Operators

        Node Clone();

        ulong Eval(ExecContext context);

        string Serialize();

        long Size();

        string ToString(int indentLevel);

        void Op(ref OpTypes ops);

        #endregion
    }
}