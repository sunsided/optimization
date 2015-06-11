using System;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization.Hypotheses
{
    /// <summary>
    /// A parameterized Rosenbrock-like function of the form <c>(1-x)^2+a*(y^x^2)^2</c>
    /// </summary>
    public sealed class RosenbrockHypothesis : ITwiceDifferentiableHypothesis<double>
    {
        /// <summary>
        /// Gets an initial guess for the coefficients.
        /// </summary>
        /// <returns>Vector&lt;TData&gt;.</returns>
        public Vector<double> GetInitialCoefficients()
        {
            var random = new Random();
            return Vector<double>.Build.Dense(1, random.NextDouble()*200D);
        }

        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="inputs" /> and the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="inputs">The inputs.</param>
        /// <returns>The estimated outputs.</returns>
        public Vector<double> Evaluate(Vector<double> coefficients, Vector<double> inputs)
        {
            Debug.Assert(coefficients.Count == 1, "coefficients.Count == 1");
            Debug.Assert(inputs.Count == 2, "inputs.Count == 2");

            var x = inputs[0];
            var y = inputs[1];
            var theta = coefficients[0];

            var a = (1D - x);
            var b = (y - x*x);

            var value = a*a + theta*b*b;

            // return the value
            return Vector<double>.Build.Dense(1, value);
        }

        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="locations" /> and the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate" />.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientGradient(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs)
        {
            return CoefficientGradient(coefficients, locations);
        }

        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="locations" /> and the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientGradient(Vector<double> coefficients, Vector<double> locations)
        {
            Debug.Assert(coefficients.Count == 1, "coefficients.Count == 1");
            Debug.Assert(locations.Count == 2, "inputs.Count == 2");

            var x = locations[0];
            var y = locations[1];

            // first partial derivative with respect to the only coefficient
            var a = (y - x*x);
            var dtheta = a*a;

            return Vector<double>.Build.Dense(1, dtheta);
        }

        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="locations" /> and the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate" />.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientLaplacian(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs)
        {
            return CoefficientLaplacian(coefficients, locations);
        }

        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="locations" /> and the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientLaplacian(Vector<double> coefficients, Vector<double> locations)
        {
            Debug.Assert(coefficients.Count == 1, "coefficients.Count == 1");
            Debug.Assert(locations.Count == 2, "inputs.Count == 2");

            // second partial derivative with respect to the only coefficient
            return Vector<double>.Build.Dense(1, 0);
        }
    }
}
