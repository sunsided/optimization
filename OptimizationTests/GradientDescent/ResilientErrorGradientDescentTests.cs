using System;
using System.Collections.Generic;
using FluentAssertions;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using NUnit.Framework;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.GradientDescent.Regular;
using widemeadows.Optimization.Hypotheses;
using widemeadows.Optimization.Tests.Hypotheses;

namespace widemeadows.Optimization.Tests.GradientDescent
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
            var initialCoefficients = Vector<double>.Build.Random(2);

            // cost function is sum of squared errors
            var costFunction = new ResidualSumOfSquaresCostFunction(hypothesis, trainingSet);

            // define the optimization problem
            var problem = new OptimizationProblem<double, IDifferentiableCostFunction<double>>(costFunction, initialCoefficients);

            // optimize!
            var gd = new ResilientErrorGD
            {
                ErrorTolerance = 0.0D
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
            var initialCoefficients = Vector<double>.Build.Random(2);

            // cost function is sum of squared errors
            var costFunction = new ResidualSumOfSquaresCostFunction(hypothesis, trainingSet);

            // define the optimization problem
            var problem = new OptimizationProblem<double, IDifferentiableCostFunction<double>>(costFunction, initialCoefficients);

            // optimize!
            var gd = new ResilientErrorGD
            {
                ErrorTolerance = 0.0D
            };
            var result = gd.Minimize(problem);

            // assert!
            var coefficients = result.Coefficients;
            coefficients[0].Should().BeApproximately(0.5, 1E-6D, "because that's the underlying system's intercept");
            coefficients[1].Should().BeApproximately(2, 1E-6D, "because that's the underlying system's slope");
        }

        [Test]
        public void RosenbrockParameterFitWithResidualSumOfSquares()
        {
            // parameter is default Rosenbrock
            var realTheta = Vector<double>.Build.DenseOfArray(new []{1D, 105D});
            var initialTheta = Vector<double>.Build.DenseOfArray(new []{2D, 200D});

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
                var output = hypothesis.Evaluate(realTheta, inputs);
                trainingSet.Add(new DataPoint<double>(inputs, output));
            };

            // cost function is sum of squared errors
            var costFunction = new ResidualSumOfSquaresCostFunction(hypothesis, trainingSet);

            // define the optimization problem
            var problem = new OptimizationProblem<double, IDifferentiableCostFunction<double>>(costFunction, initialTheta);

            // optimize!
            var gd = new ResilientErrorGD
            {
                ErrorTolerance = 0.0D // TODO: actually use it
            };
            var result = gd.Minimize(problem);

            // assert!
            var coefficients = result.Coefficients;
            coefficients[0].Should().BeApproximately(realTheta[0], 1D, "because that's the functions [a] parameter");
            coefficients[1].Should().BeApproximately(realTheta[1], 1D, "because that's the functions [b] parameter");
        }

        [Test]
        public void Rosenbrock()
        {
            // starting point for search is somewhere
            var initialTheta = Vector<double>.Build.DenseOfArray(new[] { -1D, 1D });

            // define the hypothesis with default parameter
            var rosenbrockParameter = Vector<double>.Build.DenseOfArray(new[] { 1D, 100D });
            var hypothesis = new RosenbrockHypothesis();

            // cost function is sum of squared errors
            var costFunction = new FunctionValueOptimization<double>(hypothesis, rosenbrockParameter);

            // define the optimization problem
            var problem = new OptimizationProblem<double, IDifferentiableCostFunction<double>>(costFunction, initialTheta);

            // optimize!
            var gd = new ResilientErrorGD
            {
                MaxIterations = 50000,
                ErrorTolerance = 1E-20D
            };
            var result = gd.Minimize(problem);

            // assert!
            var coefficients = result.Coefficients;
            coefficients[0].Should().BeApproximately(rosenbrockParameter[0], 1E-5D, "because the Rosenbrock function has a minimum at x={0}, y={1}", rosenbrockParameter[0], Math.Sqrt(rosenbrockParameter[0]));
            coefficients[1].Should().BeApproximately(Math.Sqrt(rosenbrockParameter[0]), 1E-5D, "because the Rosenbrock function has a minimum at x={0}, y={1}", rosenbrockParameter[0], Math.Sqrt(rosenbrockParameter[0]));
        }

        [Test, Explicit]
        public void UnivariateExponentialRegressionWithResidualSumOfSquares()
        {
            var theta = Vector<double>.Build.DenseOfArray(new[] { 0D, 13500D, -1.7D });
            var initialTheta = Vector<double>.Build.DenseOfArray(new[] { 0D, 10000D, -1D });

            // define the hypothesis
            var hypothesis = new UnivariateExponentialHypothesis();

            // define a probability distribution
            var distribution = new ContinuousUniform(0D, 1000D);

            // obtain the test data
            const int dataPoints = 100;
            var trainingSet = new List<DataPoint<double>>(dataPoints);
            for (int i = 0; i < dataPoints; ++i)
            {
                var inputs = Vector<double>.Build.Random(1, distribution);
                var output = hypothesis.Evaluate(theta, inputs);
                trainingSet.Add(new DataPoint<double>(inputs, output));
            };

            // cost function is sum of squared errors
            var costFunction = new ResidualSumOfSquaresCostFunction(hypothesis, trainingSet);

            // define the optimization problem
            var problem = new OptimizationProblem<double, IDifferentiableCostFunction<double>>(costFunction, initialTheta);

            // optimize!
            var gd = new ResilientErrorGD();
            var result = gd.Minimize(problem);

            // assert!
            var coefficients = result.Coefficients;
            coefficients[1].Should().BeApproximately(theta[1], 1000D, "because that's the underlying system's [a] parameter");
            coefficients[2].Should().BeApproximately(theta[2], 1E-2D, "because that's the underlying system's [b] parameter");
            coefficients[0].Should().BeApproximately(theta[0], 1E-5D, "because that's the underlying system's offset");
        }
    }
}
