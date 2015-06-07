using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.Cost.SSE;
using widemeadows.Optimization.GradientDescent;
using widemeadows.Optimization.Hypotheses;

namespace widemeadows.Optimization.Tests
{
    [TestFixture]
    public class ResilientErrorGradientDescentTests
    {
        [Test]
        public void ResidualSumOfSquares()
        {
            // obtain the test data
            var trainingSet = new List<DataPoint<double>>
            {
                new DataPoint<double>(-1, -2),
                new DataPoint<double>(0, 0),
                new DataPoint<double>(1, 2),
                new DataPoint<double>(2, 4),
                new DataPoint<double>(3, 6)
            };

            // assume a hypothesis
            var hypothesis = new LinearHypothesis(1);

            // cost function is sum of squared errors
            var costFunction = new ResidualSumOfSquaresCostFunction(hypothesis, trainingSet);

            // define the optimization problem
            var problem = new OptimizationProblem<double, ICostGradient<double>>(hypothesis, costFunction);

            // optimize!
            var gd = new ResilientErrorGradientDescent
            {
                GradientChangeThreshold = 0D
            };
            var result = gd.Minimize(problem);

            // assert!
            var coefficients = result.Coefficients;
            coefficients[0].Should().BeApproximately(0, 1E-6D, "because that's the underlying system's intercept");
            coefficients[1].Should().BeApproximately(2, 1E-6D, "because that's the underlying system's slope");
        }
    }
}
