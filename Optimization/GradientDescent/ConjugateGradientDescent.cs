using System;
using System.Diagnostics;
using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization.GradientDescent
{
    /// <summary>
    /// Conjugate-Gradient Descent
    /// </summary>
    public sealed class ConjugateGradientDescent : GradientDescentBase<double, IDifferentiableCostFunction<double>>
    {
        /// <summary>
        /// The maximum number of iterations
        /// </summary>
        private int _maxLineSearchIterations = 100;

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
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns>IOptimizationResult&lt;TData&gt;.</returns>
        public override IOptimizationResult<double> Minimize(IOptimizationProblem<double, IDifferentiableCostFunction<double>> problem)
        {
            // determine the iteration counts
            var maxIterations = MaxIterations;
            var maxLineSearchIteration = _maxLineSearchIterations;

            // fetch a starting point and obtain the problem size
            var x = problem.GetInitialCoefficients();
            var n = x.Count;

            // we want to restart CG at least every n steps,
            // and use this variable as a counter.
            var k = n;

            // now we determine the initial residuals, which are defined to
            // be the opposite gradient direction
            var costFunction = problem.CostFunction;
            var r = -costFunction.Jacobian(x);

            // TODO: add comment and better name
            var d = r;

            // TODO: add comment and better name
            var sigma0 = 1E-5D; // TODO ???

            // TODO: add comment and better name
            var deltaNew = r*r;
            var delta0 = deltaNew;
            var deltaOld = delta0; // will be overwritten later

            // the cost threshold
            var epsilon = 1E-10D;
            var epsilonSquare = epsilon*epsilon;

            // loop for the maximum iteration count
            for (var i = 0; i < maxIterations; ++i)
            {
                // stop if the gradient change is below the threshold
                if (deltaNew <= epsilonSquare*delta0)
                {
                    Debug.WriteLine("Stopping CG/S/FR at iteration {0}/{1} because cost |{2}| <= {3}", i, maxIterations, deltaNew, epsilonSquare * delta0);
                    break;
                }

                // var cost = costFunction.CalculateCost(x);

                // perform a line search by using the secant method
                {
                    var alpha = -sigma0;
                    var etaPrev = costFunction.Jacobian(x + sigma0*d)*d;
                    var deltaD = d*d; // used for terminating the line search
                    for (var j = 0; j < maxLineSearchIteration; ++j)
                    {
                        // terminate line search
                        if ((alpha*alpha)*deltaD <= epsilonSquare)
                        {
                            break;
                        }

                        var eta = costFunction.Jacobian(x)*d;
                        alpha = alpha*eta/(etaPrev - eta);
                        x += alpha*d;
                    }
                }

                // obtain the new residuals
                r = -costFunction.Jacobian(x);

                // Fletcher-Reeves
                deltaOld = deltaNew;
                deltaNew = r*r;
                var beta = deltaNew/deltaOld;
                d = r + beta*d;

                // Conjugate Gradient can generate only n conjugate jump directions
                // in n-dimensional space, so we'll reset the algorithm every n steps.
                // reset every n iterations or when the gradient is known to be nonorthogonal
                var shouldRestart = (--k == 0);
                var isDescentDirection = (r*d > 0);
                if (shouldRestart || !isDescentDirection)
                {
                    // reset the
                    d = r;

                    // reset the counter
                    k = n;
                }
            }

            // that's it.
            var cost = costFunction.CalculateCost(x);
            return new OptimizationResult<double>(cost, x);
        }
    }
}
