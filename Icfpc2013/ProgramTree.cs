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

        // TODO: Remove off-by-one errors, which surely exist, because I was falling asleep when I wrote this. -jmc
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
                ulong upperBound = Math.Max(maxCount, 64);
                for (ulong i = 0; i < upperBound; i++) // 64 values
                {
                    result.Add(i);
                }
                prevCount = upperBound;
            }

            // Powers of two, +-1
            if (maxCount > prevCount)
            {
                ulong upperBound = Math.Min(64, maxCount - prevCount - 6);
                for (ulong power = 6; power < upperBound; power++) // 57*3 = 171 values
                {
                    int shift = (int) power - 1;
                    ulong x = 2UL << shift;
                    result.Add(x - 1);
                    result.Add(x);
                    result.Add(x + 1);
                }
                prevCount += upperBound - 6;
            }

            // Single hex digit repeated: 15 values
            if (maxCount > prevCount)
            {
                ulong stutterCount = Math.Max(15, maxCount - prevCount);
                switch (stutterCount)
                {
                    case 1:
                        result.Add(0x1111111111111111);
                        break;
                    case 2:
                        result.Add(0x2222222222222222);
                        break;
                    case 3:
                        result.Add(0x3333333333333333);
                        break;
                    case 4:
                        result.Add(0x4444444444444444);
                        break;
                    case 5:
                        result.Add(0x5555555555555555);
                        break;
                    case 6:
                        result.Add(0x6666666666666666);
                        break;
                    case 7:
                        result.Add(0x7777777777777777);
                        break;
                    case 8:
                        result.Add(0x8888888888888888);
                        break;
                    case 9:
                        result.Add(0x9999999999999999);
                        break;
                    case 10:
                        result.Add(0xaaaaaaaaaaaaaaaa);
                        break;
                    case 11:
                        result.Add(0xbbbbbbbbbbbbbbbb);
                        break;
                    case 12:
                        result.Add(0xcccccccccccccccc);
                        break;
                    case 13:
                        result.Add(0xdddddddddddddddd);
                        break;
                    case 14:
                        result.Add(0xeeeeeeeeeeeeeeee);
                        break;
                    case 15:
                        result.Add(ulong.MaxValue);
                        break;
                    default:
                        Debug.Assert(false, "Internal error in tracking of input vectors returned");
                        break;
                }
                prevCount += stutterCount;
            }

            // Byte-sized shifts of 0xff
            if (maxCount > prevCount)
            {
                ulong localCount = maxCount - prevCount;
                for (ulong bShift = 0; bShift < localCount; bShift++) // 8 values
                {
                    const ulong ff = 0xff;
                    result.Add(ff << 8 * (int) bShift);
                }
            }

            return result;
        }

        public
            ulong Run(ulong value)
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