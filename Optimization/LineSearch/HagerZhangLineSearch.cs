using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;
// ReSharper disable InconsistentNaming

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
        /// The maximum number of bracketing iterations in <seealso cref="BracketStartingPoint"/>.
        /// </summary>
        private int _maxBracketingIterations = Int32.MaxValue;

        /// <summary>
        /// The maximum number of line search iterations.
        /// </summary>
        private int _maxIterations = Int32.MaxValue;

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

            // prefetch
            var γ = _γ;

            // convenience function for the evaluation
            var values = GetFunctionValues(function, location, direction);

            // find a starting point and check if that solution is already good enough
            var c = DetermineInitialSearchPoint(previousStepWidth, location, ref values);
            if (ShouldTerminate(c, ref values)) return c;

            // bracket the initial starting point
            var bracket = BracketStartingPoint(c, values);
            if (ShouldTerminate(bracket.Start, ref values)) return bracket.Start;
            if (ShouldTerminate(bracket.End, ref values)) return bracket.End;

            // iterate along the line
            var maxIterations = _maxIterations;
            for (var i = 0; i < maxIterations; ++i)
            {
                // L1: perform a double secant step to optimize the search interval
                var candidate = DoubleSecant(bracket, ref values);
                if (ShouldTerminate(candidate.Start, ref values)) return candidate.Start;
                if (ShouldTerminate(candidate.End, ref values)) return candidate.End;

                // L2
                if ((candidate.End - candidate.Start) > γ*(bracket.End - bracket.Start))
                {
                    // find a new midpoint
                    c = (candidate.Start + candidate.End)/2;
                    if (ShouldTerminate(c, ref values)) return c;

                    // update the bracketing interval
                    candidate = UpdateBracketingInterval(candidate, c, ref values);
                }

                // L3: wrap around
                bracket = candidate;
            }

            throw new NotImplementedException("aww yeah");
        }

        /// <summary>
        /// Updates the <paramref name="current" /> bracketing interval around the midpoint <paramref name="c" />.
        /// </summary>
        /// <param name="current">The current bracketing interval.</param>
        /// <param name="c">The midpoint.</param>
        /// <param name="values">The values.</param>
        /// <returns>Bracket.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private Bracket UpdateBracketingInterval(Bracket current, double c, ref FunctionValues values)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Perform a double secant step on the <paramref name="bracket"/> interval.
        /// </summary>
        /// <param name="bracket">The search interval.</param>
        /// <param name="values">The values.</param>
        /// <returns>Bracket.</returns>
        private Bracket DoubleSecant(Bracket bracket, ref FunctionValues values)
        {
            throw new NotImplementedException("aww yeah");
        }

        /// <summary>
        /// Brackets the starting point <paramref name="α"/>.
        /// </summary>
        /// <param name="α">The starting point.</param>
        /// <param name="values">The values.</param>
        /// <returns>Bracket.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private Bracket BracketStartingPoint(double α, FunctionValues values)
        {
            // prefetch
            var ρ = _ρ; // rho
            var θ = _θ; // theta
            var ɛ = _ɛ; // epsilon

            // make sure the parameters are in range
            Debug.Assert(ρ > 1, "ρ > 1");
            Debug.Assert(θ > 0 && θ < 1, "θ > 0 && θ < 1");

            // B0: initialize the current step value cj and keep track of the values in a stack
            var c = α;
            var previousC = new Stack<double>();
            var φ0 = values.φ0 + ɛ;

            // TODO: set a maximum iteration count
            var maxBracketingIterations = _maxBracketingIterations;
            for (var j = 0; j < maxBracketingIterations; ++j)
            {
                // register the current step value
                previousC.Push(c);

                // determine the gradient at the current step length
                var dφc = values.dφ(c);

                // B1: if we find our currently selected end value to be ascending,
                // then backtrack the starting value to the last descend.
                // Since by definition, the first step is always a descent direction,
                // this will never happen in the first loop iteration.
                if (dφc >= 0)
                {
                    var end = c;

                    // find the most recent step selection c smaller cj that resulted
                    // in a function value less than the starting point.
                    // Since we just added cj, we'll just throw that away.
                    previousC.Pop();
                    while (previousC.Count > 0)
                    {
                        c = previousC.Pop();
                        if (values.φ(c) > φ0) continue;

                        var start = c;
                        return new Bracket(start, end);
                    }

                    // at this point there was no good starting point.
                    // sadly, this point is not handled in the paper, so
                    // we'll just throw out 0 as the starting point and hope for the best.
                    Debug.WriteLine("Unable to obtain a good starting point while backtracking the bracketing.");
                    return new Bracket(0, end);
                }

                // B2: If, for some reason, we are on a descending direction, yet the
                // current function value is larger than what we started with, then
                // we know a minimum must exist between the current point and the start.
                // By using the secant method, we zoom in the range until we find a valid
                // search region.
                if (values.φ(c) > φ0)
                {
                    var current_start = 0.0D;
                    var current_end = c;

                    while (true) // TODO: bug in disguise?
                    {
                        // U3a
                        var d = (1 - θ)*current_start + θ*current_end;
                        if (values.dφ(d) >= 0.0D)
                        {
                            var start = current_start;
                            var end = d;
                            return new Bracket(start, end);
                        }

                        // U3b: close in from the left
                        if (values.φ(d) <= φ0)
                        {
                            current_start = d;
                            continue;
                        }

                        // U3c: close in from the right
                        current_end = d;
                    }
                }

                // B3: At this point, we are still descending and we did not skip
                // any minima as far as we know, so we'll increase our step size.
                c = ρ*c;
            }

            return new Bracket();
        }

        /// <summary>
        /// Convenience function to obtain often-used function values.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="location">The location.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>FunctionValues.</returns>
        private static FunctionValues GetFunctionValues(IDifferentiableCostFunction<double> function, Vector<double> location, Vector<double> direction)
        {
            // define the functions
            var φ = Getφ(function, location, direction);
            var dφ = GetDφ(function, location, direction);

            // determine the starting values
            var φ0 = φ(0);
            var dφ0 = dφ(0);
            var Δf0 = function.Jacobian(location);

            // bundle the helper
            var values = new FunctionValues(
                φ: φ, dφ: dφ,
                φ0: φ0, dφ0: dφ0,
                Δf0: Δf0);
            return values;
        }

        /// <summary>
        /// Determines the initial search point.
        /// </summary>
        /// <param name="αprev">The previous α value.</param>
        /// <param name="location">The location.</param>
        /// <param name="values">The function values.</param>
        /// <returns>System.Double.</returns>
        private double DetermineInitialSearchPoint(double αprev, [NotNull] Vector<double> location, ref FunctionValues values)
        {
            // prefetch
            var ψ0 = _ψ0;
            var ψ1 = _ψ1;
            var ψ2 = _ψ2;
            var α0 = _α0;
            var useQuadStep = _quadStepEnabled;

            // for clarity and caching
            var φ = values.φ;
            var φ0 = values.φ0;
            var dφ0 = values.dφ0;
            var f0 = values.f0;
            var Δf0 = values.Δf0;

            // check if this is the first iteration
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var isFirstIteration = αprev == 0.0D;
            if (isFirstIteration)
            {
                // if there is a user-defined starting value, then we'll use it.
                if (α0.IsFinite()) return α0;

                // if the starting point is nonzero, calculate a better α
                var supremumNormOfLocation = location.AbsoluteMaximum();
                if (supremumNormOfLocation > 0.0D)
                {
                    var supremumNormOfGradientAtLocation = Δf0.AbsoluteMaximum();
                    return ψ0*supremumNormOfLocation/supremumNormOfGradientAtLocation;
                }

                // if the function value is nonzero, calculate the next better α
                var absoluteOfValueAtLocation = Math.Abs(f0);
                if (absoluteOfValueAtLocation > 0.0D)
                {
                    var squaredEuclideanNormOfGradientAtLocation = Δf0*Δf0;
                    return ψ0*absoluteOfValueAtLocation/squaredEuclideanNormOfGradientAtLocation;
                }

                // in any other case, use α = 1
                return 1.0D;
            }

            // check if the user wishes to use quadstep, then check if quadstep may be used
            if (useQuadStep)
            {
                // the key idea here is to find a quadratic approximation q(α) = aα²+bα+c that
                // fulfills the condition q(0)=φ(0), q'(0)=φ'(0) and q(ψ1αprev)=φ(ψ1*αprev).

                // only if the function value at the new position r=ψ1*αprev is actually
                // smaller than the value at the starting position, we'll attempt to
                // interpolate through the starting position and the value at r.
                var r = ψ1*αprev;
                var φr = φ(r);
                if (φr <= φ0)
                {
                    var d = r*r;
                    var a = -(φ0 - φr + φr*dφ0)/d;

                    // find the minimizer
                    var rmin = 0.5D*(d*dφ0)/(φ0 - φr + r*dφ0);

                    // in order to have a valid minimizer here, the function must be concave.
                    // This is only the case if a is positve.
                    // We do not need to check the second derivative, since the second
                    // derivative of a concave quadratic function is positive at every point.
                    if (a > 0.0D)
                    {
                        return rmin;
                    }
                }
            }

            // in any other case, simply use a smaller step width than before
            return ψ2*αprev;
        }

        /// <summary>
        /// Determines if the algorithm should terminate, given the currently selected step width <paramref name="α" />
        /// and the shared <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="α">The selected step width.</param>
        /// <returns><see langword="true" /> if the line search should terminate, <see langword="false" /> otherwise.</returns>
        private bool ShouldTerminate(double α, ref FunctionValues values)
        {
            // calculate the function values at α
            var φα = values.φ(α);
            var dφα = values.dφ(α);

            // delegate
            return ShouldTerminate(α, values.φ0, values.dφ0, φα, dφα);
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
        /// Some function values for passing around
        /// </summary>
        private struct FunctionValues
        {
            /// <summary>
            /// The function <c>φ(α) = f(x0+α*direction)</c>
            /// </summary>
            public readonly Func<double, double> φ;

            /// <summary>
            /// The directional derivative function <c>φ'(α) = Δf(x0)'*direction</c>
            /// </summary>
            public readonly Func<double, double> dφ;

            /// <summary>
            /// The function value <c>φ(0)</c>
            /// </summary>
            public readonly double φ0;

            /// <summary>
            /// The directional derivative function value <c>φ'(0)</c>
            /// </summary>
            public readonly double dφ0;

            /// <summary>
            /// The function value <c>f(x0)</c>
            /// </summary>
            public double f0 { get { return φ0; } }

            /// <summary>
            /// The gradient <c>Δf(x0)</c>
            /// </summary>
            public readonly Vector<double> Δf0;

            /// <summary>
            /// Initializes a new instance of the <see cref="FunctionValues" /> struct.
            /// </summary>
            /// <param name="φ">The φ.</param>
            /// <param name="dφ">The dφ.</param>
            /// <param name="φ0">The φ0.</param>
            /// <param name="dφ0">The DΦ0.</param>
            /// <param name="Δf0">The ΔF0.</param>
            public FunctionValues(Func<double, double> φ, Func<double, double> dφ, double φ0, double dφ0, [NotNull] Vector<double> Δf0)
            {
                this.φ = φ;
                this.dφ = dφ;
                this.φ0 = φ0;
                this.dφ0 = dφ0;
                this.Δf0 = Δf0;
            }
        }

        /// <summary>
        /// Struct Bracket
        /// </summary>
        private struct Bracket
        {
            /// <summary>
            /// The starting value <c>a</c>
            /// </summary>
            public readonly double Start;

            /// <summary>
            /// The end value <c>b</c>
            /// </summary>
            public readonly double End;

            /// <summary>
            /// Initializes a new instance of the <see cref="Bracket"/> struct.
            /// </summary>
            /// <param name="start">The starting value.</param>
            /// <param name="end">The end value.</param>
            public Bracket(double start, double end)
            {
                Start = start;
                End = end;
            }
        }
    }
}
