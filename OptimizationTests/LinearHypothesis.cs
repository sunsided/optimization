using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization.Tests
{
    /// <summary>
    /// Class LinearHypothesis. This class cannot be inherited.
    /// </summary>
    public sealed class LinearHypothesis : IDerivableHypothesis<double>
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
        /// <param name="inputs">The inputs.</param>
        /// <param name="coefficients">The coefficients.</param>
        /// <returns>Vector&lt;TData&gt;.</returns>
        public Vector<double> Evaluate(Vector<double> inputs, Vector<double> coefficients)
        {
            Debug.Assert(inputs.Count == _ninputs, "inputs.Count == _ninputs");
            Debug.Assert(inputs.Count == coefficients.Count - 1, "inputs.Count == coefficients.Count - 1");

            // coefficients[0] is the offset
            // coefficients[1..end] are the weights
            var weightedInputs = inputs.MapIndexed((index, value) => value*coefficients[index+1]);
            var result = weightedInputs.Sum() + coefficients[0];
            return Vector<double>.Build.Dense(1, result);
        }

        /// <summary>
        /// Derivatives the specified inputs.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="outputs">The outputs.</param>
        /// <returns>Vector&lt;System.Double&gt;.</returns>
        public Vector<double> Derivative(Vector<double> inputs, Vector<double> coefficients, Vector<double> outputs)
        {
            return Vector<double>.Build.Dense(coefficients.Count, 1.0D);
        }
    }
}
