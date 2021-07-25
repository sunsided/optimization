using System.Diagnostics.CodeAnalysis;

namespace WideMeadows.Optimization.LineSearch
{
    /// <summary>
    /// Interface IHagerZhangLineSearchParameters
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Parameter names are lower case for symmetry with MATLAB code.")]
    public interface IHagerZhangLineSearchParameters
    {
        /// <summary>
        /// delta, used in the Wolfe conditions
        /// </summary>
        /// <value>The δ.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be in range (0, 0.5).</exception>
        double δ { get; set; }

        /// <summary>
        /// sigma, used in the Wolfe conditions
        /// </summary>
        /// <value>The σ.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">value;Value must be in range (<seealso cref="δ"/>, 1).</exception>
        double σ { get; set; }

        /// <summary>
        /// epsilon, used in the approximate Wolfe termination
        /// </summary>
        /// <value>The ε.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be positive.</exception>
        /// <remarks>Range [0, ∞)</remarks>
        double ε { get; set; }

        /// <summary>
        /// omega, used in switching from Wolfe to approximate Wolfe conditions
        /// </summary>
        /// <value>The ω.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be in range [0, 1].</exception>
        /// <remarks>Range [0, 1]</remarks>
        double ω { get; set; }

        /// <summary>
        /// Delta, decay factor for Qk in the recurrence
        /// </summary>
        /// <value>The δ.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be in range [0, 1].</exception>
        double Δ { get; set; }

        /// <summary>
        /// theta, used in the update rules when the potential intervals [a, c]
        /// or [c, b] violate the opposite slope condition contained in
        /// </summary>
        /// <value>The θ.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be in range (0, 1).</exception>
        double θ { get; set; }

        /// <summary>
        /// gamma, determines when a bisection step is performed
        /// </summary>
        /// <value>The γ.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">value;Value must be in range (0, 1).</exception>
        double γ { get; set; }

        /// <summary>
        /// rho, expansion factor used in the bracket rule
        /// </summary>
        /// <value>The ρ.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be in greater than 1.</exception>
        /// <remarks>Range (1, ∞)</remarks>
        double ρ { get; set; }

        /// <summary>
        /// psi 0, small factor used in starting guess
        /// </summary>
        /// <value>The ψ0.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be in range (0, 1).</exception>
        double ψ0 { get; set; }

        /// <summary>
        /// psi 1, small factor
        /// </summary>
        /// <value>The ψ1.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be in range (0, 1).</exception>
        double ψ1 { get; set; }

        /// <summary>
        /// psi 2, factor multiplying previous step α(k−1)
        /// </summary>
        /// <value>The ψ2.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be in greater than 1.</exception>
        double ψ2 { get; set; }

        /// <summary>
        /// Determines if QuadStep is used.
        /// </summary>
        bool QuadStep { get; set; }

        /// <summary>
        /// alpha 0, the initial alpha value for the first iteration.
        /// </summary>
        /// <value>The α0.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be positive.</exception>
        /// <remarks>Range (0, ∞)</remarks>
        double α0 { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of bracketing iterations.
        /// </summary>
        /// <value>The maximum bracketing iterations.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be positive.</exception>
        int MaxBracketingIterations { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of line search iterations.
        /// </summary>
        /// <value>The maximum iterations.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be positive.</exception>
        int MaxIterations { get; set; }
    }
}
