using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RippleDotNet.Json.Converters;

namespace RippleDotNet.Requests.Transaction
{
    public class ChannelAuthorizeRequest : RippleRequest
    {
        public ChannelAuthorizeRequest()
        {
            Command = "channel_authorize";
        }

        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }

        /// <summary>
        /// Do not send your secret to a server that you do not control or do not trust.
        /// </summary>
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("amount")]
        [JsonConverter(typeof(GenericStringConverter<ulong>))]
        public ulong Amount { get; set; }

        [JsonIgnore]
        public double RippleAmount
        {
            get => (double) Amount / 1000000;
            set => Amount =  Convert.ToUInt32(value * 1000000);
        }
    }
}
