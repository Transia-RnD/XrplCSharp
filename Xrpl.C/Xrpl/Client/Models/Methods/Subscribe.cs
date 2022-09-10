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

        [JsonProperty("accounts")]
        public List<string> Accounts { get; set; }
    }
}
