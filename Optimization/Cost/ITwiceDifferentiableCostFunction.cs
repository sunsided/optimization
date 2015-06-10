using System;

namespace widemeadows.Optimization.Cost
{
    /// <summary>
    /// Interface <see cref="ITwiceDifferentiableCostFunction{TData,TCost}" />
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TCost">The type of the cost function result.</typeparam>
    public interface ITwiceDifferentiableCostFunction<TData, out TCost> : ICostFunction<TData, TCost>
        where TData : struct, IFormattable, IEquatable<TData>
        where TCost : ICostGradient2<TData>
    {
    }
}
