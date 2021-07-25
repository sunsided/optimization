using System;
using MathNet.Numerics.LinearAlgebra;

namespace WideMeadows.Optimization
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
        Vector<TData> Coefficients { get; }
    }
}
