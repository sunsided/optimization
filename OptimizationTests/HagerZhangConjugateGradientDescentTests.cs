using System;
using FluentAssertions;
using MathNet.Numerics.LinearAlgebra;
using NUnit.Framework;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.GradientDescent.ConjugateGradients;
using widemeadows.Optimization.Hypotheses;

namespace widemeadows.Optimization.Tests
{
    class HagerZhangConjugateGradientDescentTests
    {
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
    }
}
