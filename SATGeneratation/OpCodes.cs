using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATGeneratation
{
    public enum OpCodes
    {
        Zero = 0,
        One = 1,
        Input2 = 2,
        Input = 3,
        And = 4,
        Or = 5,
        Xor = 6,
        Plus = 7,
        Not = 8,
        Shl1 = 9,
        Shr1 = 10,
        Shr4 = 11,
        Shr16 = 12,
        If0 = 13
    }
}
