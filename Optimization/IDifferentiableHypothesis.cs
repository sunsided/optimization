using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization
{
    /// <summary>
    /// Interface IDifferentiableHypothesis
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public interface IDifferentiableHypothesis<TData> : IHypothesis<TData> 
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="inputs" /> and the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate"/>.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients"/>.</returns>
        [NotNull]
        Vector<TData> Derivative([NotNull] Vector<TData> inputs, [NotNull] Vector<double> coefficients, [NotNull] Vector<TData> outputs);
    }
}
