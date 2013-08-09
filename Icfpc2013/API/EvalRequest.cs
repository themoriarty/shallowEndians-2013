using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icfpc2013
{
    public class EvalRequest
    {
        public string id { get; set; }
        public string program {get;set;}
        public string[] arguments {get;set;}

        public EvalRequest(string id, string program, string[] arguments)
        {
            this.id = id;
            this.program = program;
            this.arguments = arguments;
        }

        public override string ToString()
        {
            return Utils.ToString(this);
        }
    }
}
