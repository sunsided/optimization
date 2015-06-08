using System;

namespace widemeadows.Optimization.Cost
{
    /// <summary>
    /// Class CostResult.
    /// </summary>
    /// <typeparam name="TData">The type of the t data.</typeparam>
    public class CostResult<TData> : ICost<TData> 
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Gets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public TData Cost { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CostResult"/> class.
        /// </summary>
        /// <param name="cost">The cost.</param>
        public CostResult(TData cost)
        {
            Cost = cost;
        }
    }
}