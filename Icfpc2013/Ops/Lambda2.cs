namespace Icfpc2013.Ops
{
    public class Lambda2
    {
        #region Public Properties

        public NodeId Id0 { get; set; }

        public NodeId Id1 { get; set; }

        public Node Node0 { get; set; }

        public string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }
            output += "( ";
            output += "lambda " + this.Id0.ToString(indentLevel + 1) + " " + this.Id1.ToString(indentLevel + 1) + " ";
            output += this.Node0.ToString(indentLevel + 1) + " )";
            return output;
        }
        #endregion

        #region Public Methods and Operators

        public Lambda2 Clone()
        {
            return new Lambda2 { Id0 = (NodeId)this.Id0.Clone(), Id1 = (NodeId)this.Id1.Clone(), Node0 = this.Node0.Clone() };
        }

        public long Eval(long value1, long value2)
        {
            var state = new ExecContext();
            state.Vars[this.Id0.Name] = value1;
            state.Vars[this.Id1.Name] = value2;

            return this.Node0.Eval(state);
        }

        public string Serialize()
        {
            return string.Format("(lambda ({0} {1}) {2})", this.Id0.Serialize(), this.Id1.Serialize(), this.Node0.Serialize());
        }

        #endregion
    }
}