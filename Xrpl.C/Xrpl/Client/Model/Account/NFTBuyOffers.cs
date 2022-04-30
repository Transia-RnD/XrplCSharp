using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;

namespace Xrpl.Client.Model.Account
{
    public class NFTSellOffers
    {

        [JsonProperty("offers")]
        public List<NFTOffer> Offers { get; set; }

        [JsonProperty("nft_id")]
        public string TokenID { get; set; }
    }

    // public class NFTOffer...
}
