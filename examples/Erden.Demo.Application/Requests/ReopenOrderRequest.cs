using Newtonsoft.Json;

namespace Erden.Demo.Application.Requests
{
    public class ReopenOrderRequest
    {
        [JsonProperty("new_count")]
        public int NewCount { get; set; }
    }
}