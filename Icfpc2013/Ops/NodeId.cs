namespace Icfpc2013.Ops
{
    using System;
    using System.Text.RegularExpressions;

    public class NodeId : Node
    {
        #region Static Fields

        private static readonly Regex nameRegex = new Regex("[a-z][a-z_0-9]*", RegexOptions.Compiled);

        #endregion

        #region Public Properties

        public string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        public static NodeId Parse(string input)
        {
            if (nameRegex.Match(input) == null)
            {
                throw new Exception("format");
            }

            return new NodeId { Name = input };
        }

        public Node Clone()
        {
            return new NodeId { Name = this.Name };
        }

        public long Eval(ExecContext context)
        {
            long value;
            if (!context.Vars.TryGetValue(this.Name, out value))
            {
                throw new Exception("Var " + this.Name + " is undefined");
            }

            return value;
        }

        public string Serialize()
        {
            return this.Name;
        }

        public long Size()
        {
            return 1;
        }

        public string ToString(int indentLevel)
        {
            return this.Name;
        }

        #endregion
    }
}