using System;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.GradientDescent;

namespace widemeadows.Optimization.Tests.Hypotheses
{
    /// <summary>
    /// Conjugate-Gradient Descent
    /// </summary>
    public sealed class ConjugateGradientDescent : GradientDescentBase<double, ICostGradient<double>>
    {
        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns>IOptimizationResult&lt;TData&gt;.</returns>
        public override IOptimizationResult<double> Minimize(IOptimizationProblem<double, ICostGradient<double>> problem)
        {
            throw new NotImplementedException("Conjugate-Gradient Descent not implemented");
        }
    }
}
