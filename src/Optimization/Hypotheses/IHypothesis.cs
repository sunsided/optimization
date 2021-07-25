using System;
using MathNet.Numerics.LinearAlgebra;

namespace WideMeadows.Optimization.Hypotheses
{
    /// <summary>
    /// Interface IHypothesis
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public interface IHypothesis<TData>
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="inputs"/> and the <paramref name="coefficients"/>.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="inputs">The inputs.</param>
        /// <returns>The estimated outputs.</returns>
        Vector<TData> Evaluate(Vector<TData> coefficients, Vector<TData> inputs);
    }
}
