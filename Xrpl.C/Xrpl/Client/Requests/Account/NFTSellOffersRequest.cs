using Newtonsoft.Json;

namespace Xrpl.Client.Requests.Account
{
    public class NFTSellOffersRequest : BaseLedgerRequest
    {
        public NFTSellOffersRequest(string nft_id)
        {
            NFTokenID = nft_id;
            Command = "nft_sell_offers";
        }

        [JsonProperty("nft_id")]
        public string NFTokenID { get; set; }
    }
}
