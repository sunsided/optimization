using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization.LineSearch
{
    /// <summary>
    /// Class SecantMethodConjugateGradientDescentBase.
    /// </summary>
    public sealed class SecantMethod : LineSearchBase<double, IDifferentiableCostFunction<double>>
    {
        /// <summary>
        /// Performs a line search by using the secant method.
        /// </summary>
        /// <param name="costFunction">The cost function.</param>
        /// <param name="theta">The starting point.</param>
        /// <param name="direction">The search direction.</param>
        /// <returns>The best found minimum point along the <paramref name="direction"/>.</returns>
        public override Vector<double> LineSearch(IDifferentiableCostFunction<double> costFunction, Vector<double> theta, Vector<double> direction)
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
                Debug.Assert(!double.IsInfinity(eta), "!double.IsInfinity(eta)");

                // adjust the step size
                alpha = alpha*eta/(previousEta - eta);
                Debug.Assert(!double.IsNaN(alpha), "!double.IsNaN(alpha)");

                previousEta = eta;

                // step to the new location along the line
                theta += alpha*direction;
            }

            return theta;
        }
    }
}