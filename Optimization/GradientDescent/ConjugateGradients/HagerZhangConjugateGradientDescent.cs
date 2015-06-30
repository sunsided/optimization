namespace widemeadows.Optimization.GradientDescent.ConjugateGradients
{
    /// <summary>
    /// Implements Hager-Zhang Conjugate Gradient Descent (CG_DESCENT)
    /// </summary>
    class HagerZhangConjugateGradientDescent
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
        private double _ϵ = 1E-6D;

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
    }
}
