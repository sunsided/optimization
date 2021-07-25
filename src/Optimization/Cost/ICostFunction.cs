using System;
using MathNet.Numerics.LinearAlgebra;

namespace WideMeadows.Optimization.Cost
{
    /// <summary>
    /// Interface <see cref="ICostFunction{TData}"/>
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public interface ICostFunction<TData>
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Calculates the cost.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <returns>TCost.</returns>
        TData CalculateCost(Vector<TData> coefficients);
    }
}
