using System;

namespace widemeadows.Optimization.Cost
{
    /// <summary>
    /// Interface ICost
    /// </summary>
    /// <typeparam name="TData">The type of the t data.</typeparam>
    public interface ICost<out TData>
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Gets the cost.
        /// </summary>
        /// <value>The cost.</value>
        TData Cost { get; }
    }
}
