using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icfpc2013
{
    public class Guess
    {
        public string id { get; set; }

        public string program { get; set; }

        public Guess(string id, string program)
        {
            this.id = id;
            this.program = program;
        }

        public override string ToString()
        {
            return Utils.ToString(this);
        }
    }
}
