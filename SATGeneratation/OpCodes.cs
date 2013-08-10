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
        Input = 2,
        And = 3,
        Or = 4,
        Xor = 5,
        Plus = 6,
        Not = 7,
        Shl1 = 8,
        Shr1 = 9,
        Shr4 = 10,
        Shr16 = 11,
        If0 = 12
    }
}
