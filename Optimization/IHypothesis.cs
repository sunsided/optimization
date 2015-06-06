using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization
{
    /// <summary>
    /// Interface IHypothesis
    /// </summary>
    /// <typeparam name="TData">The type of the t data.</typeparam>
    public interface IHypothesis<TData> 
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Gets an initial guess for the coefficients.
        /// </summary>
        /// <returns>Vector&lt;TData&gt;.</returns>
        [NotNull]
        Vector<TData> GetInitialCoefficients();

        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="inputs"/> and the <paramref name="coefficients"/>.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <param name="coefficients">The coefficients.</param>
        /// <returns>Vector&lt;TData&gt;.</returns>
        [NotNull] 
        Vector<TData> Evaluate([NotNull] Vector<TData> inputs, [NotNull] Vector<TData> coefficients);
    }
}
