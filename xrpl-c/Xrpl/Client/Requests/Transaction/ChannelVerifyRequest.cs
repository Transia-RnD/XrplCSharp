using System;
using Newtonsoft.Json;
using RippleDotNet.Json.Converters;

namespace RippleDotNet.Requests.Transaction
{
    public class ChannelVerifyRequest : RippleRequest
    {
        public ChannelVerifyRequest()
        {
            Command = "channel_verify";
        }

        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("public_key")]
        public string PublicKey { get; set; }
        
        [JsonProperty("amount")]
        [JsonConverter(typeof(GenericStringConverter<ulong>))]
        public ulong Amount { get; set; }

        [JsonIgnore]
        public double RippleAmount
        {
            get => (double)Amount / 1000000;
            set => Amount = Convert.ToUInt32(value * 1000000);
        }
    }
}
