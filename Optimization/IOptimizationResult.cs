using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization
{
    /// <summary>
    /// Results of the optimization task.
    /// </summary>
    public interface IOptimizationResult<TData>
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Gets the final cost.
        /// </summary>
        /// <value>The cost.</value>
        double Cost { get; }

        /// <summary>
        /// Gets the coefficients.
        /// </summary>
        /// <value>The coefficients.</value>
        [NotNull]
        Vector<TData> Coefficients { get; }
    }
}
