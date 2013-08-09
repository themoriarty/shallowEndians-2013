using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icfpc2013
{
    class Random
    {
        public static System.Random Prng = new System.Random(0);

        public static ulong GetRandomULong()
        {
            var buffer = new byte[sizeof(ulong)];
            Prng.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }
    }
}
