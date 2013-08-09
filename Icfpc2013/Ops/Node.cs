namespace Icfpc2013.Ops
{
    public interface Node
    {
        #region Public Methods and Operators

        Node Clone();

        long Eval(ExecContext context);

        string Serialize();

        long Size();

        string ToString(int indentLevel);

        #endregion
    }
}