using System;
using System.Diagnostics;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization.Hypotheses
{
    /// <summary>
    /// Class UnivariateExponentialHypothesis. This class cannot be inherited.
    /// </summary>
    public sealed class UnivariateExponentialHypothesis : ITwiceDifferentiableHypothesis<double>
    {
        /// <summary>
        /// The initial coefficients
        /// </summary>
        [NotNull]
        private readonly Vector<double> _initialCoefficients;

        /// <summary>
        /// Gets the initial coefficients.
        /// </summary>
        /// <returns>Vector&lt;System.Double&gt;.</returns>
        public Vector<double> GetInitialCoefficients()
        {
            return _initialCoefficients;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnivariateExponentialHypothesis"/> class.
        /// </summary>
        /// <param name="initialCoefficients">The initial coefficients.</param>
        public UnivariateExponentialHypothesis([NotNull] Vector<double> initialCoefficients)
        {
            _initialCoefficients = initialCoefficients;
        }

        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="inputs"/> and the <paramref name="coefficients"/>.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="inputs">The inputs.</param>
        /// <returns>Vector&lt;TData&gt;.</returns>
        public Vector<double> Evaluate(Vector<double> coefficients, Vector<double> inputs)
        {
            Debug.Assert(inputs.Count == 1, "inputs.Count == 1");
            Debug.Assert(coefficients.Count == 3, "coefficients.Count == 3");

            // TODO: Implement MIMO version of this function (coefficient matrix)

            var input = inputs[0];

            var offset = coefficients[0];
            var a = coefficients[1];
            var b = coefficients[2];

            var result = a * Math.Pow(input, b) + offset;
            return Vector<double>.Build.Dense(1, result);
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
            Debug.Assert(coefficients.Count == 3, "coefficients.Count == 3");

            var input = locations[0];

            var a = coefficients[1];
            var b = coefficients[2];

            // partial derivatives of the function with respect to the inputs are the coefficients
            var jacobian = a * b * Math.Pow(input, b - 1);

            return Vector<double>.Build.Dense(1, jacobian);
        }

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="locations" /> given the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="locations" />.</returns>
        public Vector<double> Hessian(Vector<double> coefficients, Vector<double> locations)
        {
            Debug.Assert(coefficients.Count == 3, "coefficients.Count == 3");

            var input = locations[0];

            var a = coefficients[1];
            var b = coefficients[2];

            // partial derivatives of the function with respect to the inputs are the coefficients
            var hessian = a * (b - 1) * b * Math.Pow(input, b - 2);

            return Vector<double>.Build.Dense(1, hessian);
        }

        /// <summary>
        /// Determines the gradient of the hypothesis with respect to the <paramref name="locations" /> given the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate" />.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="locations" />.</returns>
        public Vector<double> Jacobian(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs)
        {
            return Jacobian(coefficients, locations);
        }

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="locations" /> given the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate" />.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="locations" />.</returns>
        public Vector<double> Hessian(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs)
        {
            return Hessian(coefficients, locations);
        }

        #endregion Partial derivatives with respect to the inputs

        #region Partial derivatives with respect to the coefficients

        /// <summary>
        /// Determines the gradient of the hypothesis with respect to the <paramref name="coefficients" /> given the <paramref name="locations" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientJacobian(Vector<double> coefficients, Vector<double> locations)
        {
            Debug.Assert(coefficients.Count == 3, "coefficients.Count == 3");

            var input = locations[0];

            var offset = coefficients[0];
            var a = coefficients[1];
            var b = coefficients[2];

            // partial derivatives of the function with respect to the inputs are the coefficients
            var jacobian = Vector<double>.Build.Dense(3);
            jacobian[0] = Math.Pow(input, b);
            jacobian[1] = a * Math.Pow(input, b) * Math.Log(input);
            jacobian[2] = 1;

            return jacobian;
        }

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="coefficients" /> given the <paramref name="locations" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientHessian(Vector<double> coefficients, Vector<double> locations)
        {
            Debug.Assert(coefficients.Count == 3, "coefficients.Count == 3");

            var input = locations[0];

            var offset = coefficients[0];
            var a = coefficients[1];
            var b = coefficients[2];

            // partial derivatives of the function with respect to the inputs are the coefficients
            var hessian = Vector<double>.Build.Dense(3);
            hessian[0] = 0;
            hessian[1] = a * Math.Pow(input, b) * Math.Log(input) * Math.Log(input);
            hessian[2] = 0;

            return hessian;
        }

        /// <summary>
        /// Determines the gradient of the hypothesis with respect to the <paramref name="coefficients" /> given the <paramref name="locations" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate" />.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientJacobian(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs)
        {
            return CoefficientJacobian(coefficients, locations);
        }

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="coefficients" /> given the <paramref name="locations" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate" />.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientHessian(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs)
        {
            return CoefficientHessian(coefficients, locations);
        }

        #endregion Partial derivatives with respect to the coefficients
    }
}
