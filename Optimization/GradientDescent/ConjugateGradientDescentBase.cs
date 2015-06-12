using System;
using widemeadows.Optimization.Cost;

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
        /// The maximum number of iterations
        /// </summary>
        private int _maxLineSearchIterations = 40;

        /// <summary>
        /// The maximum number of iterations
        /// </summary>
        private double _lineSearchStepSize = 1E-5D;

        /// <summary>
        /// The error tolerance
        /// </summary>
        private double _errorToleranceSquared;

        /// <summary>
        /// Gets the squared error tolerance.
        /// </summary>
        /// <value>The error tolerance squared.</value>
        protected double ErrorToleranceSquared
        {
            get { return _errorToleranceSquared; }
        }

        /// <summary>
        /// Gets or sets the maximum number of iterations for the line search.
        /// </summary>
        /// <value>The maximum iterations.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">The value must be positive</exception>
        public int MaxLineSearchIterations
        {
            get { return _maxLineSearchIterations; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("value", value, "The value must be positive");
                _maxLineSearchIterations = value;
            }
        }

        /// <summary>
        /// Gets or sets the initial line search step size.
        /// </summary>
        /// <value>The cost change threshold.</value>
        /// <exception cref="System.NotFiniteNumberException">The value must be finite</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The value must be nonnegative</exception>
        public double LineSearchStepSize
        {
            get { return _lineSearchStepSize; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value)) throw new NotFiniteNumberException("The value must be finite", value);
                if (value < 0) throw new ArgumentOutOfRangeException("value", value, "The value must be nonnegative");
                _lineSearchStepSize = value;
            }
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
                if (value <= 0 || value > 1) throw new ArgumentOutOfRangeException("value", value, "The value must positive and less than or equal to 1");
                base.ErrorTolerance = value;
                _errorToleranceSquared = value*value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConjugateGradientDescentBase{TData,TCostFunction}"/> class.
        /// </summary>
        protected ConjugateGradientDescentBase()
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            // ReSharper disable once ExceptionNotDocumented
            ErrorTolerance = 1E-10D;
        }
    }
}