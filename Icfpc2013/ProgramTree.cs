using System.Diagnostics;

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

        public int GetHashCodeFromInputs(List<ulong> inputs)
        {
            List<ulong> outputs = inputs.Select(Run).ToList();
            return GetHashCodeFromOutputs(outputs);
        }

        public int GetHashCodeFromOutputs(List<ulong> outputs)
        {
            return outputs.Select(x => x.GetHashCode()).Sum();
        }

        /// <summary>
        /// Returns up to 260 vectors.
        /// </summary>
        public static List<ulong> GetInputVectorList(ulong maxCount)
        {
            var result = new List<ulong>();
            ulong prevCount = 0;

            if (maxCount < 16)
            {
                if (maxCount > 0)
                {
                    result.Add(0);
                }
                if (maxCount > 1)
                {
                    result.Add(ulong.MaxValue);
                }
                for (ulong i = 2; i < maxCount; i++)
                {
                    result.Add(Random.GetRandomULong());
                }
                prevCount = Math.Min(15, maxCount);
            }

            // First 64 values
            if (maxCount > prevCount)
            {
                ulong upperBound = Math.Min(64, maxCount);
                for (ulong i = 0; i < upperBound; i++) // 64 values
                {
                    result.Add(i);
                }
                prevCount = upperBound;
            }

            // Powers of two, +-1
            if (maxCount > prevCount)
            {
                ulong start = 6;
                ulong upperBound = Math.Min((64 - start)*3, maxCount - prevCount)/3;                
                for (ulong i = 0; i < upperBound; i++) // 58*3 = 174 values -1
                {
                    int shift = (int)(start + i - 1);
                    ulong x = 2UL << shift;
                    if (i > 0)
                        result.Add(x - 1);
                    result.Add(x);
                    result.Add(x + 1);
                }
                prevCount += upperBound*3 - 1;
            }

            // Single hex digit repeated: 15 values
            if (maxCount > prevCount)
            {
                ulong seed = 0;
                ulong inc = 0x1111111111111111;
                ulong stutterCount = Math.Min(15, maxCount - prevCount);
                for (ulong i = 0; i < stutterCount; i++)
                {
                    seed += inc;
                    result.Add(seed);    
                }              
                prevCount += stutterCount;
            }

            // Byte-sized shifts of 0xff
            if (maxCount > prevCount)
            {
                ulong localCount = Math.Min(8, maxCount - prevCount);
                for (ulong bShift = 0; bShift < localCount; bShift++) // 8 values
                {
                    const ulong ff = 0xff;
                    result.Add(ff << (8 * (int) bShift));
                }
            }

            return result;
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

        public long Size()
        {
            return Program.Size();
        }

        public static OpTypes GetOpTypes(IEnumerable<string> operators)
        {
            return Parser.ParseOps(operators).Aggregate(OpTypes.none, (seed, value) => seed | value);
        }

        public static List<Node> GetAvailableNodes(OpTypes ops, bool tfoldMode)
        {
            var validNodes = new List<Node>();
            if (tfoldMode)
            {
                validNodes.Add(new NodeId { Name = "x1" });
                validNodes.Add(new NodeId { Name = "x2" });
            }
            else
            {
                validNodes.Add(new NodeId{Name = "x" });
            }
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