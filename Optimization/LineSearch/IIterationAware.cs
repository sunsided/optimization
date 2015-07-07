namespace widemeadows.Optimization.LineSearch
{
    /// <summary>
    /// Interface for line search algorithms that adapt behavior
    /// in later iterations.
    /// </summary>
    public interface IIterationAware
    {
        /// <summary>
        /// Sets the iteration.
        /// </summary>
        /// <param name="i">The i.</param>
        void SetIteration(int i);
    }
}
