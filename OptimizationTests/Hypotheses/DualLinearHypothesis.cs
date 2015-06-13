using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using widemeadows.Optimization.Hypotheses;

namespace widemeadows.Optimization.Tests.Hypotheses
{
    /// <summary>
    /// Class LinearHypothesis. This class cannot be inherited.
    /// </summary>
    public sealed class DualLinearHypothesis : IDifferentiableHypothesis<double>
    {
        /// <summary>
        /// The number of inputs
        /// </summary>
        private readonly int _ninputs;

        /// <summary>
        /// Initializes a new instance of the <see cref="DualLinearHypothesis"/> class.
        /// </summary>
        /// <param name="ninputs">The number of inputs.</param>
        public DualLinearHypothesis(int ninputs)
        {
            _ninputs = ninputs;
        }

        /// <summary>
        /// Gets the initial coefficients.
        /// </summary>
        /// <returns>Vector&lt;System.Double&gt;.</returns>
        public Vector<double> GetInitialCoefficients()
        {
            return Vector<double>.Build.Random(_ninputs + 1);
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

            // coefficients[0] is the offset
            var offset = coefficients[0];

            // coefficients[1..end] are the weights
            var weights = coefficients.SubVector(1, _ninputs);

            var result1 = inputs * weights + offset;
            var result2 = inputs * weights + 2*offset;
            return Vector<double>.Build.Dense(new []{result1, result2});
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
            Debug.Assert(coefficients.Count == locations.Count + 1, "coefficients.Count == locations.Count+1");

            // partial derivatives of the function with respect to the inputs are the coefficients
            return locations.MapIndexed(
                (i, v) => coefficients[i + 1]
                );
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
        /// Derivatives the specified inputs.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <param name="outputs">The outputs.</param>
        /// <returns>Vector&lt;System.Double&gt;.</returns>
        public Vector<double> CoefficientJacobian(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs)
        {
            return CoefficientJacobian(coefficients, locations);
        }

        /// <summary>
        /// Evaluates the hypothesis given the <paramref name="locations" /> and the <paramref name="coefficients" />.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The inputs.</param>
        /// <returns>The partial derivatives of the evaluation function with respect to the <paramref name="coefficients" />.</returns>
        public Vector<double> CoefficientJacobian(Vector<double> coefficients, Vector<double> locations)
        {
            return coefficients.MapIndexed((i, v) =>
                i == 0
                ? 1 // <-- the offset
                : locations[i - 1] // partial derivative with respect to theta(i) is input(i)
                );
        }

        #endregion Partial derivatives with respect to the coefficients
    }
}
