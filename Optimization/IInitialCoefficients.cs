using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization
{
    /// <summary>
    /// Interface IInitialCoefficients
    /// </summary>
    /// <typeparam name="TData">The type of the t data.</typeparam>
    public interface IInitialCoefficients<TData> 
        where TData : struct, IFormattable, IEquatable<TData>
    {
        /// <summary>
        /// Gets an initial guess for the coefficients.
        /// </summary>
        /// <returns>Vector&lt;TData&gt;.</returns>
        [NotNull]
        Vector<TData> GetInitialCoefficients();
    }
}