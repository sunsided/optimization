namespace widemeadows.Optimization.GradientDescent
{
    /// <summary>
    /// Interface IGradientDescent
    /// </summary>
    public interface IGradientDescent
    {
        /// <summary>
        /// Gets or sets the maximum number of iterations.
        /// </summary>
        /// <value>The maximum iterations.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">The value must be positive</exception>
        int MaxIterations { get; set; }

        /// <summary>
        /// Gets or sets the gradient change threshold. If the gradient change
        /// is less than the given threshold, iteration stops immediately.
        /// </summary>
        /// <value>The gradient change threshold.</value>
        /// <exception cref="System.NotFiniteNumberException">The value must be finite</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The value must be nonnegative</exception>
        double GradientChangeThreshold { get; set; }
    }
}