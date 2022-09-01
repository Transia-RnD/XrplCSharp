using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Common;

namespace Xrpl.Client.Models.Methods
{
    public class TakerAmount
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }
    }
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
        public TakerAmount TakerGets { get; set; }

        [JsonProperty("taker_pays")]
        public TakerAmount TakerPays { get; set; }
    }
}
