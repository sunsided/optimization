﻿using System;
using WideMeadows.Optimization.Cost;

namespace WideMeadows.Optimization.GradientDescent.Regular
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
        private double _errorTolerance = 1E-10D;

        /// <summary>
        /// Gets or sets the maximum number of iterations.
        /// </summary>
        /// <value>The maximum iterations.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">The value must be positive</exception>
        public int MaxIterations
        {
            get => _maxIterations;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), value, "The value must be positive");
                _maxIterations = value;
            }
        }

        /// <summary>
        /// Gets or sets the error tolerance. If, e.g. the cost change
        /// is less than the given threshold, optimization stops immediately.
        /// </summary>
        /// <value>The error tolerance.</value>
        /// <exception cref="System.NotFiniteNumberException">The value must be finite</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The value must be nonnegative</exception>
        public virtual double ErrorTolerance
        {
            get => _errorTolerance;
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value)) throw new NotFiniteNumberException("The value must be finite", value);
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), value, "The value must be nonnegative");
                _errorTolerance = value;
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