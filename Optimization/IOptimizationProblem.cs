using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization
{
    /// <summary>
    /// Interface IOptimizationProblem
    /// </summary>
    public interface IOptimizationProblem<TData, out TCostFunction, TCost>
        where TData : struct, IEquatable<TData>, IFormattable
        where TCostFunction : ICostFunction<TData, TCost> 
        where TCost : ICost<TData>
    {
        /// <summary>
        /// Gets the cost function.
        /// </summary>
        /// <value>The cost function.</value>
        [NotNull]
        TCostFunction CostFunction { get; }
    }
}
