﻿namespace Icfpc2013
{
    internal class NodeOp2And : NodeOp2
    {
        public override long Eval()
        {
            return Node0.Eval() & Node1.Eval();
        }
    }
}