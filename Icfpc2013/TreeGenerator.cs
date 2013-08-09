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
            MaxDepth = maxDepth;
        }

        private Dictionary<Tuple<Node, int>, List<Node>> Cache = new Dictionary<Tuple<Node, int>, List<Node>>();

        List<Node> GetNLevelTree(Node root, int depth)
        {
            Tuple<Node, int> cacheKey = new Tuple<Node, int>(root, depth);
            if (Cache.ContainsKey(cacheKey))
            {
                Console.WriteLine("{0}/{1} cache hit", root, depth);
                return Cache[cacheKey];
            }
            if (root is Node0 || root is Node1 || root is NodeId)
            {
                List<Node> ret = new List<Node>();
                ret.Add(root);
                Cache[cacheKey] = ret;
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
                    var newRoot = root.Clone();
                    (newRoot as NodeOp1).Node0 = subtree;
                    ret.Add(newRoot);
                }
                Cache[cacheKey] = ret;
                return ret;
            }
            else if (root is NodeOp2)
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
                for (var i = 0; i < subtrees.Count; ++i)
                {
                    for (var j = 0; j < subtrees.Count; ++j)
                    {
                        var newRoot = root.Clone();
                        (newRoot as NodeOp2).Node0 = subtrees[i];
                        (newRoot as NodeOp2).Node1 = subtrees[j];
                        ret.Add(newRoot);
                    }
                }
                Cache[cacheKey] = ret;
                return ret;
            }
            throw new Exception("unknown node");
        }

        //static Node[] AllNodes = { new Node0(), new Node1(), new NodeIf0(), 
        //                             new NodeOp1Not(), new NodeOp1Shl1(), 
        //                             new NodeOp1Shr1(), new NodeOp1Shr16(), 
        //                             new NodeOp1Shr4(), new NodeOp2And(), 
        //                             new NodeOp2Or(), new NodeOp2Plus(), new NodeOp2Xor()};
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
