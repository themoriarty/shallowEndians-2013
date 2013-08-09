﻿namespace Icfpc2013
{
    internal class NodeOp1Shr4 : NodeOp1
    {
        public override long Eval(ExecContext context)
        {
            return Node0.Eval(context)>>4;
        }

        public override Node Clone()
        {
            return new NodeOp1Shr4 { Node0 = Node0.Clone() };
        }
    }
}