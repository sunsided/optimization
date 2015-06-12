using System.Diagnostics;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization.GradientDescent
{
    /// <summary>
    /// Conjugate-Gradient Descent using Polak-Ribière conjugation and a Secant Method line search.
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
    public sealed class PolakRibiereConjugateGradientDescent : ConjugateGradientDescentBase<double, IDifferentiableCostFunction<double>>
    {
        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns>IOptimizationResult&lt;TData&gt;.</returns>
        public override IOptimizationResult<double> Minimize(IOptimizationProblem<double, IDifferentiableCostFunction<double>> problem)
        {
            var maxIterations = MaxIterations;
            var epsilonSquare = ErrorToleranceSquared;

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

            // determine a preconditioner
            var preconditioner = GetPreconditioner(problem, theta);

            // get the preconditioned residuals
            var preconditionedResiduals = preconditioner.Inverse()*residuals;

            // the initial search direction is along the residuals,
            // which makes the initial step a regular gradient descent.
            var direction = preconditionedResiduals;

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

                // update the search direction (Polak-Ribière)
                var previousDelta = delta;
                var midDelta = residuals*preconditionedResiduals;

                preconditioner = GetPreconditioner(problem, theta);
                preconditionedResiduals = preconditioner.Inverse()*residuals;

                delta = residuals*preconditionedResiduals;
                var beta = (delta - midDelta)/previousDelta;

                // Conjugate Gradient can generate only n conjugate jump directions
                // in n-dimensional space, so we'll reset the algorithm every n steps.
                // reset every n iterations or when the gradient is known to be nonorthogonal
                var shouldRestart = (--iterationsUntilReset == 0);
                var isDescentDirection = (beta > 0);
                if (shouldRestart || !isDescentDirection)
                {
                    // reset the
                    direction = residuals;

                    // reset the counter
                    iterationsUntilReset = problemDimension;
                }
                else
                {
                    // update the direction
                    direction = residuals + beta*direction;
                }
            }

            // that's it.
            var cost = costFunction.CalculateCost(theta);
            return new OptimizationResult<double>(cost, theta);
        }

        /// <summary>
        /// Gets a preconditioner for the residuals.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <param name="theta">The coefficients to optimize.</param>
        /// <returns>MathNet.Numerics.LinearAlgebra.Matrix&lt;System.Double&gt;.</returns>
        private Matrix<double> GetPreconditioner([NotNull] IOptimizationProblem<double, IDifferentiableCostFunction<double>> problem, Vector<double> theta)
        {
            // sadly we have no clue.
            return Matrix<double>.Build.DiagonalIdentity(theta.Count);
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
            var maxLineSearchIteration = MaxLineSearchIterations;
            var initialStepSize = LineSearchStepSize;
            var epsilonSquare = ErrorToleranceSquared;

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
