namespace Icfpc2013
{
    internal interface Node
    {
        #region Public Methods and Operators

        Node Clone();

        long Cost();

        long Eval(ExecContext context);

        #endregion
    }
}