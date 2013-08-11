namespace Icfpc2013
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Icfpc2013.Ops;

    internal class FTreeGenerator
    {
        #region Fields

        private readonly int MaxDepth;

        // TODO: Change order valid operations
        private readonly List<Node> ValidNodes;
        private readonly List<Node> ValidNodesFold;
        private readonly List<Node> ValidNodesWithNoFold;

        #endregion

        #region Constructors and Destructors

        public FTreeGenerator(List<Node> validNodes, List<Node> validNodesFold, int maxDepth)
        {
            Console.WriteLine("Using FTreeGenerator!");
            ValidNodes = validNodes;
            ValidNodesFold = validNodesFold;
            MaxDepth = maxDepth;
            ValidNodesWithNoFold = ValidNodes.Where(s => !(s is NodeFold)).ToList();
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
                foreach (var subtree in GetNLevelTree(node, MaxDepth, ValidNodes))
                {
                    yield return subtree;
                }
            }
        }

        #endregion

        #region Methods

        public IEnumerable<Node> GetNLevelTree(Node root, int depth, List<Node> activeNodes = null)
        {
            if (activeNodes == null)
            {
                activeNodes = ValidNodes;
            }

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
                    foreach (var node in activeNodes)
                    {
                        var subtrees = GetNLevelTree(node, depth - 1, activeNodes);
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
                    foreach (var node1 in activeNodes)
                    {
                        foreach (var subtree1 in GetNLevelTree(node1, depth - 2, activeNodes))
                        {
                            var subtree1Size = (int)subtree1.Size();

                            foreach (var node2 in activeNodes)
                            {
                                foreach (var subtree2 in GetNLevelTree(node2, depth - 1 - subtree1Size, activeNodes))
                                {
                                    if (subtree1Size + subtree2.Size() <= depth - 1)
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
                    foreach (var node1 in activeNodes)
                    {
                        foreach (var subtree1 in GetNLevelTree(node1, depth - 3, activeNodes))
                        {
                            var subtree1Size = (int)subtree1.Size();
                            foreach (var node2 in activeNodes)
                            {
                                foreach (var subtree2 in GetNLevelTree(node2, depth - 2 - subtree1Size, activeNodes))
                                {
                                    var subtree2Size = (int)subtree2.Size();
                                    foreach (var node3 in activeNodes)
                                    {
                                        foreach (var subtree3 in GetNLevelTree(node3, depth - 1 - subtree1Size - subtree2Size, activeNodes))
                                        {
                                            if (subtree1Size + subtree2Size + subtree3.Size() <= depth - 1)
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
            else if (root is NodeFold)
            {
                if (depth > 3)
                {
                    foreach (var node1 in ValidNodesWithNoFold)
                    {
                        foreach (var subtree1 in GetNLevelTree(node1, depth - 3, ValidNodesWithNoFold))
                        {
                            var subtree1Size = (int)subtree1.Size();
                            foreach (var node2 in ValidNodesWithNoFold)
                            {
                                foreach (var subtree2 in GetNLevelTree(node2, depth - 2 - subtree1Size, ValidNodesWithNoFold))
                                {
                                    var subtree2Size = (int)subtree2.Size();
                                    foreach (var node3 in ValidNodesFold)
                                    {
                                        foreach (var subtree3 in GetNLevelTree(node3, depth - 1 - subtree1Size - subtree2Size, ValidNodesFold))
                                        {
                                            if (subtree1.Size() + subtree2.Size() + subtree3.Size() + 1 <= depth - 1)
                                            {
                                                var newRoot = root.Clone();
                                                (newRoot as NodeFold).Node0 = subtree1;
                                                (newRoot as NodeFold).Node1 = subtree2;

                                                var lambda = (newRoot as NodeFold).Node2;
                                                if (lambda == null)
                                                {
                                                    lambda = new Lambda2 { Id0 = new NodeId { Name = "x1" }, Id1 = new NodeId { Name = "x2" } };
                                                    (newRoot as NodeFold).Node2 = lambda;
                                                }
                                                lambda.Node0 = subtree3;
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

