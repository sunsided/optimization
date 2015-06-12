using System.Collections.Generic;
using FluentAssertions;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using NUnit.Framework;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.GradientDescent;
using widemeadows.Optimization.Hypotheses;
using widemeadows.Optimization.Tests.Hypotheses;

namespace widemeadows.Optimization.Tests
{
    [TestFixture]
    public class ResilientErrorGradientDescentTests
    {
        [Test]
        public void LinearRegressionWithResidualSumOfSquares()
        {
            // obtain the test data
            var trainingSet = new List<DataPoint<double>>
            {
                new DataPoint<double>(-1, -1.5),
                new DataPoint<double>(0, 0.5),
                new DataPoint<double>(1, 2.5),
                new DataPoint<double>(2, 4.5),
                new DataPoint<double>(3, 6.5)
            };

            // assume a hypothesis
            var hypothesis = new LinearHypothesis(1);

            // cost function is sum of squared errors
            var costFunction = new ResidualSumOfSquaresCostFunction(hypothesis, trainingSet);

            // define the optimization problem
            var problem = new OptimizationProblem<double, IDifferentiableCostFunction<double>>(hypothesis, costFunction);

            // optimize!
            var gd = new ResilientErrorGradientDescent
            {
                CostChangeThreshold = 0.0D
            };
            var result = gd.Minimize(problem);

            // assert!
            var coefficients = result.Coefficients;
            coefficients[0].Should().BeApproximately(0.5, 1E-6D, "because that's the underlying system's intercept");
            coefficients[1].Should().BeApproximately(2, 1E-6D, "because that's the underlying system's slope");
        }

        [Test]
        public void DualLinearRegressionWithResidualSumOfSquares()
        {
            // obtain the test data
            var trainingSet = new List<DataPoint<double>>
            {
                new DataPoint<double>(-1, new [] {-1.5 , -1.0 }),
                new DataPoint<double>(0, new [] {0.5, 1.0}),
                new DataPoint<double>(1, new [] {2.5, 3.0}),
                new DataPoint<double>(2, new [] {4.5, 5.0}),
                new DataPoint<double>(3, new [] {6.5, 6.0})
            };

            // assume a hypothesis
            var hypothesis = new DualLinearHypothesis(1);

            // cost function is sum of squared errors
            var costFunction = new ResidualSumOfSquaresCostFunction(hypothesis, trainingSet);

            // define the optimization problem
            var problem = new OptimizationProblem<double, IDifferentiableCostFunction<double>>(hypothesis, costFunction);

            // optimize!
            var gd = new ResilientErrorGradientDescent
            {
                CostChangeThreshold = 0.0D
            };
            var result = gd.Minimize(problem);

            // assert!
            var coefficients = result.Coefficients;
            coefficients[0].Should().BeApproximately(0.5, 1E-6D, "because that's the underlying system's intercept");
            coefficients[1].Should().BeApproximately(2, 1E-6D, "because that's the underlying system's slope");
        }

        [Test]
        public void RosenbrockRegressionWithResidualSumOfSquares()
        {
            // parameter is default Rosenbrock
            var theta = Vector<double>.Build.Dense(1, 105D);

            // define the hypothesis
            var hypothesis = new RosenbrockHypothesis();

            // define a probability distribution
            var distribution = new ContinuousUniform(-10D, 10D);

            // obtain the test data
            const int dataPoints = 10;
            var trainingSet = new List<DataPoint<double>>(dataPoints);
            for (int i = 0; i < dataPoints; ++i)
            {
                var inputs = Vector<double>.Build.Random(2, distribution);
                var output = hypothesis.Evaluate(theta, inputs);
                trainingSet.Add(new DataPoint<double>(inputs, output));
            };

            // cost function is sum of squared errors
            var costFunction = new ResidualSumOfSquaresCostFunction(hypothesis, trainingSet);

            // define the optimization problem
            var problem = new OptimizationProblem<double, IDifferentiableCostFunction<double>>(hypothesis, costFunction);

            // optimize!
            var gd = new ResilientErrorGradientDescent
            {
                CostChangeThreshold = 0.0D // TODO: actually use it
            };
            var result = gd.Minimize(problem);

            // assert!
            var coefficients = result.Coefficients;
            coefficients[0].Should().BeApproximately(theta[0], 1D, "because that's the underlying system's parameter");
        }
    }
}
