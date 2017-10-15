using Newtonsoft.Json;

namespace Erden.Demo.Application.Requests
{
    public class CreateOrderRequest
    {
        [JsonProperty("position")]
        public string Position { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}