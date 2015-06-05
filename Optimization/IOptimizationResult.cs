using System;

namespace widemeadows.Optimization
{
    /// <summary>
    /// Results of the optimization task.
    /// </summary>
    public interface IOptimizationResult<TData>
        where TData : struct, IEquatable<TData>, IFormattable
    {
    }
}
