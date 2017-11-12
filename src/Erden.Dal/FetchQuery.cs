using Newtonsoft.Json;

namespace Erden.Dal
{
    /// <summary>
    /// Base fetch request with pagination
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    public abstract class FetchQuery<TResult> : IFetchRequest<TResult>
        where TResult : class
    {
        /// <summary>
        /// Initialization with params for pagination
        /// </summary>
        /// <param name="offset">Offset</param>
        /// <param name="size">Size</param>
        public FetchQuery(int offset, int size)
        {
            Offset = offset;
            Size = size;
        }

        /// <summary>
        /// Offset
        /// </summary>
        [JsonProperty("offset")]
        public int Offset { get; }
        /// <summary>
        /// Size
        /// </summary>
        [JsonProperty("size")]
        public int Size { get; }
    }
}