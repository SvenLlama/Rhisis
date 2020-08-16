namespace Rhisis.Core.Common
{
    public class Range<T> where T : struct
    {
        /// <summary>
        /// Gets or sets the minimum value of the range.
        /// </summary>
        public T Minimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the range.
        /// </summary>
        public T Maximum { get; set; }

        /// <summary>
        /// Creates a new <see cref="Range{T}"/> instance.
        /// </summary>
        public Range()
            : this(default, default)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Range{T}"/> instance.
        /// </summary>
        /// <param name="minimum">Minimum value.</param>
        /// <param name="maximum">Maximum value.</param>
        public Range(T minimum, T maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }
    }
}
