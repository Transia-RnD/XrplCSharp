using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;
//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/nftSellOffers.ts
namespace Xrpl.Models.Methods
{
    /// <summary>
    /// Response expected from an  <see cref="NFTSellOffersRequest"/> .
    /// </summary>
    public class NFTSellOffers //todo rename to NFTSellOffersResponse extends BaseResponse
    {

        /// <summary>
        /// A list of sell offers for the specified NFToken.
        /// </summary>
        [JsonProperty("offers")]
        public List<NFTOffer> Offers { get; set; }

        /// <summary>
        /// The token ID of the NFToken to which these offers pertain.
        /// </summary>
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

    /// <summary>
    /// The `nft_sell_offers` method retrieves all of sell offers for the specified  NFToken.
    /// </summary>
    public class NFTSellOffersRequest : BaseLedgerRequest
    {
        public NFTSellOffersRequest(string nft_id)
        {
            NFTokenID = nft_id;
            Command = "nft_sell_offers";
        }

        /// <summary>
        /// The unique identifier of an NFToken.<br/>
        /// The request returns sell offers for this NFToken.
        /// </summary>
        [JsonProperty("nft_id")]
        public string NFTokenID { get; set; }
    }
}
