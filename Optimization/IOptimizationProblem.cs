using System;
using JetBrains.Annotations;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.Hypotheses;

namespace widemeadows.Optimization
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
        [NotNull]
        TCostFunction CostFunction { get; }
    }
}
