using System.Collections.Generic;

namespace Icfpc2013
{
    internal class ProgramTree
    {
        private List<Node> ValidNodes;
        public ProgramTree(List<Node> validNodes)
        {
            ValidNodes = validNodes;
        }

        //static Node[] AllNodes = { new Node0(), new Node1(), new NodeIf0(), 
        //                             new NodeOp1Not(), new NodeOp1Shl1(), 
        //                             new NodeOp1Shr1(), new NodeOp1Shr16(), 
        //                             new NodeOp1Shr4(), new NodeOp2And(), 
        //                             new NodeOp2Or(), new NodeOp2Plus(), new NodeOp2Xor()};
        void GenerateNextLevel(out List<Node> thisLevel, out List<Node> nextLevel)
        {
            thisLevel = new List<Node>();
            foreach (var n in ValidNodes)
            {
                //thisLevel.Add(
            }
        }

        void DoSomething()
        {
            Node root = new Node0();
            foreach (var node in ValidNodes)
            {
                var child = root.Clone();

            }
            for (int level = 0; level < 5; level++)
            {
                List<Node> thisLevel = null;
                List<Node> nextLevel = null;
                GenerateNextLevel(thisLevel, nextLevel);
                foreach (var node in thisLevel)
                {
                    Node newNode = root.Copy();
                }
            }
        }
    }
}