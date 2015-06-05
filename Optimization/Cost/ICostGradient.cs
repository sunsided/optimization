using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization.Cost
{
    /// <summary>
    /// Interface ICostGradient
    /// </summary>
    /// <typeparam name="TData">The type of the t data.</typeparam>
    public interface ICostGradient<TData> : ICost<TData> 
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Gets the cost function gradient.
        /// </summary>
        /// <value>The cost function gradient.</value>
        [NotNull]
        Vector<TData> CostGradient { get; }
    }
}
