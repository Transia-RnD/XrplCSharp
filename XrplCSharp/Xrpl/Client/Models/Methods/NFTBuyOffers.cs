using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;

namespace Xrpl.Client.Models.Methods
{
    public class NFTSellOffers
    {

        [JsonProperty("offers")]
        public List<NFTOffer> Offers { get; set; }

        [JsonProperty("nft_id")]
        public string TokenID { get; set; }
    }

    public class NFTBuyOffersRequest : BaseLedgerRequest
    {
        public NFTBuyOffersRequest(string nft_id)
        {
            NFTokenID = nft_id;
            Command = "nft_buy_offers";
        }

        [JsonProperty("nft_id")]
        public string NFTokenID { get; set; }
    }
}
