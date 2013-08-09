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

        public ulong Run(ulong value)
        {
            var state = new ExecContext();
            state.Vars[Program.Id0.Name] = value;

            return Program.Node0.Eval(state);
        }

        public string Serialize()
        {
            return Program.Serialize();
        }

        public OpTypes Op()
        {
            return Program.Op();
        }

        public static OpTypes GetOpTypes(IEnumerable<string> operators)
        {
            return Parser.ParseOps(operators).Aggregate(OpTypes.none, (seed, value) => seed | value);
        }

        public static List<Node> GetAvailableNodes(OpTypes ops)
        {
            var validNodes = new List<Node>();
            var inputArg = new NodeId();
            inputArg.Name = "x";
            validNodes.Add(inputArg);
            validNodes.Add(new Node0());
            validNodes.Add(new Node1());

            if ((ops & OpTypes.not) != OpTypes.none)
            {
                validNodes.Add(new NodeOp1Not());  
            }

            if ((ops & OpTypes.shl1) != OpTypes.none)
            {
                validNodes.Add(new NodeOp1Shl1());
            }

            if ((ops & OpTypes.shr1) != OpTypes.none)
            {
                validNodes.Add(new NodeOp1Shr1());
            }

            if ((ops & OpTypes.shr4) != OpTypes.none)
            {
                validNodes.Add(new NodeOp1Shr4());
            }

            if ((ops & OpTypes.shr16) != OpTypes.none)
            {
                validNodes.Add(new NodeOp1Shr16());
            }

            if ((ops & OpTypes.and) != OpTypes.none)
            {
                validNodes.Add(new NodeOp2And());
            }

            if ((ops & OpTypes.or) != OpTypes.none)
            {
                validNodes.Add(new NodeOp2Or());
            }

            if ((ops & OpTypes.xor) != OpTypes.none)
            {
                validNodes.Add(new NodeOp2Xor());
            }

            if ((ops & OpTypes.plus) != OpTypes.none)
            {
                validNodes.Add(new NodeOp2Plus());
            }

            return validNodes;
        }

        #endregion
    }
}