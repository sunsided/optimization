using System;
using System.Diagnostics;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.LineSearch;

namespace widemeadows.Optimization.GradientDescent.Conjugate
{
    /// <summary>
    /// Implements Hager-Zhang Conjugate Gradient Descent (CG_DESCENT)
    /// </summary>
    public class HagerZhangCG : DoublePrecisionConjugateGradientDescentBase<IDifferentiableCostFunction<double>>, IHagerZhangLineSearchParameters
    {
        /// <summary>
        /// eta, enters into the lower bound for βNk through ηk
        /// </summary>
        /// <remarks>
        /// Range (0, ∞)
        /// </remarks>
        private double _η = .01D;

        /// <summary>
        /// eta, enters into the lower bound for βNk through ηk
        /// </summary>
        /// <value>The η.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be positive.</exception>
        public double η
        {
            get { return _η; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("value", value, "Value must be positive.");
                _η = value;
            }
        }

        #region Line search parameters

        /// <summary>
        /// Gets the line search.
        /// </summary>
        /// <value>The line search.</value>
        [NotNull]
        private new HagerZhangLineSearch LineSearch
        {
            get { return (HagerZhangLineSearch)base.LineSearch; }
        }

        /// <summary>
        /// delta, used in the Wolfe conditions
        /// </summary>
        /// <value>The δ.</value>
        public double δ { get { return LineSearch.δ; } set { LineSearch.δ = value; } }

        /// <summary>
        /// sigma, used in the Wolfe conditions
        /// </summary>
        /// <value>The σ.</value>
        public double σ { get { return LineSearch.σ; } set { LineSearch.σ = value; } }

        /// <summary>
        /// epsilon, used in the approximate Wolfe termination
        /// </summary>
        /// <value>The ε.</value>
        /// <remarks>Range [0, ∞)</remarks>
        public double ε { get { return LineSearch.ε; } set { LineSearch.ε = value; } }

        /// <summary>
        /// omega, used in switching from Wolfe to approximate Wolfe conditions
        /// </summary>
        /// <value>The ω.</value>
        /// <remarks>Range [0, 1]</remarks>
        public double ω { get { return LineSearch.ω; } set { LineSearch.ω = value; } }

        /// <summary>
        /// Delta, decay factor for Qk in the recurrence
        /// </summary>
        /// <value>The δ.</value>
        public double Δ { get { return LineSearch.Δ; } set { LineSearch.Δ = value; } }

        /// <summary>
        /// theta, used in the update rules when the potential intervals [a, c]
        /// or [c, b] violate the opposite slope condition contained in
        /// </summary>
        /// <value>The θ.</value>
        public double θ { get { return LineSearch.θ; } set { LineSearch.θ = value; } }

        /// <summary>
        /// gamma, determines when a bisection step is performed
        /// </summary>
        /// <value>The γ.</value>
        public double γ { get { return LineSearch.γ; } set { LineSearch.γ = value; } }

        /// <summary>
        /// rho, expansion factor used in the bracket rule
        /// </summary>
        /// <value>The ρ.</value>
        /// <remarks>Range (1, ∞)</remarks>
        public double ρ { get { return LineSearch.ρ; } set { LineSearch.ρ = value; } }

        /// <summary>
        /// psi 0, small factor used in starting guess
        /// </summary>
        /// <value>The ψ0.</value>
        public double ψ0 { get { return LineSearch.ψ0; } set { LineSearch.ψ0 = value; } }

        /// <summary>
        /// psi 1, small factor
        /// </summary>
        /// <value>The ψ1.</value>
        public double ψ1 { get { return LineSearch.ψ1; } set { LineSearch.ψ1 = value; } }

        /// <summary>
        /// psi 2, factor multiplying previous step α(k−1)
        /// </summary>
        /// <value>The ψ2.</value>
        public double ψ2 { get { return LineSearch.ψ2; } set { LineSearch.ψ2 = value; } }

        /// <summary>
        /// Determines if QuadStep is used.
        /// </summary>
        /// <value><see langword="true" /> if [quad step]; otherwise, <see langword="false" />.</value>
        public bool QuadStep { get { return LineSearch.QuadStep; } set { LineSearch.QuadStep = value; } }

        /// <summary>
        /// alpha 0, the initial alpha value for the first iteration.
        /// </summary>
        /// <value>The α0.</value>
        /// <remarks>Range (0, ∞)</remarks>
        public double α0 { get { return LineSearch.α0; } set { LineSearch.α0 = value; } }

        /// <summary>
        /// Gets or sets the maximum number of bracketing iterations.
        /// </summary>
        /// <value>The maximum bracketing iterations.</value>
        public int MaxBracketingIterations { get { return LineSearch.MaxBracketingIterations; } set { LineSearch.MaxBracketingIterations = value; } }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HagerZhangCG"/> class.
        /// </summary>
        public HagerZhangCG()
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

            // calculate delta (we'll use the gradient instead of the residuals here,
            // since it's just a metter of sign)
            delta = g*g;

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
