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
    class HagerZhangLineSearch : ILineSearch<double, IDifferentiableCostFunction<double>>
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
        private bool _quadStepEnabled = true;

        /// <summary>
        /// alpha 0, the initial alpha value for the first iteration.
        /// </summary>
        /// <remarks>
        /// Range (0, ∞)
        /// </remarks>
        private double _α0 = double.NaN;

        /// <summary>
        /// Minimizes the <paramref name="function" /> by performing a line search along the <paramref name="direction" />, starting from the given <paramref name="location" />.
        /// </summary>
        /// <param name="function">The cost function.</param>
        /// <param name="location">The starting point.</param>
        /// <param name="direction">The search direction.</param>
        /// <param name="previousStepWidth">The previous step width α. In the initial iteration, this value should be <c>0.0D</c>.</param>
        /// <returns>The best found minimum point along the <paramref name="direction" />.</returns>
        /// <exception cref="System.NotImplementedException">aww yeah</exception>
        public double Minimize(IDifferentiableCostFunction<double> function, Vector<double> location, Vector<double> direction, double previousStepWidth)
        {
            Debug.Assert(Math.Abs(direction.L2Norm()) < 1E-3, "Math.Abs(direction.Norm(2)) < 1E-3");

            // convenience function for the evaluation
            var φ = Getφ(function, location, direction);
            var dφ = GetDφ(function, location, direction);

            // determine the starting values
            var φ0 = φ(0);
            var dφ0 = dφ(0);
            var f0 = φ0;
            var Δf0 = function.Jacobian(location);

            // find a starting point and check if that solution is already good enough
            var c = DetermineInitialSearchPoint(previousStepWidth, location, f0, Δf0);
            if (ShouldTerminate(φ, dφ, c, φ0, dφ0)) return c;

            throw new NotImplementedException("aww yeah");
        }

        /// <summary>
        /// Determines the initial search point.
        /// </summary>
        /// <param name="αprev">The previous α value.</param>
        /// <returns>System.Double.</returns>
        private double DetermineInitialSearchPoint(double αprev, [NotNull] Vector<double> location, double valueAtLocation, [NotNull] Vector<double> gradientAtLocation)
        {
            // prefetch
            var ψ0 = _ψ0;
            var ψ1 = _ψ1;
            var ψ2 = _ψ2;
            var α0 = _α0;

            // check if this is the first iteration
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var isFirstIteration = αprev == 0.0D;
            if (isFirstIteration)
            {
                // if there is a user-defined starting value, then we'll use it.
                if (α0.IsFinite()) return α0;

                // if the starting point is nonzero, calculate a better alpha
                var supremumNormOfLocation = location.AbsoluteMaximum();
                if (supremumNormOfLocation > 0.0D)
                {
                    var supremumNormOfGradientAtLocation = gradientAtLocation.AbsoluteMaximum();
                    return ψ0*supremumNormOfLocation/supremumNormOfGradientAtLocation;
                }

                // if the function value is nonzero, calculate the next better alpha
                var absoluteOfValueAtLocation = Math.Abs(valueAtLocation);
                if (absoluteOfValueAtLocation > 0.0D)
                {
                    var squaredEuclideanNormOfGradientAtLocation = gradientAtLocation*gradientAtLocation;
                    return ψ0*absoluteOfValueAtLocation/squaredEuclideanNormOfGradientAtLocation;
                }

                // in any other case, use α = 1
                return 1.0D;
            }
        }

        /// <summary>
        /// Determines if the algorithm should terminate, given the currently selected step width <paramref name="α" />,
        /// the starting point for the search <paramref name="φ0" /> and the local directional derivative <paramref name="dφ0" />,
        /// as well as the functions <paramref name="φ" /> and its directional derivative <paramref name="dφ" />.
        /// </summary>
        /// <param name="φ">The function φ(α).</param>
        /// <param name="dφ">The function φ'(α).</param>
        /// <param name="α">The selected step width.</param>
        /// <param name="φ0">The function value at the starting point.</param>
        /// <param name="dφ0">The directional derivative of the function value at the starting point.</param>
        /// <returns><see langword="true" /> if the line search should terminate, <see langword="false" /> otherwise.</returns>
        private bool ShouldTerminate(Func<double, double> φ, Func<double, double> dφ, double α, double φ0, double dφ0)
        {
            // calculate the function values at α
            var φα = φ(α);
            var dφα = dφ(α);

            // delegate
            return ShouldTerminate(α, φ0, dφ0, φα, dφα);
        }

        /// <summary>
        /// Determines if the algorithm should terminate, given the currently selected step width <paramref name="α"/>,
        /// the starting point for the search <paramref name="φ0"/> and the local directional derivative <paramref name="dφ0"/>,
        /// as well as the function value at the new point <paramref name="φα"/> and its directional derivative <paramref name="dφα"/>.
        /// </summary>
        /// <param name="α">The selected step width.</param>
        /// <param name="φ0">The function value at the starting point.</param>
        /// <param name="dφ0">The directional derivative of the function value at the starting point.</param>
        /// <param name="φα">The function value at <paramref name="α"/>.</param>
        /// <param name="dφα">The directional derivative at <paramref name="α"/>.</param>
        /// <returns><see langword="true" /> if the line search should terminate, <see langword="false" /> otherwise.</returns>
        private bool ShouldTerminate(double α, double φ0, double dφ0, double φα, double dφα)
        {
            return OriginalWolfeConditionsFulfilled(α, φ0, dφ0, φα, dφα)
                   || ApproximateWolfeConditionsFulfilled(φ0, dφ0, φα, dφα);
        }

        /// <summary>
        /// Determines if the original Wolfe conditions are fulfilled, given the currently selected step width <paramref name="α"/>,
        /// the starting point for the search <paramref name="φ0"/> and the local directional derivative <paramref name="dφ0"/>,
        /// as well as the function value at the new point <paramref name="φα"/> and its directional derivative <paramref name="dφα"/>.
        /// </summary>
        /// <param name="α">The selected step width.</param>
        /// <param name="φ0">The function value at the starting point.</param>
        /// <param name="dφ0">The directional derivative of the function value at the starting point.</param>
        /// <param name="φα">The function value at <paramref name="α"/>.</param>
        /// <param name="dφα">The directional derivative at <paramref name="α"/>.</param>
        /// <returns><see langword="true" /> if the original Wolfe conditions are fulfilled, <see langword="false" /> otherwise.</returns>
        private bool OriginalWolfeConditionsFulfilled(double α, double φ0, double dφ0, double φα, double dφα)
        {
            // prefetch
            var δ = _δ; // delta
            var σ = _σ; // delta

            // check for sufficient decrease (Armijo rule)
            var decreaseIsSufficient = (φα - φ0) <= (δ*α*dφ0);

            // check for sufficient curvature decrease
            var curvatureDecreaseIsSufficient = dφα >= σ*dφ0;

            return decreaseIsSufficient && curvatureDecreaseIsSufficient;
        }

        /// <summary>
        /// Determines if the original Wolfe conditions are fulfilled, given the the starting point for the search <paramref name="φ0"/>
        /// and the local directional derivative <paramref name="dφ0"/>,
        /// as well as the function value at the new point <paramref name="φα"/> and its directional derivative <paramref name="dφα"/>.
        /// </summary>
        /// <param name="φ0">The function value at the starting point.</param>
        /// <param name="dφ0">The directional derivative of the function value at the starting point.</param>
        /// <param name="φα">The function value at the new evaluation point.</param>
        /// <param name="dφα">The directional derivative at the new evaluation point.</param>
        /// <returns><see langword="true" /> if the approximate Wolfe conditions are fulfilled, <see langword="false" /> otherwise.</returns>
        private bool ApproximateWolfeConditionsFulfilled(double φ0, double dφ0, double φα, double dφα)
        {
            // prefetch
            var δ = _δ; // delta
            var σ = _σ; // delta
            var ɛ = _ɛ​; // epsilon

            // check for sufficient decrease (qaudratic approximate)
            var curvatureUpperBoundGood = (2*δ - 1)*dφ0 >= dφα;

            // check for sufficient curvature decrease
            var curvatureLowerBoundGood = dφα >= σ*dφ0;

            // and another one for decrease
            var isDecrease = φα <= φ0 + ɛ;

            return curvatureUpperBoundGood && curvatureLowerBoundGood && isDecrease;
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
