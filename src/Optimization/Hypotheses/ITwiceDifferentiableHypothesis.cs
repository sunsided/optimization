using System;
using MathNet.Numerics.LinearAlgebra;

namespace WideMeadows.Optimization.Hypotheses
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
        [Obsolete("Shouldn't be required")]
        Vector<TData> Hessian(Vector<double> coefficients, Vector<TData> locations, Vector<TData> outputs);

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="locations" /> given the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="locations"/>.</returns>
        [Obsolete("Shouldn't be required")]
        Vector<TData> Hessian(Vector<double> coefficients, Vector<double> locations);

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="coefficients" /> given the <paramref name="locations" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate"/>.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="coefficients"/>.</returns>
        [Obsolete("Shouldn't be required")]
        Vector<TData> CoefficientHessian(Vector<double> coefficients, Vector<TData> locations, Vector<TData> outputs);

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="coefficients" /> given the <paramref name="locations" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="coefficients"/>.</returns>
        [Obsolete("Shouldn't be required")]
        Vector<TData> CoefficientHessian(Vector<double> coefficients, Vector<double> locations);
    }
}
