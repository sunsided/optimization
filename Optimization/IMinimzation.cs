using System;
using JetBrains.Annotations;

namespace widemeadows.Optimization
{
    /// <summary>
    /// Interface IMinimization
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public interface IMinimization<TData>
        where TData : struct, IEquatable<TData>, IFormattable, IComparable<TData>
    {
        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns>IOptimizationResult&lt;TData&gt;.</returns>
        [NotNull]
        IOptimizationResult<TData> Minimize([NotNull] IOptimizationProblem<TData> problem);
    }
}
