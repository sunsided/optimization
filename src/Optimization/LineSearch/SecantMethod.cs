using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using WideMeadows.Optimization.Cost;

namespace WideMeadows.Optimization.LineSearch
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
        /// <param name="previousStepWidth"></param>
        /// <returns>The best found minimum point along the <paramref name="direction"/>.</returns>
        public override double Minimize(IDifferentiableCostFunction<double> costFunction, Vector<double> theta, Vector<double> direction, double previousStepWidth)
        {
            var maxLineSearchIteration = MaxLineSearchIterations;
            var initialStepSize = LineSearchStepSize;
            var epsilonSquare = ErrorToleranceSquared;

            // the eta value determines how much the gradient at the current step
            // point differs from the gradient that is orthogonal to the search direction.
            var gradient = costFunction.Jacobian(theta + initialStepSize*direction);
            var eta = gradient*direction;
            var previousEta = eta;

            // if our step size is small enough to drop this error term under the
            // threshold, we terminate the search.
            var deltaD = direction*direction;

            // the step size used during line search.
            // this parameter will be adapted during search according to the change in gradient.
            var alpha = -initialStepSize;

            // welp
            var cumulativeAlpha = 0.0D;

            // iteratively find the minimum along the search direction
            for (var j = 0; j < maxLineSearchIteration; ++j)
            {
                // terminate line search if alpha is close enough to zero
                var alphaSquare = alpha*alpha;
                Debug.Assert(!double.IsNaN(alphaSquare) && !double.IsInfinity(alphaSquare), string.Format("Numerical instability in alpha� with alpha={0}", alpha));
                if (alphaSquare * deltaD <= epsilonSquare) break;

                // by multiplying the current gradient with the search direction,
                // we'll end up with a (close to ) zero term if both directions are orthogonal.
                // At this point, alpha will be zero (or close to it), which terminates our search.
                gradient = costFunction.Jacobian(theta);
                eta = gradient*direction;
                Debug.Assert(!double.IsInfinity(eta), "!double.IsInfinity(eta)");

                // determine the change in orthogonality error
                var etaChange = previousEta - eta;
                previousEta = eta;
                if (etaChange == 0.0D) break;

                // adjust the step size
                alpha = alpha * eta / etaChange;
                Debug.Assert(alpha.IsFinite(), "alpha.IsFinite()");

                // step to the new location along the line
                theta += alpha*direction;
                cumulativeAlpha += alpha;
            }

            return cumulativeAlpha;
        }
    }
}