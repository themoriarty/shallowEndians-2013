﻿using Icfpc2013.Ops;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icfpc2013
{
    public class PTreeGeneratorContainer
    {
        #region Fields

        private readonly int MaxDepth;

        // TODO: Change order valid operations
        private readonly List<Node> ValidNodes;

        #endregion

        #region Constructors and Destructors

        public PTreeGeneratorContainer(List<Node> validNodes, int maxDepth)
        {
            Console.WriteLine("Using PTreeGeneratorContainer - parallel");
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
            var tasks = new Task[ValidNodes.Count];

            for (int i=0; i< ValidNodes.Count; i++)
            {
                var node = ValidNodes[i];
                tasks[i] = Task.Factory.StartNew(() => GetPartialResult(ValidNodes, node, MaxDepth));
            }

            do
            {
                Node candidate;
                if (candidates.TryDequeue(out candidate))
                {
                    yield return candidate;
                }

                int completed = 0;
                foreach (var task in tasks)
                {
                    if (task.IsCompleted)
                    {
                        completed++;
                    }
                }

                if (completed == tasks.Length)
                {
                    while(candidates.TryDequeue(out candidate))
                    {
                        yield return candidate;
                    }

                    yield break;
                }
            } while (true == true);
        }

        private ConcurrentQueue<Node> candidates = new ConcurrentQueue<Node>();

        private void GetPartialResult(List<Node> validNodes, Node node, int maxDepth)
        {
            var ret = new List<Node>();

            var builder = new FTreeGenerator(validNodes, maxDepth);

            foreach (var subtree in builder.GetNLevelTree(node, MaxDepth))
            {
                lock (candidates)
                {
                    candidates.Enqueue(subtree);
                }
            }
        }

        /*
        // Define a delegate that prints and returns the system tick count
        Func<object, List<Node>> action = (object nodeRaw) =>
        {
            var ret = new List<Node>();

            var node = (Node)nodeRaw;

            var generator = new FTreeGenerator(validNodes, maxDepth-1);

            foreach (var subtree in generator.GenerateAllPrograms())
            {
                ret.Add(subtree);
            }

            return ret;
        };
        */

        #endregion
    }
}
