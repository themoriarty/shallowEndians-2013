namespace Icfpc2013.Ops
{
    using System;

    public class Lambda1
    {
        #region Public Properties

        public NodeId Id0 { get; set; }

        public Node Node0 { get; set; }

        #endregion

        #region Public Methods and Operators

        public static Lambda1 Parse(string input)
        {
            int pos = 1;
            string token1 = Parser.ReadToken(input, ref pos, input.Length);
            string token2 = Parser.ReadToken(input, ref pos, input.Length);
            string token3 = Parser.ReadToken(input, ref pos, input.Length);

            if (!string.Equals(token1, "lambda") || string.IsNullOrEmpty(token2) || string.IsNullOrEmpty(token3) || !token2.StartsWith("(") || !token2.EndsWith(")"))
            {
                throw new Exception("format");
            }

            var result = new Lambda1();

            result.Id0 = NodeId.Parse(token2.Substring(1, token2.Length - 2));
            result.Node0 = Parser.Parse(token3);

            return result;
        }

        public Lambda1 Clone()
        {
            return new Lambda1 { Id0 = (NodeId)this.Id0.Clone(), Node0 = this.Node0.Clone() };
        }

        public string Serialize()
        {
            return string.Format("(lambda ({0}) {1})", this.Id0.Serialize(), this.Node0.Serialize());
        }

        public long Size()
        {
            return 1 + Node0.Size();
        }

        public string ToString(int indentLevel)
        {
            string output = "\n";
            for (int i = 0; i < indentLevel; ++i)
            {
                output += "\t";
            }

            output += "( ";
            output += "lambda " + this.Id0.ToString(indentLevel + 1) + " ";
            output += this.Node0.ToString(indentLevel + 1) + " )";
            return output;
        }

        public OpTypes Op()
        {
            var ops = OpTypes.none;

            Node0.Op(ref ops);

            return ops;
        }

        #endregion
    }
}