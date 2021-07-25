using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using MathNet.Numerics.LinearAlgebra;

namespace WideMeadows.Optimization
{
    /// <summary>
    /// Interface IDataPoint
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public readonly struct DataPoint<TData>
        where TData : struct, IEquatable<TData>, IFormattable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataPoint{TData}"/> struct.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <param name="outputs">The outputs.</param>
        public DataPoint(Vector<TData> inputs, Vector<TData> outputs)
        {
            Inputs = inputs;
            Outputs = outputs;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPoint{TData}"/> struct.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <param name="outputs">The outputs.</param>
        public DataPoint(IEnumerable<TData> inputs, IEnumerable<TData> outputs)
            : this(Vector<TData>.Build.DenseOfEnumerable(inputs), Vector<TData>.Build.DenseOfEnumerable(outputs))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPoint{TData}" /> struct.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        public DataPoint(TData input, TData output)
            : this(Vector<TData>.Build.Dense(1, input), Vector<TData>.Build.Dense(1, output))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPoint{TData}" /> struct.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        public DataPoint(IEnumerable<TData> input, TData output)
            : this(Vector<TData>.Build.DenseOfEnumerable(input), Vector<TData>.Build.Dense(1, output))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPoint{TData}" /> struct.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        public DataPoint(TData input, IEnumerable<TData> output)
            : this(Vector<TData>.Build.Dense(1, input), Vector<TData>.Build.DenseOfEnumerable(output))
        {
        }

        /// <summary>
        /// Gets the inputs.
        /// </summary>
        /// <value>The inputs.</value>
        public Vector<TData> Inputs
        {
            [Pure, DebuggerStepThrough]
            get;
        }

        /// <summary>
        /// Gets the outputs.
        /// </summary>
        /// <value>The inputs.</value>
        public Vector<TData> Outputs
        {
            [Pure, DebuggerStepThrough]
            get;
        }
    }
}
