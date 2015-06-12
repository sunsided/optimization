using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization.LineSearch
{
    /// <summary>
    /// Line search-based directed function minimization.
    /// </summary>
    /// <typeparam name="TData">The type of the t data.</typeparam>
    /// <typeparam name="TFunction">The type of the t cost function.</typeparam>
    public interface ILineSearch<TData, in TFunction>
        where TData : struct, IEquatable<TData>, IFormattable, IComparable<TData>
        where TFunction : ICostFunction<TData>
    {
        /// <summary>
        /// Minimizes the <paramref name="function"/> by performing a line search along the <paramref name="direction"/>, starting from the given <paramref name="location"/>.
        /// </summary>
        /// <param name="function">The cost function.</param>
        /// <param name="location">The starting point.</param>
        /// <param name="direction">The search direction.</param>
        /// <returns>The best found minimum point along the <paramref name="direction"/>.</returns>
        [NotNull]
        Vector<TData> Minimize([NotNull] TFunction function, [NotNull] Vector<TData> location, [NotNull] Vector<TData> direction);
    }
}
