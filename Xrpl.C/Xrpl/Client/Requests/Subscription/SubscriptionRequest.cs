using Newtonsoft.Json;
using System.Collections.Generic;

namespace Xrpl.Client.Requests.Ledger
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
