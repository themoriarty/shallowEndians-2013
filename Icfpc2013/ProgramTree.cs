namespace Icfpc2013
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Icfpc2013.Ops;

    public class ProgramTree
    {
        #region Public Properties

        public Lambda1 Program { get; set; }

        #endregion

        #region Public Methods and Operators

        public static ProgramTree Parse(string input)
        {
            return new ProgramTree { Program = Lambda1.Parse(input) };
        }

        public ProgramTree Clone()
        {
            return new ProgramTree { Program = Program.Clone() };
        }

        public int GetHashCode(List<ulong> outputs)
        {
            return outputs.Select(x => x.GetHashCode()).Sum();
        }

        public long Run(long value)
        {
            var state = new ExecContext();
            state.Vars[Program.Id0.Name] = value;

            return Program.Node0.Eval(state);
        }

        public string Serialize()
        {
            return Program.Serialize();
        }

        #endregion
    }
}