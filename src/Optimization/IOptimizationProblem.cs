using System;
using WideMeadows.Optimization.Cost;

namespace WideMeadows.Optimization
{
    /// <summary>
    /// Interface IOptimizationProblem
    /// </summary>
    public interface IOptimizationProblem<TData, out TCostFunction> : IInitialCoefficients<TData>
        where TData : struct, IEquatable<TData>, IFormattable
        where TCostFunction : ICostFunction<TData>
    {
        /// <summary>
        /// Gets the cost function.
        /// </summary>
        /// <value>The cost function.</value>
        TCostFunction CostFunction { get; }
    }
}
