namespace Icfpc2013
{
    using System;
    using System.Collections.Generic;

    using Icfpc2013.Ops;

    internal class FTreeGenerator
    {
        #region Fields

        private readonly int MaxDepth;

        // TODO: Change order valid operations
        private readonly List<Node> ValidNodes;

        #endregion

        #region Constructors and Destructors

        public FTreeGenerator(List<Node> validNodes, int maxDepth)
        {
            Console.WriteLine("Using FTreeGenerator!");
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
            foreach (var node in ValidNodes)
            {
                foreach (var subtree in GetNLevelTree(node, MaxDepth))
                {
                    yield return subtree;
                }
            }
        }

        #endregion

        #region Methods

        public IEnumerable<Node> GetNLevelTree(Node root, int depth)
        {
            // TODO: remove
            /*
            if (depth > MaxDepth)
            {
                Console.WriteLine("MUMU!");
                yield break;
            }
            */

            if (root is Node0 || root is Node1 || root is NodeId)
            {
                //Console.WriteLine(depth + " " + root.Size());
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
                    foreach (var node in ValidNodes)
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
                    foreach (var node1 in ValidNodes)
                    {
                        foreach (var subtree1 in GetNLevelTree(node1, depth - 2))
                        {
                            foreach (var node2 in ValidNodes)
                            {
                                foreach (var subtree2 in GetNLevelTree(node2, depth - 2))
                                {
                                    if (subtree1.Size() + subtree2.Size() <= depth - 1)
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
                }
                yield break;
            }
            else if (root is NodeOp3)
            {
                if (depth > 3)
                {
                    foreach (var node1 in ValidNodes)
                    {
                        foreach (var subtree1 in GetNLevelTree(node1, depth - 3))
                        {
                            foreach (var node2 in ValidNodes)
                            {
                                foreach (var subtree2 in GetNLevelTree(node2, depth - 3))
                                {
                                    foreach (var node3 in ValidNodes)
                                    {
                                        foreach (var subtree3 in GetNLevelTree(node3, depth - 3))
                                        {
                                            if (subtree1.Size() + subtree2.Size() + subtree3.Size() <= depth - 1)
                                            {
                                                var newRoot = root.Clone();
                                                (newRoot as NodeOp3).Node0 = subtree1;
                                                (newRoot as NodeOp3).Node1 = subtree2;
                                                (newRoot as NodeOp3).Node2 = subtree3;
                                                yield return newRoot;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                yield break;
            }

            //Console.WriteLine("Unknown node!");
            throw new Exception("unknown node");
        }

        #endregion
    }
}

