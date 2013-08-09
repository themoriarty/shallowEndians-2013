namespace Icfpc2013.Ops
{
    using System;

    public static class Parser
    {
        #region Public Methods and Operators

        public static string ReadToken(string input, ref int pos, int end)
        {
            int pos0 = pos;
            int nesting = 0;

            while (pos < end)
            {
                var value = input[pos++];

                if (value == '(')
                {
                    ++nesting;
                }

                if (value == ')')
                {
                    --nesting;
                }

                if (value == ' ' && nesting == 0)
                {
                    break;
                }
            }

            if (pos == pos0)
            {
                return null;
            }

            return input.Substring(pos0, pos - pos0 - 1);
        }

        public static Node Parse(string input)
        {
            int pos = input.StartsWith("(") ? 1 : 0;
            string token1 = Parser.ReadToken(input, ref pos, input.Length);

            switch (token1)
            {
                case "fold":
                    return NodeFold.Parse(input);
                case "xor":
                case "or":
                case "and":
                case "plus":
                    return NodeOp2.Parse(input);
                case "not":
                case "shl1":
                case "shr1":
                case "shr4":
                case "shr16":
                    return NodeOp1.Parse(input);
                case "if0":
                    return NodeIf0.Parse(input);
                case "0":
                    return new Node0();
                case "1":
                    return new Node1();
                default:
                    if (input == "0")
                    {
                        return new Node0();
                    }
                    else if (input == "1")
                    {
                        return new Node1();
                    }
                    else
                    {
                        return NodeId.Parse(input);
                    }
            }

            return null;
        }

        #endregion
    }
}