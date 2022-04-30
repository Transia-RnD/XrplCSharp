using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;

namespace Xrpl.Client.Model.Account
{
    public class NFTBuyOffers
    {

        [JsonProperty("offers")]
        public List<NFTOffer> Offers { get; set; }

        [JsonProperty("nft_id")]
        public string TokenID { get; set; }
    }

    public class NFTOffer
    {
        [JsonProperty("amount")]
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        [JsonProperty("flags")]
        public uint Flags { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("destination")]
        public string Destination { get; set; }

        [JsonProperty("expiration")]
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
    }
}
