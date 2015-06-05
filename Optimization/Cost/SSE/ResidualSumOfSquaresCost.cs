using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization.Cost.SSE
{
    /// <summary>
    /// Class ResidualSumOfSquaresCost.
    /// </summary>
    public sealed class ResidualSumOfSquaresCost : ICostGradient<double>
    {
        /// <summary>
        /// Gets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public double Cost { get; private set; }

        /// <summary>
        /// Gets the cost gradient.
        /// </summary>
        /// <value>The cost gradient.</value>
        public Vector<double> CostGradient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResidualSumOfSquaresCost"/> class.
        /// </summary>
        /// <param name="cost">The cost.</param>
        /// <param name="costGradient">The cost gradient.</param>
        public ResidualSumOfSquaresCost(double cost, [NotNull] Vector<double> costGradient)
        {
            Cost = cost;
            CostGradient = costGradient;
        }
    }
}
