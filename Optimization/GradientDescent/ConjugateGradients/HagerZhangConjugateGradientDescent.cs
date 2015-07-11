using System;
using widemeadows.Optimization.Cost;
using widemeadows.Optimization.LineSearch;

namespace widemeadows.Optimization.GradientDescent.ConjugateGradients
{
    /// <summary>
    /// Implements Hager-Zhang Conjugate Gradient Descent (CG_DESCENT)
    /// </summary>
    class HagerZhangConjugateGradientDescent : IGradientDescent, IMinimization<double, IDifferentiableCostFunction<double>>
    {
        /// <summary>
        /// The line search
        /// </summary>
        private readonly HagerZhangLineSearch _lineSearch = new HagerZhangLineSearch();

        /// <summary>
        /// The maximum number of iterations
        /// </summary>
        private int _maxIterations = 10000;

        /// <summary>
        /// The error tolerance
        /// </summary>
        private double _errorTolerance = 1E-6D;

        /// <summary>
        /// Gets or sets the maximum number of iterations.
        /// </summary>
        /// <value>The maximum iterations.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">value;Maximum number of iterations must be positive.</exception>
        public int MaxIterations
        {
            get
            {
                return _maxIterations;
            }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("value", value, "Maximum number of iterations must be positive.");
                _maxIterations = value;
            }
        }

        /// <summary>
        /// Gets or sets the cost change threshold. If the cost change
        /// is less than the given threshold, iteration stops immediately.
        /// </summary>
        /// <value>The cost change threshold.</value>
        public double ErrorTolerance
        {
            get { return _errorTolerance; }
            set { _errorTolerance = value; }
        }

        /// <summary>
        /// Minimizes the specified problem.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns>IOptimizationResult&lt;TData&gt;.</returns>
        public IOptimizationResult<double> Minimize(IOptimizationProblem<double, IDifferentiableCostFunction<double>> problem)
        {
            throw new NotImplementedException();
        }
    }
}
