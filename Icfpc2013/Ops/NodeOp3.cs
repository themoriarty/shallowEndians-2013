namespace Icfpc2013.Ops
{
    using System;

    public class NodeOp3 : Node
    {
        #region Public Properties

        public virtual OpTypes GetMainOp()
        {
            throw new Exception("blah");
        }


        public Node Node0 { get; set; }

        public Node Node1 { get; set; }

        public Node Node2 { get; set; }

        #endregion

        #region Public Methods and Operators

        public static NodeOp3 Parse(string input)
        {
            throw new NotImplementedException();
            //int pos = 1;
            //string token1 = Parser.ReadToken(input, ref pos, input.Length);
            //string token2 = Parser.ReadToken(input, ref pos, input.Length);
            //string token3 = Parser.ReadToken(input, ref pos, input.Length);

            //if (string.IsNullOrEmpty(token1) || string.IsNullOrEmpty(token2) || string.IsNullOrEmpty(token3))
            //{
            //    throw new Exception("format");
            //}

            //NodeOp2 result = null;

            //switch (token1)
            //{
            //    case "xor":
            //        result = new NodeOp2Xor();
            //        break;
            //    case "and":
            //        result = new NodeOp2And();
            //        break;
            //    case "or":
            //        result = new NodeOp2Or();
            //        break;
            //    case "plus":
            //        result = new NodeOp2Plus();
            //        break;
            //    default:
            //        throw new Exception("format");
            //}

            //result.Node0 = Parser.Parse(token2);
            //result.Node1 = Parser.Parse(token3);

            //return result;
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
            return 1 + (Node0 == null ? 0 : Node0.Size()) + (Node1 == null ? 0 : Node1.Size()) + (Node2 == null ? 0 : Node2.Size());
        }

        public virtual string ToString(int indentLevel)
        {
            throw new NotImplementedException();
        }

        public virtual void Op(ref OpTypes ops)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}