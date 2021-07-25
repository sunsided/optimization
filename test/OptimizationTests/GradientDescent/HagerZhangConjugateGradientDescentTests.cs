using System;
using System.Collections.Generic;
using FluentAssertions;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using WideMeadows.Optimization;
using WideMeadows.Optimization.Cost;
using WideMeadows.Optimization.GradientDescent.Conjugate;
using WideMeadows.Optimization.Hypotheses;
using Xunit;

namespace OptimizationTests.GradientDescent
{
    public sealed class HagerZhangConjugateGradientDescentTests
    {
        [Fact]
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
            var gd = new HagerZhangCG()
            {
                MaxIterations = 10000,
                ErrorTolerance = 1E-8D
            };
            var result = gd.Minimize(problem);

            // assert!
            var coefficients = result.Coefficients;
            coefficients[0].Should().BeApproximately(rosenbrockParameter[0], 1E-5D, "because the Rosenbrock function has a minimum at x={0}, y={1}", rosenbrockParameter[0], Math.Sqrt(rosenbrockParameter[0]));
            coefficients[1].Should().BeApproximately(Math.Sqrt(rosenbrockParameter[0]), 1E-5D, "because the Rosenbrock function has a minimum at x={0}, y={1}", rosenbrockParameter[0], Math.Sqrt(rosenbrockParameter[0]));
        }

        [Fact(Skip = "Optimization may yield unexpected results"), Trait("Category", "explicit")]
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
            for (var i = 0; i < dataPoints; ++i)
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
            var gd = new HagerZhangCG();
            var result = gd.Minimize(problem);

            // assert!
            var coefficients = result.Coefficients;
            coefficients[1].Should().BeApproximately(theta[1], 1000D, "because that's the underlying system's [a] parameter");
            coefficients[2].Should().BeApproximately(theta[2], 1E-2D, "because that's the underlying system's [b] parameter");
            coefficients[0].Should().BeApproximately(theta[0], 1E-5D, "because that's the underlying system's offset");
        }
    }
}
