using System;
using System.Diagnostics;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.LineSearch;

namespace widemeadows.Optimization.GradientDescent.ConjugateGradients
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
    public sealed class PolakRibiereConjugateGradientDescent : DoublePrecisionConjugateGradientDescentBase<IDifferentiableCostFunction<double>>
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
        /// Initializes the algorithm.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <param name="location">The theta.</param>
        /// <param name="residuals">The initial residuals.</param>
        /// <param name="searchDirection">The initial search direction.</param>
        /// <returns>The state to be passed to the <see cref="UpdateDirection" /> function.</returns>
        protected override object InitializeAlgorithm(IOptimizationProblem<double, IDifferentiableCostFunction<double>> problem, Vector<double> location, Vector<double> residuals, out Vector<double> searchDirection)
        {
            // determine a preconditioner
            var preconditioner = GetPreconditioner(problem, location);

            // get the preconditioned residuals
            var preconditionedResiduals = preconditioner.Inverse() * residuals;

            // the initial search direction is along the residuals,
            // which makes the initial step a regular gradient descent.
            searchDirection = preconditionedResiduals;

            // return some state information
            return new State(problem, preconditionedResiduals);
        }

        /// <summary>
        /// Determines the beta coefficient used to update the direction.
        /// </summary>
        /// <param name="internalState">The algorithm's internal state.</param>
        /// <param name="location">The theta.</param>
        /// <param name="residuals">The residuals.</param>
        /// <param name="direction">The search direction.</param>
        /// <param name="delta">The squared norm of the residuals.</param>
        /// <returns><see langword="true" /> if the algorithm should continue, <see langword="false" /> if the algorithm should restart.</returns>
        protected override bool UpdateDirection(object internalState, Vector<double> location, Vector<double> residuals, ref Vector<double> direction, ref double delta)
        {
            Debug.Assert(internalState != null, "internalState != null");
            var state = (State) internalState;

            // fetch the old state
            var problem = state.Problem;
            var preconditionedResiduals = state.PreviousPreconditionedResiduals;

            // update the search direction (Polak-Ribière)
            var previousDelta = delta;
            var midDelta = residuals * preconditionedResiduals;

            var preconditioner = GetPreconditioner(problem, location);
            preconditionedResiduals = preconditioner.Inverse() * residuals;

            delta = residuals * preconditionedResiduals;
            var beta = (delta - midDelta) / previousDelta;

            // store the preconditioned residuals for the next iteration
            state.PreviousPreconditionedResiduals = preconditionedResiduals;

            // check the reset condition for Polak-Rebière
            if (!(beta > 0)) return false;

            // update the direction
            direction = residuals + beta * direction;
            return true;
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


        /// <summary>
        /// The algorithm's state.
        /// </summary>
        private sealed class State
        {
            /// <summary>
            /// The problem
            /// </summary>
            public readonly IOptimizationProblem<double, IDifferentiableCostFunction<double>> Problem;

            /// <summary>
            /// The previous preconditioned residuals
            /// </summary>
            public Vector<double> PreviousPreconditionedResiduals;

            /// <summary>
            /// Initializes a new instance of the <see cref="State"/> class.
            /// </summary>
            /// <param name="problem">The problem.</param>
            /// <param name="previousPreconditionedResiduals">The previous preconditioned residuals.</param>
            public State(IOptimizationProblem<double, IDifferentiableCostFunction<double>> problem, Vector<double> previousPreconditionedResiduals)
            {
                Problem = problem;
                PreviousPreconditionedResiduals = previousPreconditionedResiduals;
            }
        }
    }
}
