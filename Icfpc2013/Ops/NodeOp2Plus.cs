namespace Icfpc2013.Ops
{
    public class NodeOp2Plus : NodeOp2
    {
        #region Public Methods and Operators

        public override Node Clone()
        {
            return new NodeOp2Plus { Node0 = this.Node0.Clone(), Node1 = this.Node1.Clone() };
        }

        public override long Eval(ExecContext context)
        {
            return this.Node0.Eval(context) + this.Node1.Eval(context);
        }

        public override string Serialize()
        {
            return string.Format("(plus {0} {1})", this.Node0.Serialize(), this.Node1.Serialize());
        }        
        
        public override string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }
            output += "plus " + this.Node0.ToString(indentLevel + 1) + " ";
            output += this.Node1.ToString(indentLevel + 1) + " )";
            return output;
        }
        #endregion
    }
}