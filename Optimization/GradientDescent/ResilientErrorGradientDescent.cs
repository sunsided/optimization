using System;
using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization.GradientDescent
{
    /// <summary>
    /// Resilient Error Gradient Descent.
    /// </summary>
    public class ResilientErrorGradientDescent : IMinimization<double, ICostGradient<double>> 
    {
        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        public IOptimizationResult<double> Minimize(IOptimizationProblem<double, ICostGradient<double>> problem)
        {
            throw new NotImplementedException("resilient error gradient descent not implemented");
        }
    }
}
