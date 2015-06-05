using System;
using System.Diagnostics;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace widemeadows.Optimization
{
    /// <summary>
    /// Interface IDataPoint
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public struct DataPoint<TData> 
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// The inputs
        /// </summary>
        [NotNull]
        private readonly Vector<TData> _inputs;

        /// <summary>
        /// The outputs
        /// </summary>
        [NotNull]
        private readonly Vector<TData> _outputs;

        /// <summary>
        /// Gets the inputs.
        /// </summary>
        /// <value>The inputs.</value>
        [NotNull]
        public Vector<TData> Inputs
        {
            [Pure, DebuggerStepThrough]
            get { return _inputs; }
        }

        /// <summary>
        /// Gets the outputs.
        /// </summary>
        /// <value>The inputs.</value>
        [NotNull]
        public Vector<TData> Outputs
        {
            [Pure, DebuggerStepThrough]
            get { return _outputs; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPoint{TData}"/> struct.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <param name="outputs">The outputs.</param>
        public DataPoint([NotNull] Vector<TData> inputs, [NotNull] Vector<TData> outputs)
        {
            _inputs = inputs;
            _outputs = outputs;
        }
    }
}
