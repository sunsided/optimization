using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization.Cost
{
    /// <summary>
    /// Interface <see cref="ICostFunction{TData,TCost}"/>
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TCost">The type of the cost result.</typeparam>
    public interface ICostFunction<TData, out TCost>
        where TData : struct, IEquatable<TData>, IFormattable
        where TCost : ICost<TData>
    {
        /// <summary>
        /// Calculates the cost.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <returns>TCost.</returns>
        [NotNull, Obsolete]
        TCost CalculateCostAndGradient([NotNull] Vector<TData> coefficients);

        /// <summary>
        /// Calculates the cost.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <returns>TCost.</returns>
        [NotNull, Obsolete]
        double CalculateCost([NotNull] Vector<TData> coefficients);

        /// <summary>
        /// Calculates the first derivative, i.e. the gradient, at the given <paramref name="locations"/>
        /// </summary>
        /// <param name="locations">The locations at which to evaluate the gradient.</param>
        /// <returns>The gradient.</returns>
        [NotNull]
        Vector<TData> CalculateGradient([NotNull] Vector<TData> locations);

        /// <summary>
        /// Calculates the second derivative, i.e. the gradient's gradient, at the given <paramref name="locations"/>
        /// </summary>
        /// <param name="locations">The locations at which to evaluate the second derivative.</param>
        /// <returns>The second derivative.</returns>
        [NotNull]
        Vector<TData> CalculateLaplacian([NotNull] Vector<TData> locations);
    }
}
