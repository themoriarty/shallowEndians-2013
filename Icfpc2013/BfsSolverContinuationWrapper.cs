namespace Icfpc2013
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Icfpc2013.Ops;

    public class BfsSolverContinuationWrapper : SolverContinuationWrapper
    {
        #region Fields

        private readonly bool tfoldMode;

        private readonly List<Node> validFoldNodes;

        private readonly List<Node> validNodes;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private int programSize;

        #endregion

        #region Constructors and Destructors

        public BfsSolverContinuationWrapper(int judgesProgramSize, OpTypes validOps, ulong[] inputs, ulong[] outputs, Func<ulong[], ulong[], Node, bool> filter, Func<Node, Tuple<bool,bool, ulong, ulong>> checker)
            : base(judgesProgramSize, validOps, inputs, outputs, filter, checker)
        {
            programSize = judgesProgramSize - 1;

            tfoldMode = false;
            if ((validOps & OpTypes.tfold) != OpTypes.none)
            {
                tfoldMode = true;
                programSize -= 4;
            }

            ProgramTree.GetAvailableNodes(validOps, tfoldMode, out validNodes, out validFoldNodes);
        }

        #endregion

        #region Methods

        protected override Tuple<bool,bool, ulong, ulong> CheckerImpl(Node node)
        {
            var filterSolution = node;

            if (tfoldMode)
            {
                filterSolution = new NodeFold { Node0 = new NodeId { Name = "x" }, Node1 = new Node0(), Node2 = new Lambda2 { Id0 = new NodeId { Name = "x1" }, Id1 = new NodeId { Name = "x2" }, Node0 = node } };
            }

            return Checker(filterSolution);
        }

        protected override bool FilterImpl(ulong[] testInputs, ulong[] testOutputs, Node node)
        {
            if (node.Size() != programSize)
            {
                return false;
            }

            var filterSolution = node;

            if (tfoldMode)
            {
                filterSolution = new NodeFold { Node0 = new NodeId { Name = "x" }, Node1 = new Node0(), Node2 = new Lambda2 { Id0 = new NodeId { Name = "x1" }, Id1 = new NodeId { Name = "x2" }, Node0 = node } };
            }

            bool filterValid = Filter(testInputs, testOutputs, filterSolution);

            if (filterValid)
            {
                var currentOps = OpTypes.none;
                filterSolution.Op(ref currentOps);

                if (tfoldMode)
                {
                    currentOps &= ~OpTypes.fold;
                    currentOps |= OpTypes.tfold;
                }

                if (currentOps == ValidOps)
                {
                    return true;
                }
            }

            return false;
        }
        
        protected override IEnumerable<Node> RunImpl()
        {
#if true
#if true
            var builder = new PTreeGeneratorContainer(validNodes, validFoldNodes, programSize, (node) => this.FilterImpl(Inputs, Outputs, node));
            foreach (var root in builder.GenerateAllPrograms(cts.Token))
            {
                yield return root;
            }
#else
            var builder = new FTreeGenerator(validNodes, validFoldNodes, programSize);
            foreach (var root in builder.GenerateAllPrograms())
            {
                yield return root;
            }
#endif
#else

            int bfsSize = targetSize > 5 ? 5 : targetSize;
            var builder = new TreeGenerator(validNodes, bfsSize);
            foreach (var root in builder.GenerateAllPrograms().Where(x => x.Size() >= bfsSize - 1))
            {                
                foreach (var p in GenerateProgramsOfCorrectSize(targetSize, targetSize, root, validNodes))
                {
                    //Console.WriteLine("{0} -> {1}", root.Serialize(), p.Serialize());
                    yield return p;
                }
                //yield break;
            }
#endif
        }

        protected override void StopImpl()
        {
            cts.Cancel();
        }

        #endregion
    }
}