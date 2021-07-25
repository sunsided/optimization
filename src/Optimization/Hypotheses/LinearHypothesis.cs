using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;

namespace WideMeadows.Optimization.Hypotheses
{
    /// <summary>
    /// Class LinearHypothesis. This class cannot be inherited.
    /// </summary>
    public sealed class LinearHypothesis : ITwiceDifferentiableHypothesis<double>
    {
        /// <summary>
        /// The number of inputs
        /// </summary>
        private readonly int _ninputs;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearHypothesis"/> class.
        /// </summary>
        /// <param name="ninputs">The number of inputs.</param>
        public LinearHypothesis(int ninputs)
        {
            _ninputs = ninputs;
        }

        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="inputs"/> and the <paramref name="coefficients"/>.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="inputs">The inputs.</param>
        /// <returns>Vector&lt;TData&gt;.</returns>
        public Vector<double> Evaluate(Vector<double> coefficients, Vector<double> inputs)
        {
            Debug.Assert(inputs.Count == _ninputs, "inputs.Count == _ninputs");
            Debug.Assert(inputs.Count == coefficients.Count - 1, "inputs.Count == coefficients.Count - 1");

            // TODO: Implement MIMO version of this function (coefficient matrix)

            // coefficients[0] is the offset
            var offset = coefficients[0];

            // coefficients[1..end] are the weights
            var weights = coefficients.SubVector(1, _ninputs);

            var result = inputs * weights + offset;
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
            Debug.Assert(coefficients.Count == locations.Count+1, "coefficients.Count == locations.Count+1");

            // partial derivatives of the function with respect to the inputs are the coefficients
            return locations.MapIndexed((i, v) => coefficients[i + 1]);
        }

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="locations" /> given the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="locations" />.</returns>
        public Vector<double> Hessian(Vector<double> coefficients, Vector<double> locations)
        {
            // second partial derivatives of the function with respect to the inputs is the zero vector
            return locations.Map(v => 0D);
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
        /// Determines the gradient of the hypothesis with respect to the <paramref name="coefficients" /> given the <paramref name="locations" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientJacobian(Vector<double> coefficients, Vector<double> locations)
        {
            // TODO: Implement MIMO (coefficient matrix) version of this function
            return coefficients.MapIndexed((i, v) =>
                i == 0
                ? 1 // <-- the offset
                : locations[i - 1] // partial derivative with respect to theta(i) is input(i)
                );
        }

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="coefficients" /> given the <paramref name="locations" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientHessian(Vector<double> coefficients, Vector<double> locations)
        {
            // TODO: Implement MIMO (coefficient matrix) version of this function
            return coefficients.Map(v => 0D);
        }

        /// <summary>
        /// Determines the gradient of the hypothesis with respect to the <paramref name="coefficients" /> given the <paramref name="locations" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate" />.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientJacobian(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs) => CoefficientJacobian(coefficients, locations);

        /// <summary>
        /// Determines the second derivative of the hypothesis with respect to the <paramref name="coefficients" /> given the <paramref name="locations" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs of <see cref="IHypothesis{TData}.Evaluate" />.</param>
        /// <returns>The second partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientHessian(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs) => CoefficientHessian(coefficients, locations);

        #endregion Partial derivatives with respect to the coefficients
    }
}
