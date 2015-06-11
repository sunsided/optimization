using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization.Cost
{
    /// <summary>
    /// Interface <see cref="IDifferentiableCostFunction{TData}" />
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TCost">The type of the cost function result.</typeparam>
    public interface IDifferentiableCostFunction<TData> : ICostFunction<TData>
        where TData : struct, IFormattable, IEquatable<TData>
    {

        /// <summary>
        /// Calculates the first derivative, i.e. the gradient, at the given <paramref name="locations"/>
        /// </summary>
        /// <param name="locations">The locations at which to evaluate the gradient.</param>
        /// <returns>The gradient.</returns>
        [NotNull]
        Vector<TData> CalculateGradient([NotNull] Vector<TData> locations);
    }
}
