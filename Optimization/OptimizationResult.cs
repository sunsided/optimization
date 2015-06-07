using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization
{
    /// <summary>
    /// Results of the optimization task.
    /// </summary>
    public class OptimizationResult<TData> : IOptimizationResult<TData>
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Gets the final cost.
        /// </summary>
        /// <value>The cost.</value>
        public double Cost { get; private set; }

        /// <summary>
        /// Gets the coefficients.
        /// </summary>
        /// <value>The coefficients.</value>
        public Vector<TData> Coefficients { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimizationResult{TData}"/> class.
        /// </summary>
        /// <param name="cost"></param>
        /// <param name="coefficients">The coefficients.</param>
        public OptimizationResult(double cost, [NotNull] Vector<TData> coefficients)
        {
            Cost = cost;
            Coefficients = coefficients;
        }
    }
}
