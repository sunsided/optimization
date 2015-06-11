using System;
using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization.GradientDescent
{
    /// <summary>
    /// Base class for Gradient Descent-type algorithms.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TCostFunction">The type of the cost function.</typeparam>
    public abstract class GradientDescentBase<TData, TCostFunction> : IGradientDescent, IMinimization<TData, TCostFunction>
        where TData : struct, IEquatable<TData>, IFormattable, IComparable<TData>
        where TCostFunction : ICostFunction<TData>
    {
        /// <summary>
        /// The maximum number of iterations
        /// </summary>
        private int _maxIterations = 400;

        /// <summary>
        /// The cost change threshold. If the cost change
        /// is less than the given threshold, iteration stops immediately.
        /// </summary>
        private double _costChangeThreshold = 1E-20D;

        /// <summary>
        /// Gets or sets the maximum number of iterations.
        /// </summary>
        /// <value>The maximum iterations.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">The value must be positive</exception>
        public int MaxIterations
        {
            get { return _maxIterations; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("value", value, "The value must be positive");
                _maxIterations = value;
            }
        }

        /// <summary>
        /// Gets or sets the cost change threshold. If the cost change
        /// is less than the given threshold, iteration stops immediately.
        /// </summary>
        /// <value>The cost change threshold.</value>
        /// <exception cref="System.NotFiniteNumberException">The value must be finite</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The value must be nonnegative</exception>
        public double CostChangeThreshold
        {
            get { return _costChangeThreshold; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value)) throw new NotFiniteNumberException("The value must be finite", value);
                if (value < 0) throw new ArgumentOutOfRangeException("value", value, "The value must be nonnegative");
                _costChangeThreshold = value;
            }
        }

        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns>IOptimizationResult&lt;TData&gt;.</returns>
        public abstract IOptimizationResult<TData> Minimize(IOptimizationProblem<TData, TCostFunction> problem);
    }
}