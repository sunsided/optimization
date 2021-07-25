using System;
using MathNet.Numerics.LinearAlgebra;

namespace WideMeadows.Optimization
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
        Vector<TData> GetInitialCoefficients();
    }
}
