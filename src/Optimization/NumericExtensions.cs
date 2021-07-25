using System;
using System.Diagnostics.Contracts;

namespace WideMeadows.Optimization
{
    /// <summary>
    /// Class NumericExtensions.
    /// </summary>
    static class NumericExtensions
    {
        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is neither <see cref="double.NaN"/>, <see cref="double.NegativeInfinity"/> nor <see cref="double.PositiveInfinity"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><see langword="true" /> if the specified value is finite; otherwise, <see langword="false" />.</returns>
        [Pure]
        public static bool IsFinite(this double value) => !Double.IsNaN(value) && !Double.IsInfinity(value);
    }
}
