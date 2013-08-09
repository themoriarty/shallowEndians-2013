using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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

        public int GetHashCodeFromInputs(List<ulong> inputs)
        {
            List<ulong> outputs = inputs.Select(x => Run(x));
            return GetHashCodeFromOutputs(outputs);
        }

        public int GetHashCodeFromOutputs(List<ulong> outputs)
        {
            return outputs.Select(x => x.GetHashCode()).Sum();
        }

        public List<ulong> GetInputVectorList(ulong level /* = 0 to 6 */)
        {
            var result = new List<ulong>();

            // First 64 values
            ulong upperBound = level > 3 ? 64 : 16 + 16*level;
            for (ulong i = 0; i < upperBound; i++) // 63 values
            {
                result.Add(i);
            }

            // Powers of two, +-1
            if (level >= 4)
            {
                for (int power = 6; power < 64; power++) // 57*3 = 171 values
                {
                    int shift = power - 1;
                    ulong x = 2UL << shift;
                    result.Add(x - 1);
                    result.Add(x);
                    result.Add(x + 1);
                }
            }

            // Single hex digit repeated: 15 values
            if (level >= 5)
            {
                result.Add(0x1111111111111111);
                result.Add(0x2222222222222222);
                result.Add(0x3333333333333333);
                result.Add(0x4444444444444444);
                result.Add(0x5555555555555555);
                result.Add(0x6666666666666666);
                result.Add(0x7777777777777777);
                result.Add(0x8888888888888888);
                result.Add(0x9999999999999999);
                result.Add(0xaaaaaaaaaaaaaaaa);
                result.Add(0xbbbbbbbbbbbbbbbb);
                result.Add(0xcccccccccccccccc);
                result.Add(0xdddddddddddddddd);
                result.Add(0xeeeeeeeeeeeeeeee);
                result.Add(0xffffffffffffffff);
                result.Add(ulong.MaxValue);
            }

            // Byte-sized shifts of 0xff
            if (level >= 6)
            {
                for (int bShift = 0; bShift < 8; bShift++) // 8 values
                {
                    const ulong ff = 0xff;
                    result.Add(ff << 8*bShift);
                }
            }
            // More bit patterns: 4 values
            //result.Add(0x1010101010101010); 
            //result.Add(0x0101010101010101); 
            //result.Add(0x1100110011001100); 
            //result.Add(0x0011001100110011); 

            return result;
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