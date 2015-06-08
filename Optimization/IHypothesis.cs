using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization
{
    /// <summary>
    /// Interface IHypothesis
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public interface IHypothesis<TData> : IInitialCoefficients<TData> 
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="inputs"/> and the <paramref name="coefficients"/>.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <param name="coefficients">The coefficients.</param>
        /// <returns>The estimated outputs.</returns>
        [NotNull] 
        Vector<TData> Evaluate([NotNull] Vector<TData> inputs, [NotNull] Vector<TData> coefficients);
    }
}
