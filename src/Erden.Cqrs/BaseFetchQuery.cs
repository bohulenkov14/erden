using Newtonsoft.Json;

namespace Erden.Cqrs
{
    /// <summary>
    /// Base query with pagination
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    public abstract class BaseFetchQuery<TResult> : BaseQuery<TResult> where TResult : class
    {
        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="offset">Number of items to skip</param>
        /// <param name="size">Number of items to retieve</param>
        public BaseFetchQuery(int offset, int size)
        {
            Offset = offset;
            Size = size;
        }

        /// <summary>
        /// Number of items to skip
        /// </summary>
        [JsonProperty("offset")]
        public int Offset { get; private set; }
        /// <summary>
        /// Number of items to retieve
        /// </summary>
        [JsonProperty("size")]
        public int Size { get; private set; }
    }
}