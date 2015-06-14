using System.Diagnostics;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.LineSearch;

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
        /// Initializes a new instance of the <see cref="FletcherReevesConjugateGradientDescent"/> class.
        /// </summary>
        /// <param name="lineSearch">The line search.</param>
        public PolakRibiereConjugateGradientDescent([NotNull] ILineSearch<double, IDifferentiableCostFunction<double>> lineSearch)
            : base(lineSearch)
        {
        }

        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns>IOptimizationResult&lt;TData&gt;.</returns>
        public override IOptimizationResult<double> Minimize(IOptimizationProblem<double, IDifferentiableCostFunction<double>> problem)
        {
            var maxIterations = MaxIterations;

            // The idea is that we should stop the operation if ||residuals|| < epsilon.
            // Since the norm calculation requires taking the square root,
            // we instead square epsilon and compare against that.
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
                    Debug.WriteLine("Stopping CG/S/PR at iteration {0}/{1} because cost |{2}| <= {3}", i, maxIterations, delta, epsilonSquare * initialDelta);
                    break;
                }

                // var cost = costFunction.CalculateCost(x);

                // perform a line search to find the minimum along the given direction
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
        private Matrix<double> GetPreconditioner([NotNull, UsedImplicitly] IOptimizationProblem<double, IDifferentiableCostFunction<double>> problem, Vector<double> theta)
        {
            // sadly we have no clue.
            return Matrix<double>.Build.DiagonalIdentity(theta.Count);
        }
    }
}
