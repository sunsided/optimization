using System;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using WideMeadows.Optimization.Hypotheses;

namespace WideMeadows.Optimization.Cost
{
    /// <summary>
    /// This wrapper allows using a single-output <see cref="IHypothesis{TData}"/> function as a cost function.
    /// <para>
    /// It assumes that the function has a fixed set of coefficients and that the free variable
    /// to be optimized is the input vector.
    /// </para>
    /// </summary>
    /// <typeparam name="TData">The type of the t data.</typeparam>
    public sealed class FunctionValueOptimization<TData> : IDifferentiableCostFunction<TData>
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// The coefficients
        /// </summary>
        private readonly Vector<TData> _coefficients;

        /// <summary>
        /// The hypothesis
        /// </summary>
        private readonly IDifferentiableHypothesis<TData> _hypothesis;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionValueOptimization{TData}" /> class.
        /// </summary>
        /// <param name="hypothesis">The hypothesis.</param>
        /// <param name="coefficients">The fixed coefficients of the function.</param>
        public FunctionValueOptimization(IDifferentiableHypothesis<TData> hypothesis, Vector<TData> coefficients)
        {
            _coefficients = coefficients;
            _hypothesis = hypothesis;
        }

        /// <summary>
        /// Calculates the cost.
        /// </summary>
        /// <param name="locations">The locations to optimize.</param>
        /// <returns>TCost.</returns>
        public TData CalculateCost(Vector<TData> locations)
        {
            var result = _hypothesis.Evaluate(_coefficients, locations);
            Debug.Assert(result.Count == 1, "result.Count == 1");

            return result[0];
        }

        /// <summary>
        /// Calculates the first derivative, i.e. the gradient, at the given <paramref name="locations" />
        /// </summary>
        /// <param name="locations">The locations at which to evaluate the gradient.</param>
        /// <returns>The gradient.</returns>
        public Vector<TData> Jacobian(Vector<TData> locations) =>
            // TODO: Fallback using finite differences
            _hypothesis.Jacobian(_coefficients, locations);
    }
}
