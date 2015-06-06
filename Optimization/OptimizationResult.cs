using System;

namespace widemeadows.Optimization
{
    /// <summary>
    /// Results of the optimization task.
    /// </summary>
    public class OptimizationResult<TData> : IOptimizationResult<TData>
        where TData : struct, IEquatable<TData>, IFormattable
    {
    }
}
