using Newtonsoft.Json;
using Xrpl.Client.Models.Common;

namespace Xrpl.Client.Models.Methods
{
    public class BookOffersRequest : BaseLedgerRequest
    {
        public BookOffersRequest()
        {
            Command = "book_offers";
        }

        [JsonProperty("limit")]
        public uint? Limit { get; set; }

        [JsonProperty("taker")]
        public string Taker { get; set; }

        [JsonProperty("taker_gets")]
        public Currency TakerGets { get; set; }

        [JsonProperty("taker_pays")]
        public Currency TakerPays { get; set; }
    }
}
