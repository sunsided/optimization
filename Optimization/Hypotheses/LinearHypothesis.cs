using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization.Hypotheses
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

            // TODO: Implement MIMO version of this function (coefficient matrix)

            // coefficients[0] is the offset
            var offset = coefficients[0];

            // coefficients[1..end] are the weights
            var weights = coefficients.SubVector(1, _ninputs);

            var result = inputs * weights + offset;
            return Vector<double>.Build.Dense(1, result);
        }

        /// <summary>
        /// Gradients the specified coefficients.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The locations.</param>
        /// <returns>MathNet.Numerics.LinearAlgebra.Vector&lt;System.Double&gt;.</returns>
        public Vector<double> Gradient(Vector<double> coefficients, Vector<double> locations)
        {
            // TODO: Implement MIMO (coefficient matrix) version of this function
            return coefficients.MapIndexed((i, v) =>
                i == 0
                ? 1 // the offset
                : locations[i - 1] // input vector is shorter by one entry
                );
        }

        /// <summary>
        /// Derivatives the specified inputs.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The locations.</param>
        /// <param name="outputs">The outputs.</param>
        /// <returns>Vector&lt;System.Double&gt;.</returns>
        public Vector<double> Gradient(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs)
        {
            return Gradient(coefficients, locations);
        }

        /// <summary>
        /// Laplacians the specified coefficients.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The locations.</param>
        /// <returns>MathNet.Numerics.LinearAlgebra.Vector&lt;System.Double&gt;.</returns>
        public Vector<double> Laplacian(Vector<double> coefficients, Vector<double> locations)
        {
            // TODO: Implement MIMO (coefficient matrix) version of this function
            return coefficients.Map(v => 0D);
        }

        /// <summary>
        /// Laplacians the specified coefficients.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="locations">The locations.</param>
        /// <param name="outputs">The outputs.</param>
        /// <returns>MathNet.Numerics.LinearAlgebra.Vector&lt;System.Double&gt;.</returns>
        public Vector<double> Laplacian(Vector<double> coefficients, Vector<double> locations, Vector<double> outputs)
        {
            return Laplacian(coefficients, locations);
        }
    }
}
