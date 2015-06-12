using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.LineSearch;

namespace widemeadows.Optimization.GradientDescent
{
    /// <summary>
    /// Class ConjugateGradientDescentBase.
    /// </summary>
    /// <typeparam name="TData">The type of the t data.</typeparam>
    /// <typeparam name="TCostFunction">The type of the t cost function.</typeparam>
    public abstract class ConjugateGradientDescentBase<TData, TCostFunction> : GradientDescentBase<TData, TCostFunction>
        where TData : struct, IEquatable<TData>, IFormattable, IComparable<TData>
        where TCostFunction : IDifferentiableCostFunction<TData>
    {
        /// <summary>
        /// The error tolerance
        /// </summary>
        private double _errorToleranceSquared;

        /// <summary>
        /// The line search algorithm
        /// </summary>
        [NotNull]
        private ILineSearch<TData, TCostFunction> _lineSearch;

        /// <summary>
        /// Gets the squared error tolerance.
        /// </summary>
        /// <value>The error tolerance squared.</value>
        protected double ErrorToleranceSquared
        {
            get { return _errorToleranceSquared; }
        }

        /// <summary>
        /// Gets or sets the error tolerance. If, e.g. the cost change
        /// is less than the given threshold, optimization stops immediately.
        /// </summary>
        /// <value>The error tolerance.</value>
        /// <exception cref="System.NotFiniteNumberException">The value must be finite</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The value must be nonnegative</exception>
        public override double ErrorTolerance
        {
            get { return base.ErrorTolerance; }
            set
            {
                if (value <= 0 || value > 1) throw new ArgumentOutOfRangeException("value", value, "The value must be positive and less than or equal to 1");
                base.ErrorTolerance = value;
                _errorToleranceSquared = value*value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConjugateGradientDescentBase{TData,TCostFunction}"/> class.
        /// </summary>
        /// <param name="lineSearch">The line search algorithm.</param>
        protected ConjugateGradientDescentBase([NotNull] ILineSearch<TData, TCostFunction> lineSearch)
        {
            _lineSearch = lineSearch;
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            // ReSharper disable once ExceptionNotDocumented
            ErrorTolerance = 1E-10D;
        }

        /// <summary>
        /// Performs a line search by using the secant method.
        /// </summary>
        /// <param name="costFunction">The cost function.</param>
        /// <param name="theta">The starting point.</param>
        /// <param name="direction">The search direction.</param>
        /// <returns>The best found minimum point along the <paramref name="direction"/>.</returns>
        [NotNull]
        protected Vector<TData> LineSearch([NotNull] TCostFunction costFunction, [NotNull] Vector<TData> theta, [NotNull] Vector<TData> direction)
        {
            return _lineSearch.LineSearch(costFunction, theta, direction);
        }
    }
}