namespace Icfpc2013.Ops
{
    using System;

    public class Lambda2
    {
        #region Public Properties

        public NodeId Id0 { get; set; }

        public NodeId Id1 { get; set; }

        public Node Node0 { get; set; }

        #endregion

        #region Public Methods and Operators

        public static Lambda2 Parse(string input)
        {
            int pos = 1;
            string token1 = Parser.ReadToken(input, ref pos, input.Length);
            string token2 = Parser.ReadToken(input, ref pos, input.Length);
            string token3 = Parser.ReadToken(input, ref pos, input.Length);

            if (!string.Equals(token1, "lambda") || string.IsNullOrEmpty(token2) || string.IsNullOrEmpty(token3))
            {
                throw new Exception("format");
            }

            pos = 1;
            string tokenId1 = Parser.ReadToken(token2, ref pos, token2.Length);
            string tokenId2 = Parser.ReadToken(token2, ref pos, token2.Length);

            if (string.IsNullOrEmpty(tokenId1) || string.IsNullOrEmpty(tokenId2))
            {
                throw new Exception("format");
            }

            var result = new Lambda2();

            result.Id0 = NodeId.Parse(tokenId1);
            result.Id1 = NodeId.Parse(tokenId2);
            result.Node0 = Parser.Parse(token3);

            return result;
        }

        public Lambda2 Clone()
        {
            return new Lambda2 { Id0 = (NodeId)this.Id0.Clone(), Id1 = (NodeId)this.Id1.Clone(), Node0 = this.Node0.Clone() };
        }

        public ulong Eval(ulong value1, ulong value2)
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

        public long Size()
        {
            return Node0.Size();
        }

        #endregion
    }
}