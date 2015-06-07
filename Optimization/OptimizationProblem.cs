using System;
using JetBrains.Annotations;
using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization
{
    /// <summary>
    /// Class OptimizationProblem.
    /// </summary>
    public class OptimizationProblem<TData, TCost> : IOptimizationProblem<TData, TCost>
        where TData : struct, IEquatable<TData>, IFormattable
        where TCost : ICost<TData>
    {
        /// <summary>
        /// Gets the hypothesis.
        /// </summary>
        /// <value>The hypothesis.</value>
        public IHypothesis<TData> Hypothesis { get; private set; }

        /// <summary>
        /// Gets the cost function.
        /// </summary>
        /// <value>The cost function.</value>
        [NotNull]
        public ICostFunction<TData, TCost> CostFunction { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimizationProblem{TData, TCost}" /> class.
        /// </summary>
        /// <param name="hypothesis">The hypothesis.</param>
        /// <param name="costFunction">The cost function.</param>
        public OptimizationProblem([NotNull] IHypothesis<TData> hypothesis, [NotNull] ICostFunction<TData, TCost> costFunction)
        {
            Hypothesis = hypothesis;
            CostFunction = costFunction;
        }
    }
}
