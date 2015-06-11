using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Hypotheses;

namespace widemeadows.Optimization.Cost
{
    /// <summary>
    /// Class ResidualSumOfSquaresCostFunction.
    /// </summary>
    public class ResidualSumOfSquaresCostFunction : ITwiceDifferentiableCostFunction<double>
    {
        /// <summary>
        /// The hypothesis to optimize
        /// </summary>
        [NotNull]
        private readonly IDifferentiableHypothesis<double> _hypothesis;

        /// <summary>
        /// The training set
        /// </summary>
        [NotNull]
        private readonly IReadOnlyCollection<DataPoint<double>> _trainingSet;

        /// <summary>
        /// Gets the training set.
        /// </summary>
        /// <value>The training set.</value>
        [NotNull]
        public IReadOnlyCollection<DataPoint<double>> TrainingSet
        {
            get { return _trainingSet; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResidualSumOfSquaresCostFunction" /> class.
        /// </summary>
        /// <param name="hypothesis">The hypothesis.</param>
        /// <param name="trainingSet">The training set.</param>
        public ResidualSumOfSquaresCostFunction([NotNull] IDifferentiableHypothesis<double> hypothesis, [NotNull] IReadOnlyCollection<DataPoint<double>> trainingSet)
        {
            _hypothesis = hypothesis;
            _trainingSet = trainingSet;
        }

        /// <summary>
        /// Calculates the cost.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <returns>ResidualSumOfSquaresCost.</returns>
        public double CalculateCost(Vector<double> coefficients)
        {
            var hypothesis = _hypothesis;
            var trainingSet = _trainingSet;
            var sumOfSquaredErrors = (
                from dataPoint in trainingSet
                let inputs = dataPoint.Inputs
                let expectedOutputs = dataPoint.Outputs
                let outputs = hypothesis.Evaluate(coefficients, inputs)
                select (outputs - expectedOutputs) into error
                select error.Map(v => v*v).Sum()
                ).Sum();

            // scale by the number of training examples
            return sumOfSquaredErrors / trainingSet.Count;
        }

        /// <summary>
        /// Calculates the derivative.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <returns>MathNet.Numerics.LinearAlgebra.Vector&lt;System.Double&gt;.</returns>
        public Vector<double> CalculateGradient(Vector<double> coefficients)
        {
            var hypothesis = _hypothesis;
            var trainingSet = _trainingSet;
            var totalGradient = Vector<double>.Build.Dense(coefficients.Count, Vector<double>.Zero);

            foreach (var dataPoint in trainingSet)
            {
                var inputs = dataPoint.Inputs;
                var expectedOutputs = dataPoint.Outputs;

                // evaluate the hypothesis
                var outputs = hypothesis.Evaluate(coefficients, inputs);

                // calculate the sum of the squared differences
                var error = (outputs - expectedOutputs);

                // calculate the derivate of the hypothesis
                var derivatives = hypothesis.Gradient(coefficients, inputs, outputs);

                // calculate the gradient
                totalGradient += error.OuterProduct(derivatives).Row(0);
            }

            // scale by the number of training examples
            totalGradient /= trainingSet.Count;

            // done.
            return totalGradient;
        }

        /// <summary>
        /// Calculates the derivative.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <returns>MathNet.Numerics.LinearAlgebra.Vector&lt;System.Double&gt;.</returns>
        public Vector<double> CalculateLaplacian(Vector<double> coefficients)
        {
            var hypothesis = _hypothesis;
            var trainingSet = _trainingSet;
            var totalGradient = Vector<double>.Build.Dense(coefficients.Count, Vector<double>.Zero);
            var totalLaplacian = Vector<double>.Build.Dense(coefficients.Count, Vector<double>.Zero);

            foreach (var dataPoint in trainingSet)
            {
                var inputs = dataPoint.Inputs;
                var expectedOutputs = dataPoint.Outputs;

                // evaluate the hypothesis
                var outputs = hypothesis.Evaluate(coefficients, inputs);

                // calculate the sum of the squared differences
                var error = (outputs - expectedOutputs);

                // calculate the derivate of the hypothesis
                var derivatives = hypothesis.Gradient(coefficients, inputs, outputs);

                // calculate the  second derivate of the hypothesis
                var secondDerivatives = hypothesis.Gradient(coefficients, inputs, outputs);

                // calculate the gradient
                totalGradient += error.OuterProduct(derivatives).Row(0);
            }

            // scale by the number of training examples
            var gradient = totalGradient / trainingSet.Count;
            var laplacian = totalLaplacian / trainingSet.Count;

            throw new NotImplementedException("Laplacian of Residual Sum of Squares cost function not implemented");

            // done.
            return laplacian;
        }
    }
}
