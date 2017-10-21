using Newtonsoft.Json;

namespace Erden.Cqrs
{
    public abstract class BaseFetchQuery<TResult> : BaseQuery<TResult> where TResult : class
    {
        public BaseFetchQuery(int offset, int size)
        {
            Offset = offset;
            Size = size;
        }

        [JsonProperty("offset")]
        public int Offset { get; private set; }
        [JsonProperty("size")]
        public int Size { get; private set; }
    }
}