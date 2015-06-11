using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization.Hypotheses
{
    /// <summary>
    /// Interface IDifferentiableHypothesis
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public interface IDifferentiableHypothesis<TData> : IHypothesis<TData>
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="locations" /> and the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate"/>.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients"/>.</returns>
        [NotNull]
        Vector<TData> Gradient([NotNull] Vector<double> coefficients, [NotNull] Vector<TData> locations, [NotNull] Vector<TData> outputs);

        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="locations" /> and the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients"/>.</returns>
        [NotNull]
        Vector<TData> Gradient([NotNull] Vector<double> coefficients, [NotNull] Vector<double> locations);
    }
}
