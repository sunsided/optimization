using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization.GradientDescent
{
    /// <summary>
    /// Conjugate-Gradient Descent
    /// </summary>
    public sealed class ConjugateGradientDescent : GradientDescentBase<double, IDifferentiableCostFunction<double>>
    {
        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns>IOptimizationResult&lt;TData&gt;.</returns>
        public override IOptimizationResult<double> Minimize(IOptimizationProblem<double, IDifferentiableCostFunction<double>> problem)
        {
            // fetch a starting point and obtain the problem size
            var x = problem.GetInitialCoefficients();
            var n = x.Count;

            // we want to restart CG at least every n steps,
            // and use this variable as a counter.
            var k = n;

            // var cost = costFunction.CalculateCost(x);

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

            // TODO: add comment and better name
            var jmax = 100; // TODO: ???

            // loop for the maximum iteration count
            var maxIterations = MaxIterations;
            for (var i = 0; i < maxIterations; ++i)
            {
                // stop if the cost change is below the threshold
                if (deltaNew <= epsilonSquare*delta0) break;

                var j = 0;
                var deltaD = d*d;

                // perform a line search by using the secant method
                var alpha = -sigma0;
                var etaPrev = costFunction.Jacobian(x + sigma0*d)*d;
                do
                {
                    var eta = costFunction.Jacobian(x) * d;
                    alpha = alpha*eta/(etaPrev - eta);
                    x += alpha*d;
                    ++j;
                } while (j < jmax && (alpha*alpha)*deltaD > (epsilonSquare)); // TODO: convert to for loop

                // obtain the new residuals
                r = -costFunction.Jacobian(x);

                // Fletcher-Reeves
                deltaOld = deltaNew;
                deltaNew = r*r;
                var beta = deltaNew/deltaOld;
                d = r + beta*d;

                // reset every n iterations or when the
                // gradient is known to be nonorthogonal
                if (--k == 0 || r*d <= 0)
                {
                    d = r;
                    k = n;
                }
            }

            // that's it.
            var cost = costFunction.CalculateCost(x);
            return new OptimizationResult<double>(cost, x);
        }
    }
}
