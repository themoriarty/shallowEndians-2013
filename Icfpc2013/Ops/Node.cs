namespace Icfpc2013
{
    internal interface Node
    {
        #region Public Methods and Operators

        Node Clone();

        long Size();

        long Eval(ExecContext context);

        string Serialize();
        string ToString(int indentLevel);

        #endregion
    }
}