namespace Icfpc2013
{
    internal class ProgramTree
    {
        #region Public Properties

        public Lambda1 Program { get; set; }

        #endregion

        #region Public Methods and Operators

        public ProgramTree Clone()
        {
            return new ProgramTree { Program = Program.Clone() };
        }

        public long Run(long value)
        {
            var state = new ExecContext();
            state.Vars[Program.Id0.Name] = value;

            return Program.Node0.Eval(state);
        }

        #endregion
    }
}