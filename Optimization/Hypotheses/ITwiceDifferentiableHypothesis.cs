using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization.Hypotheses
{
    /// <summary>
    /// Interface IDifferentiableHypothesis
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public interface ITwiceDifferentiableHypothesis<TData> : IDifferentiableHypothesis<TData>
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="locations" /> given the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate"/>.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="locations"/>.</returns>
        [NotNull]
        Vector<TData> Laplacian([NotNull] Vector<double> coefficients, [NotNull] Vector<TData> locations, [NotNull] Vector<TData> outputs);

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="locations" /> given the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="locations"/>.</returns>
        [NotNull]
        Vector<TData> Laplacian([NotNull] Vector<double> coefficients, [NotNull] Vector<double> locations);

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="coefficients" /> given the <paramref name="locations" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate"/>.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="coefficients"/>.</returns>
        [NotNull]
        Vector<TData> CoefficientLaplacian([NotNull] Vector<double> coefficients, [NotNull] Vector<TData> locations, [NotNull] Vector<TData> outputs);

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="coefficients" /> given the <paramref name="locations" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="coefficients"/>.</returns>
        [NotNull]
        Vector<TData> CoefficientLaplacian([NotNull] Vector<double> coefficients, [NotNull] Vector<double> locations);
    }
}
