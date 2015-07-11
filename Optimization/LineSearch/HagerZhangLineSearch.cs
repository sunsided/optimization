using System;
using System.Diagnostics;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization.LineSearch
{
    /// <summary>
    /// Implements Hager-Zhang Conjugate Gradient Descent (CG_DESCENT)
    /// </summary>
    class HagerZhangLineSearch : ILineSearch<double, IDifferentiableCostFunction<double>>, IIterationAware
    {
        /// <summary>
        /// delta, used in the Wolfe conditions
        /// </summary>
        /// <remarks>
        /// Range (0, 0.5)
        /// </remarks>
        private double _δ = .1D;

        /// <summary>
        /// sigma, used in the Wolfe conditions
        /// </summary>
        /// <remarks>
        /// Range [<see cref="_δ"/>, 1)
        /// </remarks>
        private double _σ = .9D;

        /// <summary>
        /// epsilon, used in the approximate Wolfe termination
        /// </summary>
        /// <remarks>
        /// Range [0, ∞)
        /// </remarks>
        private double _ɛ​ = 1E-6D;

        /// <summary>
        /// omega, used in switching fromWolfe to approximateWolfe conditions
        /// </summary>
        /// <remarks>
        /// Range [0, 1]
        /// </remarks>
        private double _ω = 1E-3D;

        /// <summary>
        /// Delta, decay factor for Qk in the recurrence
        /// </summary>
        /// <remarks>
        /// Range [0, 1]
        /// </remarks>
        private double _Δ = .7D;

        /// <summary>
        /// theta, used in the update rules when the potential intervals [a, c]
        /// or [c, b] violate the opposite slope condition contained in
        /// </summary>
        /// <remarks>
        /// Range (0, 1)
        /// </remarks>
        private double _θ = .5D;

        /// <summary>
        /// gamma, determines when a bisection step is performed
        /// </summary>
        /// <remarks>
        /// Range (0, 1)
        /// </remarks>
        private double _γ = .66D;

        /// <summary>
        /// eta, enters into the lower bound for βNk through ηk
        /// </summary>
        /// <remarks>
        /// Range (0, ∞)
        /// </remarks>
        private double _η = .01D;

        /// <summary>
        /// rho, expansion factor used in the bracket rule
        /// </summary>
        /// <remarks>
        /// Range (1, ∞)
        /// </remarks>
        private double _ρ = 5;

        /// <summary>
        /// psi 0, small factor used in starting guess
        /// </summary>
        /// <remarks>
        /// Range (0, 1)
        /// </remarks>
        private double _ψ0 = .01D;

        /// <summary>
        /// psi 1, small factor
        /// </summary>
        /// <remarks>
        /// Range (0, 1)
        /// </remarks>
        private double _ψ1 = .1D;

        /// <summary>
        /// psi 2, factor multiplying previous step α(k−1)
        /// </summary>
        /// <remarks>
        /// Range (1, ∞)
        /// </remarks>
        private double _ψ2 = 2;

        /// <summary>
        /// Determines if QuadStep is used.
        /// </summary>
        private bool _quadStep = true;

        /// <summary>
        /// The current iteration
        /// </summary>
        private int _currentIteration = 0;

        /// <summary>
        /// Minimizes the <paramref name="function" /> by performing a line search along the <paramref name="direction" />, starting from the given <paramref name="location" />.
        /// </summary>
        /// <param name="function">The cost function.</param>
        /// <param name="location">The starting point.</param>
        /// <param name="direction">The search direction.</param>
        /// <returns>The best found minimum point along the <paramref name="direction" />.</returns>
        public Vector<double> Minimize(IDifferentiableCostFunction<double> function, Vector<double> location, Vector<double> direction)
        {
            Debug.Assert(Math.Abs(direction.Norm(2)) < 1E-3, "Math.Abs(direction.Norm(2)) < 1E-3");

            // convenience function for the evaluation
            var φ = Getφ(function, location, direction);
            var dφ = GetDφ(function, location, direction);

            throw new NotImplementedException("aww yeah");
        }

        /// <summary>
        /// Gets the directional derivative φ'(α).
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="location">The location.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>Func&lt;System.Double, System.Double&gt;.</returns>
        private static Func<double, double> GetDφ([NotNull] IDifferentiableCostFunction<double> function, [NotNull] Vector<double> location, [NotNull] Vector<double> direction)
        {
            return alpha => function.Jacobian(location + alpha*direction)*direction;
        }

        /// <summary>
        /// Gets the φ(α) function.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="location">The location.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>Func&lt;System.Double, System.Double&gt;.</returns>
        private static Func<double, double> Getφ([NotNull] ICostFunction<double> function, [NotNull] Vector<double> location, [NotNull] Vector<double> direction)
        {
            return alpha => function.CalculateCost(location + alpha*direction);
        }

        /// <summary>
        /// Sets the iteration.
        /// </summary>
        /// <param name="i">The i.</param>
        public void SetIteration(int i)
        {
            _currentIteration = i;
        }
    }
}
