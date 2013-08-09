using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icfpc2013
{
    class TreeGenerator
    {
        private List<Node> ValidNodes;
        private int MaxDepth;

        public TreeGenerator(List<Node> validNodes, int maxDepth)
        {
            ValidNodes = validNodes;
            maxDepth = MaxDepth;
        }

        List<Node> GetNLevelTree(Node root, int depth)
        {
            if (root is Node0 || root is Node1)
            {
                List<Node> ret = new List<Node>();
                ret.Add(root);
                return ret;
            }
            if (depth == 0)
            {
                return null;
            }
            if (root is NodeOp1)
            {
                List<Node> subtrees = new List<Node>();
                foreach (var node in ValidNodes)
                {
                    var subtree = GetNLevelTree(node, depth - 1);
                    if (subtree != null)
                    {
                        subtrees.AddRange(subtree);
                    }
                }
                List<Node> ret = new List<Node>();
                foreach (var subtree in subtrees)
                {
                    root.Clone();
                    (root as NodeOp1).Node0 = subtree;
                }
                return ret;
            }
            throw new Exception("unknown node");
        }

        //static Node[] AllNodes = { new Node0(), new Node1(), new NodeIf0(), 
        //                             new NodeOp1Not(), new NodeOp1Shl1(), 
        //                             new NodeOp1Shr1(), new NodeOp1Shr16(), 
        //                             new NodeOp1Shr4(), new NodeOp2And(), 
        //                             new NodeOp2Or(), new NodeOp2Plus(), new NodeOp2Xor()};
        #region Public Properties

        public Lambda1 Program { get; set; }

        #endregion

        #region Public Methods and Operators

        public List<Node> GenerateAllPrograms()
        {
            List<Node> ret = new List<Node>();
            foreach (var node in ValidNodes)
            {
                var subtree = GetNLevelTree(node, MaxDepth);
                if (subtree != null)
                {
                    ret.AddRange(subtree);
                }
            }
            return ret;
        }
    }
}
