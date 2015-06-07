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
        /// The maximum number of iterations
        /// </summary>
        private readonly int _maxIterations = 400;

        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        public IOptimizationResult<double> Minimize(IOptimizationProblem<double, ICostGradient<double>> problem)
        {
            var maxIterations = _maxIterations;
            
            // obtain the initial cost
            var costFunction = problem.CostFunction;
            var previousCost = double.MaxValue;

            // obtain the initial coefficients
            var hypothesis = problem.Hypothesis;
            var coefficients = hypothesis.GetInitialCoefficients();

            // loop over all allowed iterations
            for (var i = 0; i < maxIterations; ++i)
            {
                // obtain the cost and its gradient
                var costResult = costFunction.CalculateCost(coefficients);

                // determine the change in cost
                var costChange = costResult.Cost - previousCost;
            }

            throw new NotImplementedException("resilient error gradient descent not implemented");
        }
    }
}
