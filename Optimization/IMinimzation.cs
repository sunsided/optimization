using System;
using JetBrains.Annotations;
using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization
{
    /// <summary>
    /// Interface IMinimization
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TCostFunction">The type of the cost function.</typeparam>
    public interface IMinimization<TData, in TCostFunction>
        where TData : struct, IEquatable<TData>, IFormattable, IComparable<TData>
        where TCostFunction : ICostFunction<TData>
    {
        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns>IOptimizationResult&lt;TData&gt;.</returns>
        [NotNull]
        IOptimizationResult<TData> Minimize([NotNull] IOptimizationProblem<TData, TCostFunction> problem);
    }
}
