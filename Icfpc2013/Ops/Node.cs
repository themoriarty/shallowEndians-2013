namespace Icfpc2013
{
    interface Node
    {
        long Eval(ExecContext context);

        long Cost();
    }
}