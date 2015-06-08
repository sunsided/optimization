using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization.Cost
{
    /// <summary>
    /// Class DifferentiableCostResult.
    /// </summary>
    public sealed class DifferentiableCostResult<TData>  : CostResult<TData>, ICostGradient<TData>
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Gets the cost gradient.
        /// </summary>
        /// <value>The cost gradient.</value>
        public Vector<TData> CostGradient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DifferentiableCostResult{TData}"/> class.
        /// </summary>
        /// <param name="cost">The cost.</param>
        /// <param name="costGradient">The cost gradient.</param>
        public DifferentiableCostResult(TData cost, [NotNull] Vector<TData> costGradient)
            : base(cost)
        {
            CostGradient = costGradient;
        }
    }
}
