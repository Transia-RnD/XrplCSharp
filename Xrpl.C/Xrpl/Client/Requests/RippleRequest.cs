using System;
using Newtonsoft.Json;

namespace Xrpl.Client.Requests
{
    public class RippleRequest
    {
        public RippleRequest()
        {
            Id = Guid.NewGuid();
        }

        public RippleRequest(Guid id)
        {
            Id = id;
        }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("command")]
        public string Command { get; set; }
    }
}
