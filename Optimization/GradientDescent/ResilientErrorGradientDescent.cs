using System;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Cost;

namespace widemeadows.Optimization.GradientDescent
{
    /// <summary>
    /// Resilient Error Gradient Descent.
    /// </summary>
    public sealed class ResilientErrorGradientDescent : GradientDescentBase<double, IDifferentiableCostFunction<double>>
    {
        /// <summary>
        /// The initial step width
        /// </summary>
        private double _initialStepSize = 0.1D;

        /// <summary>
        /// The step increase factor
        /// </summary>
        private double _stepIncreaseFactor = 1.2D;

        /// <summary>
        /// The step decrease factor
        /// </summary>
        private double _stepDecreaseFactor = 0.5D;

        /// <summary>
        /// Gets or sets the initial coefficient change step size.
        /// </summary>
        /// <value>The initial size of the step.</value>
        /// <exception cref="NotFiniteNumberException">The value must be finite</exception>
        /// <exception cref="ArgumentOutOfRangeException">The value must be positive</exception>
        public double InitialStepSize
        {
            get { return _initialStepSize; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value)) throw new NotFiniteNumberException("The value must be finite", value);
                if (value <= 0) throw new ArgumentOutOfRangeException("value", value, "The value must be positive");
                _initialStepSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the step increase factor.
        /// </summary>
        /// <value>The step increase factor.</value>
        /// <exception cref="System.NotFiniteNumberException">The value must be finite</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The value must be greater than one</exception>
        public double StepIncreaseFactor
        {
            get { return _stepIncreaseFactor; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value)) throw new NotFiniteNumberException("The value must be finite", value);
                if (value <= 1) throw new ArgumentOutOfRangeException("value", value, "The value must be greater than 1");
                _stepIncreaseFactor = value;
            }
        }

        /// <summary>
        /// Gets or sets the step decrease factor.
        /// </summary>
        /// <value>The step decrease factor.</value>
        /// <exception cref="System.NotFiniteNumberException">The value must be finite</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The value must be in range ]0..1[</exception>
        public double StepDecreaseFactor
        {
            get { return _stepDecreaseFactor; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value)) throw new NotFiniteNumberException("The value must be finite", value);
                if (value <= 0 || value >= 1) throw new ArgumentOutOfRangeException("value", value, "The value must be in range ]0..1[");
                _stepDecreaseFactor = value;
            }
        }

        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        public override IOptimizationResult<double> Minimize(IOptimizationProblem<double, IDifferentiableCostFunction<double>> problem)
        {
            var maxIterations = MaxIterations;
            var increaseFactor = _stepIncreaseFactor;
            var decreaseFactor = _stepDecreaseFactor;
            var initialStepSize = _initialStepSize;
            // ReSharper disable once ExceptionNotDocumented
            var threshold = CostChangeThreshold;

            // obtain the initial cost
            var costFunction = problem.CostFunction;
            var cost = 0D;
            var previousCost = double.MaxValue;

            // obtain the initial coefficients
            var coefficients = problem.GetInitialCoefficients();
            var coefficientCount = coefficients.Count;

            // we need to store that last iteration's gradients
            var previousGradient = Vector<double>.Build.Dense(coefficientCount, Vector<double>.Zero);
            var secondPreviousGradient = Vector<double>.Build.Dense(coefficientCount, Vector<double>.Zero);

            // initialize the step widths
            var stepWidths = Vector<double>.Build.Dense(coefficientCount, initialStepSize);

            // loop over all allowed iterations
            for (var i = 0; i < maxIterations; ++i)
            {
                // obtain the cost
                cost = costFunction.CalculateCost(coefficients);

                // determine the change in cost
                var costChange = cost - previousCost;
                if (Math.Abs(costChange) <= threshold)
                {
                    Debug.WriteLine("Stopping REGD at iteration {0}/{1} because costChange |{2}| <= {3}", i, maxIterations, costChange, threshold);
                    break;
                }

                // determine changes in gradient direction
                var gradient = costFunction.CalculateGradient(coefficients);
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
