using System;
using Newtonsoft.Json;

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
        public string Type { get; set; }

        [JsonProperty("result")]
        public object Result { get; set; }
    }    
}
