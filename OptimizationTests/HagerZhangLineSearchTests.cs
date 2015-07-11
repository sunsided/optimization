using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.Hypotheses;
using widemeadows.Optimization.LineSearch;

namespace widemeadows.Optimization.Tests
{
    /// <summary>
    /// Class HagerZhangLineSearchTests.
    /// </summary>
    [TestFixture]
    public class HagerZhangLineSearchTests
    {
        /// <summary>
        /// Alphas the of minimizer is found.
        /// </summary>
        [Test]
        public void AlphaOfMinimizerIsFound()
        {
            var theta = Vector<double>.Build.DenseOfArray(new[] {1.0D, 100.0D});
            var x0 = Vector<double>.Build.DenseOfArray(new[] {-1.5D, 0.6D});
            var rosenbrock = new RosenbrockHypothesis();

            // evaluate at the point
            var value = rosenbrock.Evaluate(theta, x0);
            var gradient = rosenbrock.Jacobian(theta, x0);

            // determine the initial search direction
            var direction = -gradient.Normalize(2);

            // create a wrapper cost function
            var wrapper = new FunctionValueOptimization<double>(rosenbrock, theta);

            // perform a line search
            var lineSearch = new HagerZhangLineSearch();
            var alpha = lineSearch.Minimize(wrapper, x0, direction, 0.0D);

            alpha.Should().BeApproximately(0.235552763819095D, 1E-5D, "because that is the alpha value of the minimum along the search direction");
        }
    }
}
