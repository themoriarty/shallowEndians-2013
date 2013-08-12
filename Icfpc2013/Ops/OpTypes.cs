namespace Icfpc2013.Ops
{
    using System;

    [Flags]
    public enum OpTypes : uint
    {
        none = 0,

        if0 = 1, 

        tfold = 2, 

        fold = 4, 

        not = 8, 

        shl1 = 16, 

        shr1 = 32, 

        shr4 = 64, 

        shr16 = 128, 

        and = 256, 

        or = 512, 

        xor = 1024, 

        plus = 2048,

        bonus = 4096,

        // Used for caching:

        zero = 8192,

        one = 16384,

        x =  32768
    }
}