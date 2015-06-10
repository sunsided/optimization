using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization.Cost
{
    /// <summary>
    /// Interface ICostGradient2
    /// </summary>
    /// <typeparam name="TData">The type of the t data.</typeparam>
    public interface ICostGradient2<TData> : ICostGradient<TData>
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Gets the cost function's second derivative, i.e. gradient-of-gradient.
        /// </summary>
        /// <value>The cost function gradient.</value>
        [NotNull]
        Vector<TData> CostGradient2 { get; }
    }
}
