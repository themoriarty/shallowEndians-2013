namespace Icfpc2013.Ops
{
    public class NodeFold : Node
    {
        #region Public Properties

        public Node Node0 { get; set; }

        public Node Node1 { get; set; }

        public Lambda2 Node2 { get; set; }

        #endregion

        #region Public Methods and Operators

        public Node Clone()
        {
            return new NodeFold { Node0 = this.Node0.Clone(), Node1 = this.Node1.Clone(), Node2 = this.Node2.Clone() };
        }

        public long Size()
        {
            return 2 + this.Node0.Size() + this.Node1.Size() + this.Node2.Node0.Size();
        }

        public string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }
            output += "( ";
            output += "fold " + this.Node0.ToString(indentLevel + 1) + " ";
            output += this.Node1.ToString(indentLevel + 1) + " ";
            output += this.Node2.ToString(indentLevel + 1) + " )";
            return output;
        }

        public long Eval(ExecContext context)
        {
            var input = this.Node0.Eval(context);
            var init = this.Node1.Eval(context);

            var current = init;

            for (int i = 0; i < 8; ++i)
            {
                var left = (input >> (i * 8)) & 0x00ff;

                current = this.Node2.Eval(left, current);
            }

            return current;
        }

        public string Serialize()
        {
            return string.Format("(fold {0} {1} {2})", this.Node0.Serialize(), this.Node1.Serialize(), this.Node2.Serialize());
        }

        public static NodeFold Parse(string input)
        {
            int pos = 1;
            string token1 = Parser.ReadToken(input, ref pos, input.Length);


            return null;
        }

        #endregion
    }
}