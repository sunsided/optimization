using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;

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
        /// The initial coefficients.
        /// </summary>
        [NotNull]
        private readonly Vector<TData> _initialCoefficients;

        /// <summary>
        /// Gets the cost function.
        /// </summary>
        /// <value>The cost function.</value>
        [NotNull]
        public TCostFunction CostFunction { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimizationProblem{TData, TCost}" /> class.
        /// </summary>
        /// <param name="costFunction">The cost function.</param>
        /// <param name="initialCoefficients"></param>
        public OptimizationProblem([NotNull] TCostFunction costFunction, [NotNull] Vector<TData> initialCoefficients)
        {
            CostFunction = costFunction;
            _initialCoefficients = initialCoefficients;
        }

        /// <summary>
        /// Gets the initial coefficients.
        /// </summary>
        /// <returns>Vector&lt;TData&gt;.</returns>
        public Vector<TData> GetInitialCoefficients()
        {
            return _initialCoefficients;
        }
    }
}
