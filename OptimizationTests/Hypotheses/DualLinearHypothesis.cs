﻿using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;

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
        /// <param name="inputs">The inputs.</param>
        /// <param name="coefficients">The coefficients.</param>
        /// <returns>Vector&lt;TData&gt;.</returns>
        public Vector<double> Evaluate(Vector<double> inputs, Vector<double> coefficients)
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

        /// <summary>
        /// Derivatives the specified inputs.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <param name="coefficients">The coefficients.</param>
        /// <param name="outputs">The outputs.</param>
        /// <returns>Vector&lt;System.Double&gt;.</returns>
        public Vector<double> Derivative(Vector<double> inputs, Vector<double> coefficients, Vector<double> outputs)
        {
            // TODO: Implement MIMO version of this function
            return coefficients.MapIndexed((i, v) => 
                i == 0 
                ? 1 
                : inputs[i-1]
                );
        }
    }
}
