namespace Icfpc2013
{
    internal class ProgramTree
    {
        public Lambda1 Program { get; set; } 

        public long Run(long value)
        {
            var state = new ExecContext();
            state.Vars[Program.Id0.Name] = value;

            return Program.Node0.Eval(state);
        }

        public ProgramTree Clone()
        {
            return new ProgramTree { Program = Program.Clone() };
        }
    }
}