namespace Icfpc2013
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Icfpc2013.Ops;

    public class SolverContinuationWrapper
    {
        #region Constructors and Destructors

        public SolverContinuationWrapper(int judgesProgramSize, OpTypes validOps, ulong[] inputs, ulong[] outputs, Func<ulong[], ulong[], Node, bool> filter, Func<Node, Tuple<bool,bool, ulong, ulong>> checker)
        {
            this.JudgesProgramSize = judgesProgramSize;
            this.ValidOps = validOps;
            this.Inputs = inputs;
            this.Outputs = outputs;
            this.Filter = filter;
            this.Checker = checker;
        }

        #endregion

        #region Public Properties

        public ulong[] Inputs { get; set; }

        public int JudgesProgramSize { get; set; }

        public ulong[] Outputs { get; set; }

        public OpTypes ValidOps { get; set; }

        public Func<ulong[], ulong[], Node, bool> Filter { get; set; }

        public Func<Node, Tuple<bool,bool, ulong, ulong>> Checker { get; set; }

        #endregion

        #region Public Methods and Operators

        public void AddCounterExample(ulong newInput, ulong newOutput)
        {
            var newInputs = new List<ulong>(Inputs);
            newInputs.Add(newInput);
            Inputs = newInputs.ToArray();

            var newOutputs = new List<ulong>(Outputs);
            newOutputs.Add(newOutput);
            Outputs = newOutputs.ToArray();

            Console.WriteLine("Input: {{{0}}}", string.Join(", ", Inputs.Select(s => string.Format("0x{0:X16}", s)).ToArray()));
            Console.WriteLine("Output: {{{0}}}", string.Join(", ", Outputs.Select(s => string.Format("0x{0:X16}", s)).ToArray()));

            AddCounterExampleImpl(newInput, newOutput);
        }

        protected virtual bool FilterImpl(ulong[] testInputs, ulong[] testOutputs, Node node)
        {
            return Filter(testInputs, testOutputs, node);
        }

        protected virtual Tuple<bool,bool, ulong, ulong> CheckerImpl(Node node)
        {
            return Checker(node);
        }

        public Lambda1 Run()
        {
            try
            {

                foreach (var candidate in RunImpl())
                {
                    if (FilterImpl(Inputs, Outputs, candidate))
                    {
                        var result = CheckerImpl(candidate);

                        if (result.Item1)
                        {
                            StopImpl();

                            return new Lambda1 { Id0 = new NodeId { Name = "x" }, Node0 = candidate };
                        }
                        else if (!result.Item2)
                        {
                            break;
                        }
                        else
                        {
                            AddCounterExample(result.Item3, result.Item4);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex);
            }
            
            StopImpl();

            return null;
        }

        public void Stop()
        {
            this.StopImpl();
        }

        #endregion

        #region Methods

        protected virtual void AddCounterExampleImpl(ulong newInput, ulong newOutput)
        {
        }

        protected virtual IEnumerable<Node> RunImpl()
        {
            yield break;
        }

        protected virtual void StopImpl()
        {
        }

        #endregion
    }
}