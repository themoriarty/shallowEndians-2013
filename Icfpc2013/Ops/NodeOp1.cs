namespace Icfpc2013.Ops
{
    using System;

    public class NodeOp1 : Node
    {
        #region Public Properties

        public Node Node0 { get; set; }

        #endregion

        #region Public Methods and Operators

        public static NodeOp1 Parse(string input)
        {
            int pos = 1;
            string token1 = Parser.ReadToken(input, ref pos, input.Length);
            string token2 = Parser.ReadToken(input, ref pos, input.Length);

            if (string.IsNullOrEmpty(token1) || string.IsNullOrEmpty(token2))
            {
                throw new Exception("format");
            }

            NodeOp1 result = null;

            switch (token1)
            {
                case "not":
                    result = new NodeOp1Not();
                    break;
                case "shl1":
                    result = new NodeOp1Shl1();
                    break;
                case "shr1":
                    result = new NodeOp1Shr1();
                    break;
                case "shr4":
                    result = new NodeOp1Shr4();
                    break;
                case "shr16":
                    result = new NodeOp1Shr16();
                    break;
                default:
                    throw new Exception("format");
            }

            result.Node0 = Parser.Parse(token2);

            return result;
        }

        public virtual Node Clone()
        {
            throw new NotImplementedException();
        }

        public virtual ulong Eval(ExecContext context)
        {
            throw new NotImplementedException();
        }

        public virtual string Serialize()
        {
            throw new NotImplementedException();
        }

        public long Size()
        {
            return 1 + this.Node0.Size();
        }

        public virtual string ToString(int indentLevel)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}