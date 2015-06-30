using System.Diagnostics;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.LineSearch;

namespace widemeadows.Optimization.GradientDescent.ConjugateGradients
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
    public sealed class FletcherReevesConjugateGradientDescent : DoublePrecisionConjugateGradientDescentBase<IDifferentiableCostFunction<double>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FletcherReevesConjugateGradientDescent"/> class.
        /// </summary>
        /// <param name="lineSearch">The line search.</param>
        public FletcherReevesConjugateGradientDescent([NotNull] ILineSearch<double, IDifferentiableCostFunction<double>> lineSearch)
            : base(lineSearch)
        {
        }

        /// <summary>
        /// Initializes the algorithm.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <param name="theta">The theta.</param>
        /// <param name="residuals">The initial residuals.</param>
        /// <param name="searchDirection">The initial search direction.</param>
        /// <returns>The state to be passed to the <see cref="UpdateDirection" /> function.</returns>
        protected override object InitializeAlgorithm(IOptimizationProblem<double, IDifferentiableCostFunction<double>> problem, Vector<double> theta, Vector<double> residuals, out Vector<double> searchDirection)
        {
            // the initial search direction is along the residuals,
            // which makes the initial step a regular gradient descent.
            searchDirection = residuals.Normalize(2);

            // return some state information
            return null;
        }

        /// <summary>
        /// Determines the beta coefficient used to update the direction.
        /// </summary>
        /// <param name="internalState">The algorithm's internal state.</param>
        /// <param name="theta">The theta.</param>
        /// <param name="residuals">The residuals.</param>
        /// <param name="direction">The search direction.</param>
        /// <param name="delta">The squared norm of the residuals.</param>
        /// <returns><see langword="true" /> if the algorithm should continue, <see langword="false" /> if the algorithm should restart.</returns>
        protected override bool UpdateDirection(object internalState, Vector<double> theta, Vector<double> residuals, ref Vector<double> direction, ref double delta)
        {
            Debug.Assert(internalState == null, "internalState == null");

            // calculate the new error
            var previousDelta = delta;
            delta = residuals * residuals;

            // update the search direction (Fletcher-Reeves)
            var beta = delta / previousDelta;
            direction = residuals + beta * direction;
            direction = direction.Normalize(2);

            // if this is not a descent direction, then restart
            return (residuals*direction > 0);
        }
    }
}
