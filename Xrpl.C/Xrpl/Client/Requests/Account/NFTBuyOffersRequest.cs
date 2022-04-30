using Newtonsoft.Json;

namespace Xrpl.Client.Requests.Account
{
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
