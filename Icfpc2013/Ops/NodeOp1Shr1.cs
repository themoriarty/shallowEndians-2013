﻿namespace Icfpc2013
{
    internal class NodeOp1Shr1 : NodeOp1
    {
        public override long Eval()
        {
            return Node0.Eval()>>1;
        }
    }
}