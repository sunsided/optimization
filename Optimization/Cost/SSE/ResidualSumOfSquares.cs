﻿using System;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization.Cost.SSE
{
    /// <summary>
    /// Class ResidualSumOfSquaresCostFunction.
    /// </summary>
    public class ResidualSumOfSquaresCostFunction : IDerivableCostFunction<double, ResidualSumOfSquaresCost>
    {
        /// <summary>
        /// The hypothesis to optimize
        /// </summary>
        [NotNull]
        private readonly IHypothesis<double> _hypothesis;

        /// <summary>
        /// The training set
        /// </summary>
        [NotNull]
        private readonly IReadOnlyCollection<DataPoint<double>> _trainingSet;

        /// <summary>
        /// Gets the training set.
        /// </summary>
        /// <value>The training set.</value>
        [NotNull]
        public IReadOnlyCollection<DataPoint<double>> TrainingSet
        {
            get { return _trainingSet; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResidualSumOfSquaresCostFunction"/> class.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        public ResidualSumOfSquaresCostFunction([NotNull] IHypothesis<double> hypothesis, [NotNull] IReadOnlyCollection<DataPoint<double>> trainingSet)
        {
            _hypothesis = hypothesis;
            _trainingSet = trainingSet;
        }

        /// <summary>
        /// Calculates the cost.
        /// </summary>
        /// <param name="coefficients">The coefficients.</param>
        /// <returns>ResidualSumOfSquaresCost.</returns>
        public ResidualSumOfSquaresCost CalculateCost(Vector<double> coefficients)
        {
            var hypothesis = _hypothesis;
            var trainingSet = _trainingSet;
            var rss = 0.0D;

            foreach (var dataPoint in trainingSet)
            {
                var inputs = dataPoint.Inputs;
                var expectedOutputs = dataPoint.Outputs;

                // evaluate the hypothesis
                var outputs = hypothesis.Evaluate(inputs, coefficients);

                // calculate the sum of the squared differences
                rss += (outputs - expectedOutputs).Map(v => v*v).Sum();
            }

            // scale by the number of training examples
            rss /= trainingSet.Count;

            // done.
            throw new NotImplementedException("Cost gradient not implemented");
            return new ResidualSumOfSquaresCost(cost: rss, costGradient: null);
        }
    }
}