﻿using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization.Cost
{
    /// <summary>
    /// Interface <see cref="ITwiceDifferentiableCostFunction{TData}" />
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public interface ITwiceDifferentiableCostFunction<TData> : IDifferentiableCostFunction<TData>
        where TData : struct, IFormattable, IEquatable<TData>
    {
        /// <summary>
        /// Calculates the second derivative, i.e. the gradient's gradient, at the given <paramref name="locations"/>
        /// </summary>
        /// <param name="locations">The locations at which to evaluate the second derivative.</param>
        /// <returns>The second derivative.</returns>
        [NotNull]
        Vector<TData> CalculateLaplacian([NotNull] Vector<TData> locations);
    }
}
