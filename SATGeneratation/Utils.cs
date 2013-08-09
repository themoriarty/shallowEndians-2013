using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    class Utils
    {
        public static SatExpression GetBit(int bitIndex, int opcode, string nodePrefix)
        {
            return new PosLeafExpresssion { VarName = nodePrefix + bitIndex };
        }

        public static AndExpression GetBitsForOpCode(string nodePrefix, int opcode)
        {
            return new AndExpression
            {
                Exp1 = GetBit(0, opcode, nodePrefix),
                Exp2 = new AndExpression
                {
                    Exp1 = GetBit(1, opcode, nodePrefix),
                    Exp2 = new AndExpression
                    {
                        Exp1 = GetBit(2, opcode, nodePrefix),
                        Exp2 = GetBit(3, opcode, nodePrefix)
                    }
                }
            };
        }

        public static SatExpression GetBitsFromNumber(string nodePrefix, ulong number, int curOffset)
        {
            string varName = nodePrefix + curOffset;
            ulong mask = (ulong)0x1 << curOffset;
            AndExpression res = new AndExpression();
            if (((mask & number) > 0))
            {
                res.Exp1 = new PosLeafExpresssion { VarName = varName };
            }
            else
            {
                res.Exp1 = new NegLeafExpresssion { VarName = varName };
            }
            if (curOffset == 63)
            {

                mask = mask << 1;
                if (((mask & number) > 0))
                {
                    res.Exp2 = new PosLeafExpresssion { VarName = varName };
                }
                else
                {
                    res.Exp2 = new NegLeafExpresssion { VarName = varName };
                }
            }
            else
            {
                res.Exp2 = GetBitsFromNumber(nodePrefix, number, curOffset + 1);
            }
            return res;
        }
    }
}
