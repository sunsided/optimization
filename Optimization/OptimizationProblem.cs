using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.Hypotheses;

namespace widemeadows.Optimization
{
    /// <summary>
    /// Class OptimizationProblem.
    /// </summary>
    public class OptimizationProblem<TData, TCostFunction> : IOptimizationProblem<TData, TCostFunction>
        where TData : struct, IEquatable<TData>, IFormattable
        where TCostFunction : ICostFunction<TData>
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
        public TCostFunction CostFunction { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimizationProblem{TData, TCost}" /> class.
        /// </summary>
        /// <param name="hypothesis">The hypothesis.</param>
        /// <param name="costFunction">The cost function.</param>
        public OptimizationProblem([NotNull] IHypothesis<TData> hypothesis, [NotNull] TCostFunction costFunction)
        {
            Hypothesis = hypothesis;
            CostFunction = costFunction;
        }

        /// <summary>
        /// Gets the initial coefficients.
        /// </summary>
        /// <returns>Vector&lt;TData&gt;.</returns>
        public Vector<TData> GetInitialCoefficients()
        {
            return Hypothesis.GetInitialCoefficients();
        }
    }
}
