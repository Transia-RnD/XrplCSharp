using Newtonsoft.Json;

using System;

using xrpl_c.Xrpl.Client.Models.Subscriptions;

namespace Xrpl.Client.Models.Transactions
{
    public class RippleResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("type")]
        public ResponseStreamType Type { get; set; }

        [JsonProperty("result")]
        public object Result { get; set; }
    }
}
