using Newtonsoft.Json;
using System.Collections.Generic;

namespace Xrpl.Client.Models.Methods
{
    public class SubscribeRequest : RippleRequest
    {
        public SubscribeRequest()
        {
            Command = "subscribe";
        }

        [JsonProperty("streams")]
        public List<string> Streams { get; set; }
    }
}
