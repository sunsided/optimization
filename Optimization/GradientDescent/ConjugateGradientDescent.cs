using System;
using System.Diagnostics;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization.GradientDescent
{
    /// <summary>
    /// Conjugate-Gradient Descent using Fletcher-Reeves conjugation and a Secant Method line search.
    /// </summary>
    /// <remarks>
    /// <code>
    /// @techreport{Shewchuk:1994:ICG:865018,
    ///  author = {Shewchuk, Jonathan R},
    ///  title = {An Introduction to the Conjugate Gradient Method Without the Agonizing Pain},
    ///  year = {1994},
    ///  source = {http://www.ncstrl.org:8900/ncstrl/servlet/search?formname=detail\&amp;id=oai%3Ancstrlh%3Acmucs%3ACMU%2F%2FCS-94-125},
    ///  publisher = {Carnegie Mellon University},
    ///  address = {Pittsburgh, PA, USA},
    /// }
    /// </code>
    /// </remarks>
    public sealed class ConjugateGradientDescent : GradientDescentBase<double, IDifferentiableCostFunction<double>>
    {
        /// <summary>
        /// The maximum number of iterations
        /// </summary>
        private int _maxLineSearchIterations = 100;

        /// <summary>
        /// The maximum number of iterations
        /// </summary>
        private double _lineSearchStepSize = 1E-5D;

        /// <summary>
        /// The error tolerance
        /// </summary>
        private double _errorToleranceSquared;

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
                base.ErrorTolerance = value;
                _errorToleranceSquared = value*value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConjugateGradientDescent"/> class.
        /// </summary>
        public ConjugateGradientDescent()
        {
            var epsilon = ErrorTolerance;
            _errorToleranceSquared = epsilon * epsilon;
        }

        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns>IOptimizationResult&lt;TData&gt;.</returns>
        public override IOptimizationResult<double> Minimize(IOptimizationProblem<double, IDifferentiableCostFunction<double>> problem)
        {
            var maxIterations = MaxIterations;
            var epsilonSquare = _errorToleranceSquared;

            // fetch a starting point and obtain the problem size
            var theta = problem.GetInitialCoefficients();
            var problemDimension = theta.Count;

            // we want to restart nonlinear CG at least every n steps,
            // and use this variable as a counter.
            var iterationsUntilReset = problemDimension;

            // now we determine the initial residuals, which are defined to
            // be the opposite gradient direction
            var costFunction = problem.CostFunction;
            var residuals = -costFunction.Jacobian(theta);

            // the initial search direction is along the residuals,
            // which makes the initial step a regular gradient descent.
            var direction = residuals;

            // determine the initial error
            var delta = residuals*residuals;
            var initialDelta = delta;

            // loop for the maximum iteration count
            for (var i = 0; i < maxIterations; ++i)
            {
                // stop if the gradient change is below the threshold
                if (delta <= epsilonSquare*initialDelta)
                {
                    Debug.WriteLine("Stopping CG/S/FR at iteration {0}/{1} because cost |{2}| <= {3}", i, maxIterations, delta, epsilonSquare * initialDelta);
                    break;
                }

                // var cost = costFunction.CalculateCost(x);

                // perform a line search by using the secant method
                theta = LineSearch(costFunction, theta, direction);

                // obtain the new residuals
                residuals = -costFunction.Jacobian(theta);

                // calculate the new error
                var previousDelta = delta;
                delta = residuals*residuals;

                // update the search direction (Fletcher-Reeves)
                var beta = delta/previousDelta;
                direction = residuals + beta*direction;

                // Conjugate Gradient can generate only n conjugate jump directions
                // in n-dimensional space, so we'll reset the algorithm every n steps.
                // reset every n iterations or when the gradient is known to be nonorthogonal
                var shouldRestart = (--iterationsUntilReset == 0);
                var isDescentDirection = (residuals*direction > 0);
                if (shouldRestart || !isDescentDirection)
                {
                    // reset the
                    direction = residuals;

                    // reset the counter
                    iterationsUntilReset = problemDimension;
                }
            }

            // that's it.
            var cost = costFunction.CalculateCost(theta);
            return new OptimizationResult<double>(cost, theta);
        }

        /// <summary>
        /// Performs a line search by using the secant method.
        /// </summary>
        /// <param name="costFunction">The cost function.</param>
        /// <param name="theta">The starting point.</param>
        /// <param name="direction">The search direction.</param>
        /// <returns>The best found minimum point along the <paramref name="direction"/>.</returns>
        [NotNull]
        private Vector<double> LineSearch([NotNull] IDifferentiableCostFunction<double> costFunction, [NotNull] Vector<double> theta, [NotNull] Vector<double> direction)
        {
            var maxLineSearchIteration = _maxLineSearchIterations;
            var initialStepSize = _lineSearchStepSize;
            var epsilonSquare = _errorToleranceSquared;

            // the eta value determines how much the gradient at the current step
            // point differs from the gradient that is orthogonal to the search direction.
            var previousEta = costFunction.Jacobian(theta + initialStepSize*direction)*direction;

            // if our step size is small enough to drop this error term under the
            // thershold, we terminate the search.
            var deltaD = direction*direction;

            // the step size used during line search.
            // this parameter will be adapted during search according to the change in gradient.
            var alpha = -initialStepSize;

            // iteratively find the minimum along the search direction
            for (var j = 0; j < maxLineSearchIteration; ++j)
            {
                // terminate line search if alpha is close enough to zero
                if ((alpha*alpha)*deltaD <= epsilonSquare) break;

                // by multiplying the current gradient with the search direction,
                // we'll end up with a (close to ) zero term if both directions are orthogonal.
                // At this point, alpha will be zero (or close to it), which terminates our search.
                var eta = costFunction.Jacobian(theta)*direction;

                // adjust the step size
                alpha = alpha*eta/(previousEta - eta);
                previousEta = eta;

                // step to the new location along the line
                theta += alpha*direction;
            }

            return theta;
        }
    }
}
