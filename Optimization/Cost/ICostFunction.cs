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
        [NotNull]
        TCost CalculateCost([NotNull] Vector<TData> coefficients);
    }
}
