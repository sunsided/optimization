using System;
using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization.GradientDescent
{
    /// <summary>
    /// Conjugate-Gradient Descent
    /// </summary>
    public sealed class ConjugateGradientDescent : GradientDescentBase<double, ITwiceDifferentiableCostFunction<double>>
    {
        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns>IOptimizationResult&lt;TData&gt;.</returns>
        public override IOptimizationResult<double> Minimize(IOptimizationProblem<double, ITwiceDifferentiableCostFunction<double>> problem)
        {
            throw new NotImplementedException("Conjugate-Gradient Descent not implemented");
        }
    }
}
