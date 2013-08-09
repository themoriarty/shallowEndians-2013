namespace Icfpc2013.Ops
{
    using System;

    public class NodeIf0 : Node
    {
        #region Public Properties

        public Node Node0 { get; set; }

        public Node Node1 { get; set; }

        public Node Node2 { get; set; }

        #endregion

        #region Public Methods and Operators

        public static NodeIf0 Parse(string input)
        {
            int pos = 1;
            string token1 = Parser.ReadToken(input, ref pos, input.Length);
            string token2 = Parser.ReadToken(input, ref pos, input.Length);
            string token3 = Parser.ReadToken(input, ref pos, input.Length);
            string token4 = Parser.ReadToken(input, ref pos, input.Length);

            if (!string.Equals(token1, "if0") || string.IsNullOrEmpty(token2) || string.IsNullOrEmpty(token3) || string.IsNullOrEmpty(token4))
            {
                throw new Exception("format");
            }

            return new NodeIf0 { Node0 = Parser.Parse(token2), Node1 = Parser.Parse(token3), Node2 = Parser.Parse(token4) };
        }

        public Node Clone()
        {
            return new NodeIf0 { Node0 = this.Node0.Clone(), Node1 = this.Node1.Clone(), Node2 = this.Node2.Clone() };
        }

        public ulong Eval(ExecContext context)
        {
            if (this.Node0.Eval(context) == 0)
            {
                return this.Node1.Eval(context);
            }

            return this.Node2.Eval(context);
        }

        public string Serialize()
        {
            return string.Format("(if0 {0} {1} {2})", this.Node0.Serialize(), this.Node1.Serialize(), this.Node2.Serialize());
        }

        public long Size()
        {
            return 1 + this.Node0.Size() + this.Node1.Size() + this.Node2.Size();
        }

        public string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "  ";
            }

            output += "( ";
            output += "if0 " + this.Node0.ToString(indentLevel + 1) + " ";
            output += this.Node1.ToString(indentLevel + 1) + " ";
            output += this.Node2.ToString(indentLevel + 1);
            output += " )";
            return output;
        }

        public void Op(ref OpTypes ops)
        {
            ops |= OpTypes.if0;
            Node0.Op(ref ops);
            Node1.Op(ref ops);
            Node2.Op(ref ops);
        }

        #endregion
    }
}