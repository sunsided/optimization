using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;

namespace WideMeadows.Optimization.Hypotheses
{
    /// <summary>
    /// A parameterized Rosenbrock-like function of the form <c>(1-x)^2+a*(y^x^2)^2</c>
    /// </summary>
    public sealed class RosenbrockHypothesis : ITwiceDifferentiableHypothesis<double>
    {
        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="inputs" /> and the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="inputs">The inputs.</param>
        /// <returns>The estimated outputs.</returns>
        public Vector<double> Evaluate(Vector<double> coefficients, Vector<double> inputs)
        {
            Debug.Assert(coefficients.Count == 2, "coefficients.Count == 2");
            Debug.Assert(inputs.Count == 2, "inputs.Count == 2");

            var x = inputs[0];
            var y = inputs[1];
            var a = coefficients[0];
            var b = coefficients[1];

            var term1 = a - x;
            var term2 = y - x*x;

            var value = term1*term1 + b*term2*term2;

            // return the value
            return Vector<double>.Build.Dense(1, value);
        }

        #region Partial derivatives with respect to the inputs

        /// <summary>
        /// Determines the gradient of the hypothesis with respect to the <paramref name="locations" /> given the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="locations" />.</returns>
        public Vector<double> Jacobian(Vector<double> coefficients, Vector<double> locations)
        {
            Debug.Assert(coefficients.Count == 2, "coefficients.Count == 2");
            Debug.Assert(locations.Count == 2, "inputs.Count == 2");

            var x = locations[0];
            var y = locations[1];
            var a = coefficients[0];
            var b = coefficients[1];

            var gradient = Vector<double>.Build.Dense(2);

            // first partial derivative with respect to the first input
            gradient[0] = -2*a + 4*b*x*x*x - 4*b*x*y + 2*x;

            // first partial derivative with respect to the second input
            gradient[1] = 2 * b * (y - x * x);

            return gradient;
        }

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="locations" /> given the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="locations" />.</returns>
        public Vector<double> Hessian(Vector<double> coefficients, Vector<double> locations)
        {
            Debug.Assert(coefficients.Count == 2, "coefficients.Count == 2");
            Debug.Assert(locations.Count == 2, "inputs.Count == 2");

            var x = locations[0];
            var y = locations[1];
            var a = coefficients[0];
            var b = coefficients[1];

            var unmixedHessian = Vector<double>.Build.Dense(2);

            // second partial derivative with respect to the first input
            unmixedHessian[0] = 12*b*x*x - 4*b*y + 2;

            // second partial derivative with respect to the second input
            unmixedHessian[1] = 2 * b;

            return unmixedHessian;
        }

        /// <summary>
        /// Determines the gradient of the hypothesis with respect to the <paramref name="locations" /> given the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate" />.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="locations" />.</returns>
        public Vector<double> Jacobian(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs) => Jacobian(coefficients, locations);

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="locations" /> given the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate" />.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="locations" />.</returns>
        public Vector<double> Hessian(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs) => Hessian(coefficients, locations);

        #endregion Partial derivatives with respect to the inputs

        #region Partial derivatives with respect to the coefficients

        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="locations" /> and the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientJacobian(Vector<double> coefficients, Vector<double> locations)
        {
            Debug.Assert(coefficients.Count == 2, "coefficients.Count == 2");
            Debug.Assert(locations.Count == 2, "inputs.Count == 2");

            var x = locations[0];
            var y = locations[1];
            var a = coefficients[0];
            var b = coefficients[1];

            var gradient = Vector<double>.Build.Dense(2);

            // first partial derivative with respect to the first coefficient
            gradient[0] = 2*(a-x);

            // first partial derivative with respect to the second coefficient
            gradient[1] = (y - x*x)*(y - x*x);

            return gradient;
        }

        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="locations" /> and the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientHessian(Vector<double> coefficients, Vector<double> locations)
        {
            Debug.Assert(coefficients.Count == 2, "coefficients.Count == 2");
            Debug.Assert(locations.Count == 2, "inputs.Count == 2");

            var x = locations[0];
            var y = locations[1];
            var a = coefficients[0];
            var b = coefficients[1];

            var unmixedHessian = Vector<double>.Build.Dense(2);

            // second partial derivative with respect to the first coefficient
            unmixedHessian[0] = 2;

            // second partial derivative with respect to the second coefficient
            unmixedHessian[1] = 0;

            return unmixedHessian;
        }

        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="locations" /> and the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate" />.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientJacobian(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs) => CoefficientJacobian(coefficients, locations);

        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="locations" /> and the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate" />.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientHessian(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs) => CoefficientHessian(coefficients, locations);

        #endregion Partial derivatives with respect to the coefficients
    }
}
