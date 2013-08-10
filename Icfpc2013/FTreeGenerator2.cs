namespace Icfpc2013
{
    using System;
    using System.Collections.Generic;

    using Icfpc2013.Ops;
    using System.Security.Cryptography;

    internal class FTreeGenerator2
    {
        #region Fields

        private readonly int MaxDepth;

        // TODO: Change order valid operations
        private readonly List<Node> ValidNodes;

        #endregion

        #region Constructors and Destructors

        public FTreeGenerator2(List<Node> validNodes, int maxDepth)
        {
            Console.WriteLine("Using FTreeGenerator2! - with random");
            ValidNodes = validNodes;
            MaxDepth = maxDepth;
        }

        #endregion

        // static Node[] AllNodes = { new Node0(), new Node1(), new NodeIf0(), 
        // new NodeOp1Not(), new NodeOp1Shl1(), 
        // new NodeOp1Shr1(), new NodeOp1Shr16(), 
        // new NodeOp1Shr4(), new NodeOp2And(), 
        // new NodeOp2Or(), new NodeOp2Plus(), new NodeOp2Xor()};
        #region Public Methods and Operators

        public IEnumerable<Node> GenerateAllPrograms()
        {
            foreach (var node in GetRandomListCopy(ValidNodes))
            {
                foreach (var subtree in GetNLevelTree(node, MaxDepth))
                {
                    yield return subtree;
                }
            }
        }

        #endregion

        #region Methods

        public static void Shuffle<T>(IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public IList<Node> GetRandomListCopy(List<Node> validNodes)
        {
            var validNodesRand = new List<Node>(validNodes);
            Shuffle(validNodesRand);
            return validNodesRand;
        }

        private IEnumerable<Node> GetNLevelTree(Node root, int depth)
        {
            if (root is Node0 || root is Node1 || root is NodeId)
            {
                yield return root;
                yield break;
            }

            if (depth == 0)
            {
                yield break;
            }

            if (root is NodeOp1)
            {
                if (depth > 1)
                {
                    /*
                    var subtrees = new List<Node>();
                    foreach (var node in ValidNodes)
                    {
                        var subtree = GetNLevelTree(node, depth - 1);
                        if (subtree != null)
                        {
                            subtrees.AddRange(subtree);
                        }
                    }

                    foreach (var subtree in subtrees)
                    {
                        var newRoot = root.Clone();
                        (newRoot as NodeOp1).Node0 = subtree;
                        yield return newRoot;
                    }
                    */

                    foreach (var node in GetRandomListCopy(ValidNodes))
                    {
                        var subtrees = GetNLevelTree(node, depth - 1);
                        foreach (var subtree in subtrees)
                        {
                            var newRoot = root.Clone();
                            (newRoot as NodeOp1).Node0 = subtree;
                            yield return newRoot;
                        }
                    }
                }
                yield break;
            }
            else if (root is NodeOp2)
            {
                if (depth > 2)
                {
                    // This can be transformed into two foreach yield in different directions - this will decrease RAM usage and substantially increase CPU
                    /*
                    var subtrees = new List<Node>();
                    foreach (var node in ValidNodes)
                    {
                        var subtree = GetNLevelTree(node, depth - 1);
                        if (subtree != null)
                        {
                            subtrees.AddRange(subtree);
                        }
                    }

                    for (var i = 0; i < subtrees.Count; ++i)
                    {
                        for (var j = i; j < subtrees.Count; ++j)
                        {
                            var newRoot = root.Clone();
                            (newRoot as NodeOp2).Node0 = subtrees[i];
                            (newRoot as NodeOp2).Node1 = subtrees[j];
                            yield return newRoot;
                        }
                    }
                    */

                    var rand1 = GetRandomListCopy(ValidNodes);
                    var rand2 = GetRandomListCopy(ValidNodes);

                    foreach (var node1 in rand1)
                    {
                        foreach (var subtree1 in GetNLevelTree(node1, depth - 1))
                        {
                            foreach (var node2 in rand2)
                            {
                                foreach (var subtree2 in GetNLevelTree(node2, depth - 1))
                                {
                                    var newRoot = root.Clone();
                                    (newRoot as NodeOp2).Node0 = subtree1;
                                    (newRoot as NodeOp2).Node1 = subtree2;
                                    yield return newRoot;
                                }
                            }
                        }
                    }
                }
                yield break;
            }

            Console.WriteLine("Unknown node!");
            //throw new Exception("unknown node");
            yield break;
        }

        #endregion
    }
}

