using System;
using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization.GradientDescent
{
    /// <summary>
    /// Conjugate-Gradient Descent
    /// </summary>
    public sealed class ConjugateGradientDescent : GradientDescentBase<double, ICostGradient<double>>
    {
        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns>IOptimizationResult&lt;TData&gt;.</returns>
        public override IOptimizationResult<double> Minimize(IOptimizationProblem<double, ICostGradient<double>> problem)
        {
            // fetch a starting point and obtain the problem size
            var theta = problem.GetInitialCoefficients();
            var n = theta.Count;

            // in order to obtain the initial residuals, we'll calculate the cost once
            var costFunction = problem.CostFunction;
            var costResult = costFunction.CalculateCost(theta);

            // now we determine the initial residuals, which are defined to
            // be the opposite gradient direction
            var residuals = -costResult.CostGradient;

            // we want to restart CG at least every n steps,
            // and use this variable as a counter.
            var iterationsUntilRestart = n;

            // TODO: add comment and better name
            var d = residuals;

            // TODO: add comment and better name
            var deltaNew = residuals*residuals;
            var delta0 = deltaNew;

            // loop for the maximum iteration count
            var maxIterations = MaxIterations;
            for (var i = 0; i < maxIterations; ++i)
            {
                var j = 0;
                var deltaD = d*d;
                var jmax = 100; // TODO: well ...
                var alpha = 1; // TODO: umh ...
                var epsilon = 1E-10D; // TODO: yeah ...!

                // perform a line search
                // TODO: probably ...
                do
                {
                    alpha = 0; // TODO: second derivative required here!
                    theta += alpha*d;
                    ++j;
                } while (j < jmax && (alpha*alpha)*deltaD > (epsilon*epsilon));

                // obtain the new residuals
                residuals = -costResult.CostGradient; // TODO: needs to be calculated above

                // --------------------------------------------------------------------------------
                // TODO: The cost function and its gradients must be available from different calls
                // --------------------------------------------------------------------------------
            }

            throw new NotImplementedException("Conjugate-Gradient Descent not implemented");
        }
    }
}
