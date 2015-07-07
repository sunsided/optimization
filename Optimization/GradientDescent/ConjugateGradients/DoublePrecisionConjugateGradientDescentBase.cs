using System;
using System.Diagnostics;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.LineSearch;

namespace widemeadows.Optimization.GradientDescent.ConjugateGradients
{
    /// <summary>
    /// Class DoublePrecisionConjugateGradientDescentBase.
    /// </summary>
    /// <typeparam name="TCostFunction">The type of the t cost function.</typeparam>
    public abstract class DoublePrecisionConjugateGradientDescentBase<TCostFunction> : ConjugateGradientDescentBase<double, TCostFunction>
        where TCostFunction : IDifferentiableCostFunction<double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoublePrecisionConjugateGradientDescentBase{TCostFunction}"/> class.
        /// </summary>
        /// <param name="lineSearch">The line search algorithm.</param>
        protected DoublePrecisionConjugateGradientDescentBase([NotNull] ILineSearch<double, TCostFunction> lineSearch)
            :base(lineSearch)
        {
        }

        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns>IOptimizationResult&lt;TData&gt;.</returns>
        public sealed override IOptimizationResult<double> Minimize(IOptimizationProblem<double, TCostFunction> problem)
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

            // the initial search direction is along the residuals,
            // which makes the initial step a regular gradient descent.
            // However, some CG algorithms (especially when using a preconditioner)
            // might yield slightly different directions, so we'll leave that to
            // the initialization function.
            Vector<double> direction; // = residuals

            // initialize the algorithm
            object state = InitializeAlgorithm(problem, theta, residuals, out direction);

            // we require the direction to be normalized
            Debug.Assert(Math.Abs(direction.Norm(2) - 1.0D) < 1E-5D, "Math.Abs(direction.Norm(2) - 1.0D) < 1E-5D");

            // determine the initial error
            var delta = residuals * residuals;
            var initialDelta = delta;

            // loop for the maximum iteration count
            for (var i = 0; i < maxIterations; ++i)
            {
                // stop if the gradient change is below the threshold
                if (delta <= epsilonSquare * initialDelta) // TODO the scaling with initialDelta does do some trouble every now and then ...
                {
                    Debug.WriteLine("Stopping CG/S/FR at iteration {0}/{1} because cost |{2}| <= {3}", i, maxIterations, delta, epsilonSquare * initialDelta);
                    break;
                }

                // var cost = costFunction.CalculateCost(x);

                // perform a line search to find the minimum along the given direction
                theta = LineSearch(costFunction, theta, direction);

                // obtain the new residuals
                residuals = -costFunction.Jacobian(theta);

                // obtain the update parameter
                var shouldContinue = UpdateDirection(state, theta, residuals, ref direction, ref delta);

                // we require the direction to be normalized
                Debug.Assert(Math.Abs(direction.Norm(2) - 1.0D) < 1E-5D, "Math.Abs(direction.Norm(2) - 1.0D) < 1E-5D");

                // Conjugate Gradient can generate only n conjugate search directions
                // in n-dimensional space, so we'll reset the algorithm every n steps in order
                // to give nonlinear optimization a chance.
                // Alternatively, when the implementation decides that the resulting search direction
                // would be non-A-conjugate (e.g. non-orthogonal to all previous search directions),
                // reset is triggered as well.
                var shouldReset = (--iterationsUntilReset == 0) || !shouldContinue;
                if (shouldReset)
                {
                    // reset the search direction to point towards the current
                    // residuals (which are opposite of the gradient!)
                    direction = residuals.Normalize(2);

                    // reset the counter
                    iterationsUntilReset = problemDimension;
                }
            }

            // that's it.
            var cost = costFunction.CalculateCost(theta);
            return new OptimizationResult<double>(cost, theta);
        }

        /// <summary>
        /// Initializes the algorithm.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <param name="residuals">The initial residuals.</param>
        /// <param name="searchDirection">The initial search direction.</param>
        /// <returns>The state to be passed to the <see cref="UpdateDirection" /> function.</returns>
        protected abstract object InitializeAlgorithm([NotNull] IOptimizationProblem<double, TCostFunction> problem, Vector<double> theta, Vector<double> residuals, out Vector<double> searchDirection);

        /// <summary>
        /// Determines the beta coefficient used to update the direction.
        /// </summary>
        /// <param name="internalState">The algorithm's internal state.</param>
        /// <param name="theta">The theta.</param>
        /// <param name="residuals">The residuals.</param>
        /// <param name="direction">The search direction.</param>
        /// <param name="delta">The squared norm of the residuals.</param>
        /// <returns><see langword="true" /> if the algorithm should continue, <see langword="false" /> if the algorithm should restart.</returns>
        protected abstract bool UpdateDirection([CanBeNull] object internalState, [NotNull] Vector<double> theta, [NotNull] Vector<double> residuals, ref Vector<double> direction, ref double delta);
    }
}
