using Newtonsoft.Json;

using System.Collections.Generic;
//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/nftBuyOffers.ts
namespace Xrpl.Models.Methods
{
    /// <summary>
    /// Response expected from an  <see cref="NFTBuyOffersRequest"/> ..
    /// </summary>
    public class NFTBuyOffers //todo rename to  NFTBuyOffersResponse extends BaseResponse 
    {

        /// <summary>
        /// A list of buy offers for the specified NFToken.
        /// </summary>
        [JsonProperty("offers")]
        public List<NFTOffer> Offers { get; set; }

        /// <summary>
        /// The token ID of the NFToken to which these offers pertain.
        /// </summary>
        [JsonProperty("nft_id")]
        public string TokenID { get; set; }
    }

    /// <summary>
    /// The `nft_buy_offers` method retrieves all of buy offers for the specified  NFToken.
    /// </summary>
    public class NFTBuyOffersRequest : BaseLedgerRequest
    {
        public NFTBuyOffersRequest(string nft_id)
        {
            NFTokenID = nft_id;
            Command = "nft_buy_offers";
        }

        /// <summary>
        /// The unique identifier of an NFToken.<br/>
        /// The request returns buy offers for this NFToken.
        /// </summary>
        [JsonProperty("nft_id")]
        public string NFTokenID { get; set; }
    }
}
