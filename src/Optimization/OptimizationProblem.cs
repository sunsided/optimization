using System;
using MathNet.Numerics.LinearAlgebra;
using WideMeadows.Optimization.Cost;

namespace WideMeadows.Optimization
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
        private readonly Vector<TData> _initialCoefficients;

        /// <summary>
        /// Gets the cost function.
        /// </summary>
        /// <value>The cost function.</value>
        public TCostFunction CostFunction { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimizationProblem{TData, TCost}" /> class.
        /// </summary>
        /// <param name="costFunction">The cost function.</param>
        /// <param name="initialCoefficients"></param>
        public OptimizationProblem(TCostFunction costFunction, Vector<TData> initialCoefficients)
        {
            CostFunction = costFunction;
            _initialCoefficients = initialCoefficients;
        }

        /// <summary>
        /// Gets the initial coefficients.
        /// </summary>
        /// <returns>Vector&lt;TData&gt;.</returns>
        public Vector<TData> GetInitialCoefficients() => _initialCoefficients;
    }
}
