namespace Icfpc2013.Ops
{
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
            int pos = 1;
            string token1 = Parser.ReadToken(input, ref pos, input.Length);

            switch (token1)
            {
                case "fold":

                    break;
            }

            return null;
        }

        #endregion
    }
}