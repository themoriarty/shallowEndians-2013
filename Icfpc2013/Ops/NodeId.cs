namespace Icfpc2013
{
    using System;

    internal class NodeId : Node
    {
        #region Public Properties

        public string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        public Node Clone()
        {
            return new NodeId { Name = Name };
        }

        public long Cost()
        {
            return 1;
        }

        public long Eval(ExecContext context)
        {
            long value;
            if (!context.Vars.TryGetValue(Name, out value))
            {
                throw new Exception("Var " + Name + " is undefined");
            }

            return value;
        }

        public string Serialize()
        {
            return Name;
        }

        #endregion
    }
}