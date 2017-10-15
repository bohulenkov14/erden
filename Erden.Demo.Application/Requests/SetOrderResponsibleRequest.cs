using System;

using Newtonsoft.Json;

namespace Erden.Demo.Application.Requests
{
    public class SetOrderResponsibleRequest
    {
        [JsonProperty("responsible_id")]
        public Guid ResponsibleId { get; private set; }
    }
}