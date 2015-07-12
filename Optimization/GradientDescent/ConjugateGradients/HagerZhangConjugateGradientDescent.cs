using System;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.LineSearch;

namespace widemeadows.Optimization.GradientDescent.ConjugateGradients
{
    /// <summary>
    /// Implements Hager-Zhang Conjugate Gradient Descent (CG_DESCENT)
    /// </summary>
    public class HagerZhangConjugateGradientDescent : DoublePrecisionConjugateGradientDescentBase<IDifferentiableCostFunction<double>>
    {
        /// <summary>
        /// eta, enters into the lower bound for βNk through ηk
        /// </summary>
        /// <remarks>
        /// Range (0, ∞)
        /// </remarks>
        private double _η = .01D;

        /// <summary>
        /// Initializes a new instance of the <see cref="HagerZhangConjugateGradientDescent"/> class.
        /// </summary>
        public HagerZhangConjugateGradientDescent()
            : base(new HagerZhangLineSearch())
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
            searchDirection = -residuals.Normalize(2);

            // at this point, no previous gradient exists
            var previousGradient = Vector<double>.Build.Dense(residuals.Count, Vector<double>.Zero);

            // no state required
            return new State(previousGradient, problem);
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
            var state = (State)internalState;
            var function = state.Problem.CostFunction;

            // prefetch
            var η = _η;

            // determine the gradient at the current location
            var g = function.Jacobian(location);

            // fetch the change in gradient
            var y = g - state.PreviousGradient;

            // calculate betaN
            var yy = y*y;
            var dy = direction*y;
            var beta = 1/(dy)*(y - 2*direction*(yy)/(dy))*g;

            // calculate etak
            var ηk = -1/(direction.L2Norm()*Math.Min(η, g.L2Norm()));

            // calculate beta-bar
            beta = Math.Max(beta, ηk);

            // update the direction
            direction = -g + beta*direction;
            return true;
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
            /// The previous gradient
            /// </summary>
            public Vector<double> PreviousGradient;

            /// <summary>
            /// Initializes a new instance of the <see cref="State"/> class.
            /// </summary>
            /// <param name="previousGradient">The previous gradient.</param>
            public State(Vector<double> previousGradient, IOptimizationProblem<double, IDifferentiableCostFunction<double>> problem)
            {
                PreviousGradient = previousGradient;
                Problem = problem;
            }
        }
    }
}
