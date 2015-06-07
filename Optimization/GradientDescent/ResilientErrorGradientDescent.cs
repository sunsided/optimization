using System;
using MathNet.Numerics.LinearAlgebra;
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
        /// The initial step width
        /// </summary>
        private readonly double _initialStepSize = 0.1D;

        /// <summary>
        /// The step increase factor
        /// </summary>
        private readonly double _stepIncreaseFactor = 1.2D;

        /// <summary>
        /// The step decrease factor
        /// </summary>
        private readonly double _stepDecreaseFactor = 0.5D;

        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        public IOptimizationResult<double> Minimize(IOptimizationProblem<double, ICostGradient<double>> problem)
        {
            var maxIterations = _maxIterations;
            var increaseFactor = _stepIncreaseFactor;
            var decreaseFactor = _stepDecreaseFactor;
            var initialStepSize = _initialStepSize;
            
            // obtain the initial cost
            var costFunction = problem.CostFunction;
            var cost = 0D;
            var previousCost = double.MaxValue;

            // obtain the initial coefficients
            var hypothesis = problem.Hypothesis;
            var coefficients = hypothesis.GetInitialCoefficients();
            var coefficientCount = coefficients.Count;

            // we need to store that last iteration's gradients
            var previousGradient = Vector<double>.Build.Dense(coefficientCount, Vector<double>.Zero);
            var secondPreviousGradient = Vector<double>.Build.Dense(coefficientCount, Vector<double>.Zero);

            // initialize the step widths
            var stepWidths = Vector<double>.Build.Dense(coefficientCount, initialStepSize);

            // loop over all allowed iterations
            for (var i = 0; i < maxIterations; ++i)
            {
                // obtain the cost and its gradient
                var costResult = costFunction.CalculateCost(coefficients);

                // determine the change in cost
                cost = costResult.Cost;
                var costChange = cost - previousCost;
                
                // determine changes in gradient direction
                var gradient = costResult.CostGradient;
                var gradientDirectionIndicator = gradient.PointwiseMultiply(previousGradient);
                var previousGradientDirectionIndicator = previousGradient.PointwiseMultiply(secondPreviousGradient);

                // update step sizes for each individual coefficient
                for (var p = 0; p < coefficientCount; ++p)
                {
                    var gradientInOppositeDirection = gradientDirectionIndicator[p] < 0;
                    var gradientInSameDirection = gradientDirectionIndicator[p] > 0;
                    var previousGradientInSameDirection = previousGradientDirectionIndicator[p] >= 0;
                    if (gradientInOppositeDirection)
                    {
                        // decrease step size for this coefficient
                        stepWidths[p] *= decreaseFactor;
                    }
                    else if (gradientInSameDirection && previousGradientInSameDirection) // TODO check that condition
                    {
                        // increase step size for this coefficient
                        stepWidths[p] *= increaseFactor;
                    }
                    else
                    {
                        // keep step size for this coefficient
                        stepWidths[p] *= 1.0D;
                    }
                }

                // determine the coefficient delta to apply
                var delta = gradient.MapIndexed((index, g) => Math.Sign(g)*stepWidths[index]);
                coefficients -= delta;

                // store values for the next iteration
                previousCost = cost;
                secondPreviousGradient = previousGradient;
                previousGradient = gradient;
            }

            return new OptimizationResult<double>(cost, coefficients);
        }
    }
}
